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

        HomeForm homeForm;

        public ProjetoControl(Projeto projeto, HomeForm homeForm)
        {
            InitializeComponent();
            label1.Text = projeto.Nome;

            proj = projeto;
            this.homeForm = homeForm;

            var tarefas = ctx.Projeto_Tarefas
                .Where(t => t.CodProjeto == proj.Codigo)
                .ToList();

            if (tarefas.Count <= 0)
            {
                panel5.Visible = true;
                panel4.Visible = false;
            }
            else
            {
                panel5.Visible = false;
                panel4.Visible = true;
            }
        }

        private void ProjetoControl_Load(object sender, EventArgs e)
        {
            linkLabel1.Text = "0 TAREFAS CONCLUIDAS";
            carregarTarefas();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Tag = pictureBox1.Image == Properties.Resources.estrela_off ? Properties.Resources.estrela_on : Properties.Resources.estrela_off;
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

                carregarTarefas();
                textBox1.Text = "Adicionar uma tarefa...";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void carregarTarefas()
        {
            var tarefas = ctx.Projeto_Tarefas
                .Where(t => t.CodProjeto == proj.Codigo)
                .OrderByDescending(t => t.Codigo)
                .ToList();

            if (tarefas.Count <= 0) return;

            flowLayoutPanel1.Controls.Clear();

            int i = 0;

            foreach (var tarefa in tarefas)
            {
                if (tarefa.isConcluida == true)
                {
                    i++;
                    linkLabel1.Text = i + " TAREFAS CONCLUIDAS";
                    continue;
                }

                var tarefaControl = new TarefaControl(tarefa);
                tarefaControl.CheckBoxSelected += TarefaControl_CheckBoxSelected;
                flowLayoutPanel1.Controls.Add(tarefaControl);
            }
        }

        private void TarefaControl_CheckBoxSelected(object sender, Projeto_Tarefas e)
        {
            if (sender is TarefaControl control && control.Tag is Projeto_Tarefas tarefas)
            {
                var tarefaSelecionada = ctx.Projeto_Tarefas.FirstOrDefault(p => p.Codigo == tarefas.Codigo);
                if (tarefaSelecionada != null)
                {
                    tarefaSelecionada.isConcluida = !tarefaSelecionada.isConcluida;
                    ctx.SaveChanges();
                    carregarTarefas();
                    homeForm.LoadProjetos();
                }
            }
        }
    }
}
