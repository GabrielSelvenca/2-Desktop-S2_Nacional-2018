using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabrielForm.Components
{
    public partial class FavoritosControl : UserControl
    {
        public FavoritosControl(int qtdFavoritos)
        {
            InitializeComponent();

            label2.Text = qtdFavoritos.ToString();

            AdicionarCliques(this);
        }

        private void AdicionarCliques(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                ctrl.Click += (s, e) => this.OnClick(e);
                if (ctrl.HasChildren)
                {
                    AdicionarCliques(ctrl);
                }
            }
        }
    }
}
