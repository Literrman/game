using System.Collections;
using System.Windows.Forms;

namespace ShootGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new GameForm(Levels.CreateLevels());
            form.KeyPreview = true;
            form.InitializeComponent();
            Application.Run(form);
        }
    }
}