using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using static ShootGame.Hero;

namespace ShootGame
{
    sealed class GameForm : Form
    {
        private Image hero;
        private Image aim;
        private Image bulletIMG;

        private Enemy enemy;
        private Bullet bullet;

        private Weapon weapon;
        //private static Image fonImage;

        private Level currentLevel;
        private Timer timer;
        private int iterationIndex;

        private int animation;
        private int cooldown;
        private int timeCount;

        private readonly bool[] heroMove = new bool[4];
        private bool bulletMove;

        private readonly Size mapSize = new Size(1023, 768);
        private Point MousePos = new Point(MousePosition.X, MousePosition.Y);

        private static Random rnd = new Random();

        public void InitializeForm()
        {
            Cursor.Hide();
            ClientSize = mapSize;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;

            //fonImage = Image.FromFile("Images/background0.jpg");
            BackgroundImage = GetImage("lvl1");
            BackgroundImageLayout = ImageLayout.None;
            DoubleBuffered = true;
            KeyPreview = true;

            MouseMove += (sender, e) => MousePos = e.Location;
            KeyDown += (sender, e) => HandleKey(e.KeyCode, true);
            KeyUp += (sender, e) => HandleKey(e.KeyCode, false);
            MouseDown += (sender, e) => HandleKey(e.Button, true);
            MouseUp += (sender, e) => HandleKey(e.Button, false);
            MouseWheel += (sender, e) => weapon = (Weapon)(((int)weapon + e.Delta / 120 + 3) % 3);
            Paint += (sender, e) => DrawTo(e.Graphics);
        }       

        public GameForm(IEnumerable<Level> levels)
        {
            weapon = default(Weapon);
            hero = GetImage(Heroes[MainHero.Halloween][weapon][0]);
            aim = GetImage("aim");
            bulletIMG = GetImage("bullRED");
            

            timer = new Timer { Interval = 10 };
            timer.Tick += TimerTick;
            timer.Start();            

            foreach (var level in levels)
            {
                if (currentLevel == null) currentLevel = level;
            }
        }

        private void ChangeLevel(Level newSpace)
        {
            currentLevel = newSpace;
            currentLevel.Reset();
            timer.Start();
            iterationIndex = 0;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (currentLevel == null) return;

            if (!heroMove.All(x => !x))
            {
                currentLevel.MoveHero(mapSize, heroMove);
                hero = GetImage(Heroes[MainHero.Halloween][weapon][++animation / 5 % 5]);
            }
            else hero = GetImage(Heroes[MainHero.Halloween][weapon][animation = 0]);

            currentLevel.RotateHero(MousePos);

            timeCount += timer.Interval;////////не забыть
            
            if (bulletMove && timeCount % 100 == 0)
                bullet = new Bullet(currentLevel.Hero.Location, 10, currentLevel.Hero.Direction, 5);

            foreach (var bull in Bullet.Bullets.ToList())
                bull.Move();

            if (timeCount % 300 == 0) enemy = new Enemy(RandomName(), RandomStartLocation());

            foreach (var enem in Enemy.Enemies.ToList())
            {
                enem.Move(currentLevel.Hero, timeCount);
                if ((enem.Location - currentLevel.Hero.Location).Length < 30 && timeCount % 500 == 0)
                    currentLevel.Hero.Health -= enem.Damage;
            }
            if (currentLevel.IsDead) timer.Stop();
            Invalidate();
            Update();
        }

        private void HandleKey(Keys e, bool isActive)
        {
            if (e == Keys.W) heroMove[(int)Step.Up] = isActive;           
            if (e == Keys.A) heroMove[(int)Step.Left] = isActive;
            if (e == Keys.D) heroMove[(int)Step.Right] = isActive;
            if (e == Keys.S) heroMove[(int)Step.Down] = isActive;

            if (e == Keys.D1) weapon = Weapon.UZI;
            if (e == Keys.D2) weapon = Weapon.Shotgun;
            if (e == Keys.D3) weapon = Weapon.Plasmagun;

            if (e == Keys.Escape) Close();
        }

        private void HandleKey(MouseButtons e, bool isAcive)
        {
            if (e == MouseButtons.Left) bulletMove = isAcive;
            //if (e == MouseButtons.Right) Paint -= DrawBullet;
        }

        private Vector RandomStartLocation()
        {
            var a = mapSize.Width;
            var b = mapSize.Height;
            var perimetr = (a + b) * 2;
            var n = rnd.Next(perimetr);

            if (n < a) return new Vector(n, -20);
            if (n - a < b) return new Vector(a + 20, n - a);
            if (n - a - b < a) return new Vector(n - a - b, b + 20);
            return new Vector(0, n - 2 * a - b);
        }

        private Name RandomName()
        {
            var n = rnd.Next(4);
            if (n == 0) return global::Name.robot0;
            if (n == 1) return global::Name.robot1;
            if (n == 2) return global::Name.robot2;
            if (n == 3) return global::Name.robot3;
            return global::Name.monstr;
        }

        private void DrawTo(Graphics g)
        {
            if (currentLevel == null) return;
            g.SmoothingMode = SmoothingMode.HighQuality;
            var matrix = g.Transform;

            foreach (var blood in Enemy.Blood)
            {
                DrawObj(g, matrix, blood.Location, blood.Direction, blood.EnemyIMG);
            }

            if (timer.Enabled)
            {
                DrawObj(g, matrix, currentLevel.Hero.Location, currentLevel.Hero.Direction, hero);
            }

            foreach (var enem in Enemy.Enemies)
            {
                DrawObj(g, matrix, enem.Location, enem.Direction, enem.EnemyIMG);
            }               
            foreach (var bull in Bullet.Bullets)
            {
                DrawObj(g, matrix, bull.Location, bull.Direction, bulletIMG);                            
            }
            g.Transform = matrix;
            g.DrawImage(aim, MousePos.X - aim.Width/2, MousePos.Y - aim.Height/2);
        }

        private void DrawObj(Graphics g, Matrix matrix, Vector loc, double dir, Image img)
        {
            g.Transform = matrix;
            g.TranslateTransform((float)loc.X, (float)loc.Y);
            g.RotateTransform(90 + (float)(dir * 180 / Math.PI));
            g.DrawImage(img, -img.Width/2, -img.Height/2);
        }
    }   
}

public enum Level
{
    One,
    Two,
    Three,
    Four,
    Five
}

public enum Step
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3
}
