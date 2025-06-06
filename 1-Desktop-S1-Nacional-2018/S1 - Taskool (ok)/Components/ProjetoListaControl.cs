﻿using GabrielForm.Models;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GabrielForm.Components
{
    public partial class ProjetoListaControl : UserControl
    {
        dbTarefasEntities ctx = new dbTarefasEntities();

        public ProjetoListaControl(Projeto proj)
        {
            InitializeComponent();

            Tag = proj;

            label1.Text = proj.Nome;
            var qtd = ctx.Projeto_Tarefas.Where(p => p.CodProjeto == proj.Codigo && p.isConcluida == false).Count().ToString();
            label2.Text = qtd;

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
