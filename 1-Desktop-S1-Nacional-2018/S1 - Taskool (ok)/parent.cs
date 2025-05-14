using GabrielForm.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GabrielForm
{
    public partial class parent : Form
    {
        public parent()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
        }
        public dbTarefasEntities ctx = new dbTarefasEntities();

        private void parent_Load(object sender, EventArgs e)
        {
            if (UserData.user != null)
            {
                if (UserData.user.Senha != null)
                {
                    var cor = UserData.user.Senha.Replace("#", "");
                    panel1.BackColor = ColorTranslator.FromHtml($"{UserData.user.Senha}");
                }
            }
            PutStyle(panel1);
        }

        private void PutStyle(Control panel)
        {
            foreach (Control c in panel.Controls)
            {
                if (c.HasChildren)
                    PutStyle(c);

                if (c is ComboBox cmb)
                    cmb.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }
    }
}
