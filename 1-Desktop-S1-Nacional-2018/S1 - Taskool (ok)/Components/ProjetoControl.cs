using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabrielForm.Components
{
    public partial class ProjetoControl : UserControl
    {
        dbTarefasEntities ctx = new dbTarefasEntities();

        Projeto proj;

        public ProjetoControl(Projeto projeto)
        {
            InitializeComponent();
            label1.Text = projeto.Nome;

            proj = projeto;

            var tarefas = ctx.Projeto_Tarefas
                .Where(t => t.CodProjeto == proj.Codigo && t.isConcluida == false)
                .ToList();

            if (tarefas.Count > 0)
            {
                panel5.Visible = false;
                panel5.BringToFront();
                panel4.Visible = true;
            }
            else
            {
                panel5.Visible = true;
                panel5.SendToBack();
                panel4.Visible = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox1.Image == Properties.Resources.estrela_off1 ? Properties.Resources.estrela_on1 : Properties.Resources.estrela_off1;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Adicionar uma tarefa...")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string tarefa = textBox1.Text.Trim();

            if (tarefa.Length > 0 && e.KeyChar == (char)Keys.Enter)
            {
                var proj_tarefa = new Projeto_Tarefas
                {
                    CodProjeto = proj.Codigo,
                    Descricao = tarefa,
                    isConcluida = false
                };

                ctx.Projeto_Tarefas.Add(proj_tarefa);
                ctx.SaveChanges();

                var tarefaControl = new TarefaControl(proj_tarefa);
            }
        }
    }
}
