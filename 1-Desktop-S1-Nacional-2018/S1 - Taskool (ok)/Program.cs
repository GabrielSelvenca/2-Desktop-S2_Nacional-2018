using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabrielForm
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
      {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static DialogResult Alert(this string msg)
        {
            return MessageBox.Show(msg, "Taskool - Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static DialogResult Information(this string msg)
        {
            return MessageBox.Show(msg, "Taskool - Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static DialogResult Question(this string msg)
        {
            return MessageBox.Show(msg, "Taskool - Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
