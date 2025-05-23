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
    public partial class ProjetoListaControl : UserControl
    {
        public ProjetoListaControl(Projeto proj)
        {
            InitializeComponent();

            Tag = proj;

            label1.Text = proj.Nome;
            label2.Text = proj.Projeto_Tarefas.Count.ToString();

            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => OnClick(e);
            }

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
