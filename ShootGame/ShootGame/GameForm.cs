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

        private Level currentLvl;
        private Timer timer;

        private int animation;
        private int cooldown;
        private int timeCount;
        private int lvlIndex;

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
            BackgroundImage = GetImage(currentLvl.Name);
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
                if (currentLvl == null) currentLvl = level;
                if (currentLvl.IsCompleted) ChangeLevel(level);
            }
        }

        private void ChangeLevel(Level newSpace)
        {
            currentLvl = newSpace;
            currentLvl.Reset();
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (currentLvl == null) return;

            if (!heroMove.All(x => !x))
            {
                currentLvl.MoveHero(mapSize, heroMove);
                hero = GetImage(Heroes[MainHero.Halloween][weapon][++animation / 5 % 5]);
            }
            else hero = GetImage(Heroes[MainHero.Halloween][weapon][animation = 0]);

            currentLvl.RotateHero(MousePos);

            timeCount += timer.Interval;////////не забыть
            
            if (bulletMove && timeCount % 50 == 0)
                bullet = new Bullet(weapon, currentLvl.Hero.Location, currentLvl.Hero.Direction);

            foreach (var bull in Bullet.Bullets.ToList())
                bull.Move();

            if (timeCount % 300 == 0) enemy = new Enemy(RandomName(), RandomStartLocation());

            foreach (var enem in Enemy.Enemies.ToList())
            {
                enem.Move(currentLvl.Hero, timeCount);
                if ((enem.Location - currentLvl.Hero.Location).Length < 30 && timeCount % 500 == 0)
                    currentLvl.Hero.Health -= enem.Damage;
            }
            if (currentLvl.IsDead) timer.Stop();
            if (currentLvl.IsCompleted) ChangeLevel(Levels.MyLevels[++lvlIndex]);
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

        private EName RandomName()
        {
            var n = rnd.Next(4);
            if (n == 0) return EName.robot0;
            if (n == 1) return EName.robot1;
            if (n == 2) return EName.robot2;
            if (n == 3) return EName.robot3;
            return EName.monstr;
        }

        private void DrawTo(Graphics g)
        {
            if (currentLvl == null) return;
            g.SmoothingMode = SmoothingMode.HighQuality;
            var matrix = g.Transform;

            foreach (var blood in Enemy.Blood)
                DrawObj(g, matrix, blood.Location, blood.Direction, blood.EnemyIMG);

            if (timer.Enabled)
                DrawObj(g, matrix, currentLvl.Hero.Location, currentLvl.Hero.Direction, hero);

            foreach (var enem in Enemy.Enemies)
                DrawObj(g, matrix, enem.Location, enem.Direction, enem.EnemyIMG);

            foreach (var bull in Bullet.Bullets)
                DrawObj(g, matrix, bull.Location, bull.Direction, bulletIMG);
            
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

public enum Step
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3
}
