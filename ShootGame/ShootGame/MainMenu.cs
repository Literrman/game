using System.Drawing;
using System.Net;
using System.Windows.Forms;
using static ShootGame.Hero;

namespace ShootGame
{
    public class MainMenu : Form
    {
        public MainMenu()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;

            BackColor = Color.Black;
            ClientSize = new Size(400, 300);
            TopMost = true;

            var cont = new Button
            {
                Text = "Start Game",
                ForeColor = Color.Green,
                Font = new Font("Helvetica", 20),
                Size =  new Size(300, 50),
                Location = new Point(50, 120),
            };
            var exit = new Button
            {
                Text = "Exit",
                ForeColor = cont.ForeColor,
                Font = cont.Font,
                Size = cont.Size,
                Location = new Point(50, cont.Bottom)
            };
            var label = new Label
            {
                Text = "Rip 4",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkRed,
                Font = new Font("Helvetica", 30),
                Size = new Size(400, 100),
            };

            Controls.Add(cont);
            Controls.Add(exit);
            Controls.Add(label);

            cont.MouseEnter += (sender, e) => cont.ForeColor = Color.DarkRed;
            cont.MouseLeave += (sender, e) => cont.ForeColor = Color.Green;
            exit.MouseEnter += (sender, e) => exit.ForeColor = Color.DarkRed;
            exit.MouseLeave += (sender, e) => exit.ForeColor = Color.Green;

            cont.Click += (sender, e) => { ChooseHero(); Hide(); };
            exit.Click += (sender, e) => Application.Exit();
        }

        private static void ChooseHero()
        {
            var first = new Button
            {
                Text = "Halloween",
                ForeColor = Color.Green,
                Font = new Font("Helvetica", 20),
                Size = new Size(300, 50),
                Location = new Point(50, 325),
            };
            var second = new Button
            {
                Text = "Death",
                ForeColor = first.ForeColor,
                Font = first.Font,
                Size = first.Size,
            };
            var label = new Label
            {
                Text = "Choose Hero",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkRed,
                Font = new Font("Helvetica", 30),
                Size = new Size(800, 100),
            };
            var pict1 = new PictureBox
            {
                Image = GetImage("halloween"),
                SizeMode = PictureBoxSizeMode.AutoSize
            };
            var pict2 = new PictureBox
            {
                Image = GetImage("death"),
                SizeMode = PictureBoxSizeMode.AutoSize
            };
            var f2 = new Form
            {
                ClientSize = new Size(800, 500),
                BackColor = Color.Black,
                TopMost = true,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,

                Controls = { first, second, label, pict1, pict2 }
            };
            
            second.Location = new Point(f2.Right - second.Width - 50, first.Top);
            pict1.Location = new Point(f2.Left + 120, first.Top - pict1.Height - 50);
            pict2.Location = new Point(f2.Right - pict2.Width - 120, second.Top - pict2.Height - 50);
            f2.Show();
            
            first.MouseEnter += (sender, e) => first.ForeColor = Color.DarkRed;
            first.MouseLeave += (sender, e) => first.ForeColor = Color.Green;
            second.MouseEnter += (sender, e) => second.ForeColor = Color.DarkRed;
            second.MouseLeave += (sender, e) => second.ForeColor = Color.Green;

            first.Click += (sender, e) => Continue(MainHero.Halloween, f2);
            second.Click += (sender, e) => Continue(MainHero.Death, f2);
        }

        private static void Continue(MainHero hero, Form f)
        {
            var form = new GameForm(hero);
            form.InitializeForm();
            form.Show();
            f.Hide();
        }
    }
}