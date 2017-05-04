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
        //private static Image fonImage;
        private Level currentLevel;
        private Timer timer;
        private int iterationIndex;

        private int animation;
        private readonly bool[] movement = new bool[4];
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

            KeyDown += (sender, e) => HandleKey(e.KeyCode, true);
            KeyUp += (sender, e) => HandleKey(e.KeyCode, false);
            MouseMove += (sender, e) => MousePos = e.Location;
            Paint += (sender, e) => DrawTo(e.Graphics);
        }

        public GameForm(IEnumerable<Level> levels)
        {           
            hero = GetImage(Heros[0]);

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
            if (!movement.All(x => !x))
            {
                currentLevel.MoveHero(mapSize, movement);
                hero = GetImage(Heros[++animation / 5 % 5]);
            }
            else hero = GetImage(Heros[animation = 0]);
            currentLevel.RotateHero(MousePos);
            //if (currentLevel.IsCompleted) timer.Stop();
            Invalidate();
            Update();
        }

        private void HandleKey(Keys e, bool isPress)
        {
            if (e == Keys.W) movement[(int)Step.Up] = isPress;           
            if (e == Keys.A) movement[(int)Step.Left] = isPress;
            if (e == Keys.D) movement[(int)Step.Right] = isPress;
            if (e == Keys.S) movement[(int)Step.Down] = isPress;

            if (e == Keys.Escape) Close();
        }

        private void DrawTo(Graphics g)
        {           
            g.SmoothingMode = SmoothingMode.HighQuality;
            //g.DrawImage(fonImage, 0, 0);

            if (currentLevel == null) return;

            var matrix = g.Transform;

            if (timer.Enabled)
            {
                g.Transform = matrix;
                g.TranslateTransform((float)currentLevel.Hero.Location.X, (float)currentLevel.Hero.Location.Y);
                g.RotateTransform(90 + (float)(currentLevel.Hero.Direction * 180 / Math.PI));
                g.DrawImage(hero, -hero.Width / 2, -hero.Height / 2);
            }
            g.Transform = matrix;
            g.FillEllipse(Brushes.Red, MousePos.X - 4, MousePos.Y - 4, 8, 8);
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
