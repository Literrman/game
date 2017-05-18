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
            var form = new MainMenu();
            Application.Run(form);
        }
    }
}