using GabrielForm.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GabrielForm
{
    public partial class UserControl1 : UserControl
    {
        public enum TipoControl
        {
            Owner,
            Usuario
        }

        public UserControl1()
        {
            InitializeComponent();
        }

        public UserControl1(Usuario item, TipoControl tipo = TipoControl.Usuario)
        {
            InitializeComponent();
            Item = item;
            string[] parts = item.Nome.Split(' ');
            label1.Text = parts[0][0].ToString().ToUpper();

            if (parts.Length > 1)
                label2.Text = $"{parts[0]} {parts[1]}";
            else
                label2.Text = parts[0];

            ConfigControl(item, tipo);
        }

        private void ConfigControl(Usuario item, TipoControl tipo)
        {
            switch (tipo)
            {
                case TipoControl.Owner:
                    button1.Enabled = false;
                    button1.ForeColor = Color.Blue;
                    button1.Text = "Proprietário";
                    break;
                case TipoControl.Usuario:
                    button1.Visible = true;
                    button1.Click += Button1_Click;
                    break;

                default:
                    break;
            }
        }

        private event EventHandler<Usuario> BotaoClicado;

        private void Button1_Click(object sender, EventArgs e)
        {
            BotaoClicado?.Invoke(this, Item);
        }

        private Usuario Item { get; }
    }
}
