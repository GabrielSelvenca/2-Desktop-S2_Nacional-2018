using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public void AtualizarProgressoCircular()
        {
            var tarefas = ctx.Projeto_Tarefas
                .Where(t => t.CodProjeto == proj.Codigo)
                .ToList();

            int total = tarefas.Count;
            int concluidas = tarefas.Count(t => t.isConcluida == true);
            int percentual = total == 0 ? 0 : (int)((concluidas / (float)total) * 100);

            panel6.Controls.Clear();
            var circle = new CircleBoxProgress
            {
                Dock = DockStyle.Fill,
                Percent = percentual
            };
            panel6.Controls.Add(circle);
        }

        private void ProjetoControl_Load(object sender, EventArgs e)
        {
            linkLabel1.Text = "0 TAREFAS CONCLUIDAS";

            carregarTarefas(false);

            AtualizarProgressoCircular();
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

                var tarefaControl = new TarefaControl(proj_tarefa, homeForm, this);

                carregarTarefas(false);
                textBox1.Text = "Adicionar uma tarefa...";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void carregarTarefas(bool concluidas)
        {
            var tarefasPendentes = ctx.Projeto_Tarefas
                .Where(t => t.CodProjeto == proj.Codigo && t.isConcluida == false)
                .OrderByDescending(t => t.Codigo)
                .ToList();

            var tarefasConluidas = ctx.Projeto_Tarefas
                .Where(t => t.CodProjeto == proj.Codigo && t.isConcluida == true)
                .OrderByDescending(t => t.Codigo)
                .ToList();

            var tarefas = concluidas ? tarefasConluidas : tarefasPendentes;

            if (tarefasConluidas != null)
                linkLabel1.Text = tarefasConluidas.Count + " TAREFAS CONCLUIDAS";

            flowLayoutPanel1.Controls.Clear();

            if (tarefas.Count <= 0)
            {
                return;
            }

            foreach (var tarefa in tarefas)
            {
                var tarefaControl = new TarefaControl(tarefa, homeForm, this);
                tarefaControl.CheckBoxSelected += Tarefas_CheckBoxSelected;
                flowLayoutPanel1.Controls.Add(tarefaControl);
            }
        }

        public void Tarefas_CheckBoxSelected(object sender, Projeto_Tarefas e)
        {
            AttConclusao(e);
        }

        public void AttConclusao(Projeto_Tarefas proj)
        {
            var tarefaAtual = ctx.Projeto_Tarefas.FirstOrDefault(p => p.Codigo == proj.Codigo);

            if (tarefaAtual != null)
            {

                tarefaAtual.isConcluida = !tarefaAtual.isConcluida;
                textBox1.Enabled = true;
                ctx.SaveChanges();
                carregarTarefas(false);
                homeForm.LoadProjetos();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Enabled = false;
            carregarTarefas(true);
        }
    }
}