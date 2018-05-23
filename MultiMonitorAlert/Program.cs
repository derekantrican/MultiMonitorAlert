using System;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace MultiMonitorAlert
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SystemSounds.Exclamation.Play(); //Todo: should work off a setting

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Dialog(args.First(), true));
        }
    }
}
