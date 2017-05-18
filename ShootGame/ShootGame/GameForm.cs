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

        private Weapon weapon;
        private MainHero heroName;

        private Level currentLvl;
        private Timer timer;

        private int animation;
        private int timeCount;

        private readonly bool[] heroMove = new bool[4];
        private bool bulletMove;

        private readonly Size mapSize = new Size(1023, 768);
        private Point MousePos = new Point(MousePosition.X, MousePosition.Y);

        //private static Random rnd = new Random();

        public void InitializeForm()
        {
            Cursor.Hide();
            ClientSize = mapSize;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;

            BackgroundImage = GetImage(currentLvl.Name);
            BackgroundImageLayout = ImageLayout.None;
            DoubleBuffered = true;
            KeyPreview = true;

            MouseMove += (sender, e) => MousePos = e.Location;
            KeyDown += (sender, e) => HandleKey(e.KeyCode, true);
            KeyUp += (sender, e) => HandleKey(e.KeyCode, false);
            MouseDown += (sender, e) => HandleKey(e.Button, true);
            MouseUp += (sender, e) => HandleKey(e.Button, false);
            MouseWheel += (sender, e) => ChangeWeapon(Math.Sign(e.Delta));
            Paint += (sender, e) => DrawTo(e.Graphics);
        }

        public GameForm(MainHero name)
        {
            weapon = default(Weapon);
            heroName = name;
            hero = GetImage(Heroes[heroName][weapon][0]);
            aim = GetImage("aim");
            
            timer = new Timer { Interval = 10 };
            timer.Tick += TimerTick;
            timer.Start();            

            if (currentLvl == null) currentLvl = Levels.MyLevels[0];
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (currentLvl == null) return;

            if (!heroMove.All(x => !x))
            {
                currentLvl.MoveHero(mapSize, heroMove);
                hero = GetImage(Heroes[heroName][weapon][++animation / 5 % 5]);
            }
            else hero = GetImage(Heroes[heroName][weapon][animation = 0]);

            currentLvl.Hero.CheckEnemyBull();
            currentLvl.RotateHero(MousePos);

            timeCount += timer.Interval;////////не забыть
            
            if (bulletMove) Shoot(weapon, currentLvl.Hero.Location, currentLvl.Hero.Direction);

            foreach (var bull in Bullet.Bullets.ToList())
                bull.Move(currentLvl.Hero);

            if (timeCount % 300 == 0) new Enemy(mapSize);

            foreach (var enem in Enemy.Enemies.ToList())
            {
                enem.Move(currentLvl.Hero, timeCount);
                enem.CheckIsDead(currentLvl.Hero);

                if (enem.Name == EName.robot3) Enemy.Shoot(Weapon.Rocket, enem);
            }

            if (currentLvl.IsDead) Menu("You Lose\n\rDo you want to restart?", "Yes", "No", 20);
            else if (currentLvl.IsCompleted) ChangeLevel(Levels.NextLvl());

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

            if (e == Keys.Escape) Menu("Menu","Continue", "Exit Game", 30);
        }
        private void HandleKey(MouseButtons e, bool isAcive)
        {
            if (e == MouseButtons.Left) bulletMove = isAcive;
        }
      
        private void ChangeLevel(Level newSpace)
        {
            if (currentLvl.IsWin) Menu("You Win!\n\rOpen Main Menu?", "Yes", "Exit Game", 20);
            else
            {
                currentLvl = newSpace;
                currentLvl.Reset();
                BackgroundImage = GetImage(newSpace.Name);
                timer.Start();
            }
        }

        private void ChangeWeapon(int index)
        {
            weapon = (Weapon)(((int)weapon + index + 3) % 3);
        }

        private void Menu(string labelText, string txt1, string txt2, int textSize)
        {
            var cont = new Button
            {
                Text = txt1,
                ForeColor = Color.Green,
                Font = new Font("Helvetica", 20),
                Size = currentLvl.IsDead ? new Size(150, 50) : new Size(300, 50),
                Location = currentLvl.IsDead ? new Point(50, 200) : new Point(50, 120),
            };
            var exit = new Button
            {
                Text = txt2,
                ForeColor = cont.ForeColor,
                Font = cont.Font,
                Size = cont.Size,
                Location = currentLvl.IsDead ? new Point(cont.Right, cont.Top) : new Point(50, cont.Bottom)
            };
            var label = new Label
            {
                Text = labelText,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkRed,
                Font = new Font("Helvetica", textSize),
                Size = new Size(400, 100),
            };
            var f2 = new Form
            {
                BackColor = Color.Black,
                Size = new Size(400, 300),
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true,
                Controls = { cont, label, exit }
            };
            Stop(f2);

            cont.MouseEnter += (sender, e) => cont.ForeColor = Color.DarkRed;
            cont.MouseLeave += (sender, e) => cont.ForeColor = Color.Green;
            exit.MouseEnter += (sender, e) => exit.ForeColor = Color.DarkRed;
            exit.MouseLeave += (sender, e) => exit.ForeColor = Color.Green;

            cont.Click += (sender, e) => Continue(f2);
            exit.Click += (sender, e) => Application.Exit();
        }

        private void Continue(Form f)
        {
            if (currentLvl.IsDead) currentLvl.Restart();
            if (currentLvl.IsWin) Application.Restart();
            Cursor.Hide();
            f.Close();
            timer.Start();
        }

        private void Stop(Form f)
        {
            Cursor.Show();
            f.Show();
            timer.Stop();
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
                DrawObj(g, matrix, bull.Location, bull.Direction, bull.BullIMG);
            
            g.Transform = matrix;
            g.DrawImage(aim, MousePos.X - aim.Width/2, MousePos.Y - aim.Height/2);
        }

        private static void DrawObj(Graphics g, Matrix matrix, Vector loc, double dir, Image img)
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
