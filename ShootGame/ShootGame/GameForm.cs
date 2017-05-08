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
        //private static Image fonImage;

        private Level currentLevel;
        private Timer timer;
        private Timer timer2;
        private int iterationIndex;

        private int animation;
        private int cooldown;
        private int timeCount;

        private readonly bool[] heroMove = new bool[4];
        private bool bulletMove = false;

        private readonly Size mapSize = new Size(1023, 768);
        private Point MousePos = new Point(MousePosition.X, MousePosition.Y);

        public void InitializeForm()
        {
            Cursor.Hide();
            ClientSize = mapSize;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;

            //fonImage = Image.FromFile("Images/background0.jpg");
            BackgroundImage = Image.FromFile("Images/background0.jpg");
            DoubleBuffered = true;
            KeyPreview = true;

            MouseMove += (sender, e) => MousePos = e.Location;
            KeyDown += (sender, e) => HandleKey(e.KeyCode, true);
            KeyUp += (sender, e) => HandleKey(e.KeyCode, false);
            MouseDown += (sender, e) => HandleKey(e.Button,true);
            MouseUp += (sender, e) => HandleKey(e.Button, false);
            Paint += (sender, e) => DrawTo(e.Graphics);
        }       

        public GameForm(IEnumerable<Level> levels)
        {           
            hero = GetImage(Heros[0]);
            aim = GetImage("aim");
            bulletIMG = GetImage("bullet1");

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
                hero = GetImage(Heros[++animation / 5 % 5]);
            }
            else hero = GetImage(Heros[animation = 0]);

            currentLevel.RotateHero(MousePos);
            timeCount += timer.Interval;
            if (bulletMove && timeCount > 130)
            {
                timeCount = 0;
                var bull = new Bullet(new Vector(currentLevel.Hero.Location), currentLevel.Hero.Direction);
                Paint += (sender1, a) => DrawBullet(a.Graphics, bull);
            }
            foreach (var bullet in Bullet.Bullets)
            {
                bullet.Move();
            }

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

        private void HandleKey(MouseButtons e, bool isAcive)
        {
            if (e == MouseButtons.Left) bulletMove = isAcive;
            //if (e == MouseButtons.Right) Paint -= DrawBullet;
        }

        private void DrawBullet(Graphics g, Bullet bull)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            
            var matrix = g.Transform;

            g.TranslateTransform((float)bull.Location.X, (float)bull.Location.Y);
            g.RotateTransform(90 + (float)(bull.Direction * 180 / Math.PI));
            g.FillEllipse(Brushes.Red, -bulletIMG.Width/2+20, -bulletIMG.Height/2+20, 20, 20);
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
                g.DrawImage(hero, -hero.Width/2, -hero.Height/2);
            }
            g.Transform = matrix;
            g.DrawImage(aim, MousePos.X - aim.Width/2, MousePos.Y - aim.Height/2);
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
