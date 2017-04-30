using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShootGame.Properties.Resources;

namespace ShootGame
{
    sealed class GameForm : Form
    {
        private readonly Image hero;
        private static Image fonImage;
        private Level currentLevel;
        private Timer timer;
        private int iterationIndex;
        private bool right, left, up, down;
        private readonly Size mapSize = new Size(1023, 768);
        private double X = MousePosition.X;
        private double Y = MousePosition.Y;

        public void InitializeComponent()
        {
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = mapSize;
            FormBorderStyle = FormBorderStyle.None;
            fonImage = Image.FromFile("Images/background0.jpg");
            BackgroundImage = Image.FromFile("Images/background0.jpg");
            DoubleBuffered = true;

            KeyDown += (sender, e) => HandleKey(e.KeyCode, true);
            KeyUp += (sender, e) => HandleKey(e.KeyCode, false);
            MouseMove += (sender, e) => { X = e.X; Y = e.Y; };
        }

        public GameForm(IEnumerable<Level> levels)
        {           
            hero = Hero.GetImage(Hero.Heros[0]);

            timer = new Timer { Interval = 10 };
            timer.Tick += TimerTick;
            timer.Start();

            foreach (var level in levels)
            {
                if (currentLevel == null) currentLevel = level;
            }
            KeyPreview = true;
        }

        private void ChangeLevel(Level newSpace)
        {
            currentLevel = newSpace;
            currentLevel.Reset();
            timer.Start();
            iterationIndex = 0;
        }

        //private static void Recounter(double x, double y, double angle)
        //{
        //    var angles = ManipulatorTask.MoveManipulatorTo(x, y, angle);
        //    Form.Invalidate();
        //}

        private void TimerTick(object sender, EventArgs e)
        {
            if (currentLevel == null) return;
            //MoveMonster();
            MoveHero();
            //if (currentLevel.IsCompleted) timer.Stop();
            Invalidate();
            Update();
        }

        private void MoveHero()
        {
            var step = left ? Step.Left : right ? Step.Right : up ? Step.Up : down ? Step.Down : Step.None;
            currentLevel.Move(mapSize, step); //////////////////сделать для диагональных шагов и угол////////////////////
        }

        //public static void KeyDown1(object sender, Keys key)
        //{
        //    if (key == Keys.S) Recounter(X, Y -= 10, Angle);
        //    else if (key == Keys.A) Recounter(X -= 10, Y, Angle);
        //    else if (key == Keys.W) Recounter(X, Y += 10, Angle);
        //    else if (key == Keys.D) Recounter(X += 10, Y, Angle);
        //    else if (key == Keys.F) Recounter(X, Y, Angle -= Math.PI / 6);
        //    else if (key == Keys.R) Recounter(X, Y, Angle += Math.PI / 6);
        //    else
        //    {
        //        message = "Unknow input!";
        //        Form.Invalidate();
        //    }
        //}

        private void HandleKey(Keys e, bool isDown)
        {
            if (e == Keys.A) left = isDown;
            if (e == Keys.D) right = isDown;
            if (e == Keys.W) up = isDown;
            if (e == Keys.S) down = isDown;
            if (e == Keys.Escape) Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            //var g = Graphics.FromImage(BackgroundImage);
            DrawTo(g);
        }

        private void DrawTo(Graphics g)
        {           
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(fonImage, 0, 0);

            if (currentLevel == null) return;

            var matrix = g.Transform;

            if (timer.Enabled)
            {
                g.Transform = matrix;
                g.TranslateTransform((float)currentLevel.Hero.Location.X, (float)currentLevel.Hero.Location.Y);
                //g.RotateTransform(90 + (float)(currentLevel.Hero.Direction * 180 / Math.PI));
                g.DrawImage(hero, -hero.Width / 2, -hero.Height / 2);
            }
            g.Transform = matrix;
            g.FillEllipse(Brushes.Red, (float)X - 4, (float)Y - 4, 8, 8);
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
    None = 0,
    Left = -1,
    Right = 1,
    Up = -2,
    Down = 2
}
