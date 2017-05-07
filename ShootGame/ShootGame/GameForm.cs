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
        private Image bullet;
        //private static Image fonImage;

        private Level currentLevel;
        private Timer timer;
        private int iterationIndex;

        private int animation;
        private readonly bool[] heroMove = new bool[4];
        private readonly Size mapSize = new Size(1023, 768);
        private Point MousePos = new Point(MousePosition.X, MousePosition.Y);

        public void InitializeForm()
        {
            //Cursor.Hide();
            ClientSize = mapSize;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;

            //fonImage = Image.FromFile("Images/background0.jpg");
            BackgroundImage = Image.FromFile("Images/background0.jpg");
            DoubleBuffered = true;
            KeyPreview = true;

            KeyDown += (sender, e) => HandleKey(e.KeyCode, true);
            KeyUp += (sender, e) => HandleKey(e.KeyCode, false);
            MouseMove += (sender, e) => MousePos = e.Location;
            MouseDown += (sender, e) => HandleKey(e.Button);
            Paint += (sender, e) => DrawTo(e.Graphics);
        }       

        public GameForm(IEnumerable<Level> levels)
        {           
            hero = GetImage(Heros[0]);
            aim = GetImage("aim");
            bullet = GetImage("bullet1");

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
            //MoveMonster();
            if (!heroMove.All(x => !x))
            {
                currentLevel.MoveHero(mapSize, heroMove);
                hero = GetImage(Heros[++animation / 5 % 5]);
            }
            else hero = GetImage(Heros[animation = 0]);
            currentLevel.RotateHero(MousePos);
            //if (currentLevel.IsCompleted) timer.Stop();
            Invalidate();
            Update();
        }

        private void HandleKey(Keys e, bool isActive)
        {
            if (e == Keys.W) heroMove[(int)Step.Up] = isActive;           
            if (e == Keys.A) heroMove[(int)Step.Left] = isActive;
            if (e == Keys.D) heroMove[(int)Step.Right] = isActive;
            if (e == Keys.S) heroMove[(int)Step.Down] = isActive;

            if (e == Keys.Escape) Close();
        }

        private void HandleKey(MouseButtons e)
        {
            if (e == MouseButtons.Left) Paint += DrawBullet;
            if (e == MouseButtons.Right) Paint -= DrawBullet;
            //throw new NotImplementedException();
        }

        private void DrawBullet(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            
            var matrix = g.Transform;
            //var bullet = new Bullet(currentLevel.Hero.Location , currentLevel.Hero.Direction);

            g.TranslateTransform((float)currentLevel.Hero.Location.X, (float)currentLevel.Hero.Location.Y);
            //g.RotateTransform(90 + (float)(currentLevel.Hero.Direction * 180 / Math.PI));
            g.FillEllipse(Brushes.Red, -bullet.Width / 2+32, -bullet.Height / 2, 20, 20);
            g.Transform = matrix;
        }

        private void DrawTo(Graphics g)
        {           
            g.SmoothingMode = SmoothingMode.HighQuality;
            //g.DrawImage(fonImage, 0, 0);

            if (currentLevel == null) return;

            var matrix = g.Transform;

            if (timer.Enabled)
            {
                //g.Transform = matrix;
                g.TranslateTransform((float)currentLevel.Hero.Location.X, (float)currentLevel.Hero.Location.Y);
                g.RotateTransform(90 + (float)(currentLevel.Hero.Direction * 180 / Math.PI));
                g.DrawImage(hero, -hero.Width / 2, -hero.Height / 2);
            }
            g.Transform = matrix;
            g.DrawImage(aim, MousePos.X - aim.Width, MousePos.Y - aim.Height);
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
