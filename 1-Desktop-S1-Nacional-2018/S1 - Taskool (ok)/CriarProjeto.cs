using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace GabrielForm
{
    public partial class CriarProjeto : parent
    {
        List<Usuario> listaSelecionados = new List<Usuario>();
        List<UserControl1> listaControl = new List<UserControl1>();
        private bool ignoreTextChange = false;

        public CriarProjeto()
        {
            InitializeComponent();
            this.Text = "Criar Projeto | Taskool";
            this.FormClosing += CriarProjeto_FormClosing;
        }

        private void CriarProjeto_Load(object sender, System.EventArgs e)
        {
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;

            var usuarioAtual = ctx.Usuario.FirstOrDefault(u => u.Nome == UserData.user.Nome);
            listaSelecionados.Add(usuarioAtual);

            var userControl = new UserControl1(
                usuarioAtual,
                UserControl1.TipoControl.Owner
                );

            listaControl.Add(userControl);

            LoadLista();
        }

        private void CriarProjeto_FormClosing(object sender, FormClosingEventArgs e)
        {
            new HomeForm().Show();
        }

        private void label3_Click(object sender, System.EventArgs e)
        {
            panel3.Visible = !panel3.Visible;
            label3.ForeColor = Color.Black;
            label2.ForeColor = Color.Gray;
        }

        private void label2_Click(object sender, System.EventArgs e)
        {
            panel3.Visible = !panel3.Visible;
            label2.ForeColor = Color.Black;
            label3.ForeColor = Color.Gray;
        }

        private void LoadLista()
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (var user in listaControl)
            {
                user.Width = flowLayoutPanel1.ClientSize.Width - flowLayoutPanel1.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth;

                flowLayoutPanel1.Controls.Add(user);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text)) return;

            string nomeUsuario = comboBox1.Text.Split('(')[0].Trim();

            var usuarioSelect = ctx.Usuario.FirstOrDefault(u => u.Nome == nomeUsuario);

            if (usuarioSelect == null) return;

            if (!listaSelecionados.Any(lista => lista.Nome == nomeUsuario))
            {
                listaSelecionados.Add(usuarioSelect);

                var userControl = new UserControl1(
                    usuarioSelect,
                    UserControl1.TipoControl.Usuario
                );

                userControl.BotaoClicado += UserControl_BotaoClicado;

                listaControl.Add(userControl);

                LoadLista();
            }

            ignoreTextChange = true;
            comboBox1.Text = "";
            ignoreTextChange = false;
            comboBox1.Focus();
        }

        private void UserControl_BotaoClicado(object sender, Usuario e)
        {
            var control = sender as UserControl1;
            if (control == null) return;

            listaControl.Remove(control);
            listaSelecionados.RemoveAll(u => u.Codigo == e.Codigo);

            LoadLista();
        }

        private void comboBox1_TextChanged(object sender, System.EventArgs e)
        {
            if (ignoreTextChange || string.IsNullOrEmpty(comboBox1.Text)) return;

            comboBox1.Items.Clear();
            comboBox1.SelectionStart = comboBox1.Text.Length;

            var codigosSelecionados = listaSelecionados.Select(u => u.Codigo).ToList();

            var usuariosEncontrados = ctx.Usuario.Where(u => (u.Nome.Contains(comboBox1.Text) || u.Email.Contains(comboBox1.Text)) && !codigosSelecionados.Contains(u.Codigo))
                .Take(20)
                .ToList();

            comboBox1.Items.AddRange(
                usuariosEncontrados.Select(u => u.Nome).ToArray()
            );

            comboBox1.DroppedDown = true;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var nomeProjeto = textBox1.Text.Trim();
            if (nomeProjeto.Replace(" ", "").Length < 3)
            {
                SystemSounds.Beep.Play();
                "Nome do projeto não pode conter espaços em branco.".Alert();
                return;
            }

            var usuario_atual = ctx.Usuario.FirstOrDefault(u => u.Nome == UserData.user.Nome);

            var projeto = new Projeto
            {
                Nome = nomeProjeto,
                CodUsuario = usuario_atual.Codigo,
                NaoPertube = checkBox1.Checked
            };

            ctx.Projeto.Add(projeto);
            ctx.SaveChanges();

            foreach (var user in listaSelecionados)
            {

                ctx.Database.ExecuteSqlCommand(
                    "INSERT INTO Projeto_Membros (CodMembro, CodProjeto) VALUES (@p0, @p1)",
                    user.Codigo,
                    projeto.Codigo
                    );
            }

            "Projeto criado!".Alert();
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
                checkBox1.Text = "Ativado";
            else
                checkBox1.Text = "Desativado";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var nomeProjeto = textBox1.Text;
            if (nomeProjeto.Length > 2)
            {
                button1.Enabled = true;
                button1.BackColor = SystemColors.Control;
                button1.UseVisualStyleBackColor = true;
            }
            else
            {
                button1.Enabled = false;
                button1.BackColor = SystemColors.ControlDark;
                button1.UseVisualStyleBackColor = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var question = "Deseja mesmo cancelar a criação?".Question();

            if (question == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}