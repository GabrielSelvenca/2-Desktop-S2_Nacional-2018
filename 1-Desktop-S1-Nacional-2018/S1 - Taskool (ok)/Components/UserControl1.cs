using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabrielForm.Components
{
    public partial class UserControl1 : UserControl
    {
        public EventHandler<Usuario> BotaoClidado;

        public UserControl1()
        {
            InitializeComponent();
        }

        public UserControl1(Usuario item)
        {
            InitializeComponent();
            Item = item;
            string[] partsName = item.Nome.Split(' ');
            string[] letras = partsName[0].Split();

            label1.Text = letras[0].ToUpper();

            if (partsName.Length > 1 )
            {
                label2.Text = $"{partsName[0]} {partsName[1]}";
            }
            else
            {
                label2.Text = partsName[0];
            }

            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BotaoClidado?.Invoke(this, Item);
        }

        public Usuario Item { get; }
    }
}
