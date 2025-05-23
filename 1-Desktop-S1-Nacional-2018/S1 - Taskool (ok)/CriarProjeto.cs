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

        private void CriarProjeto_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;

            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

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

        private void label3_Click(object sender, EventArgs e)
        {
            panel3.Visible = !panel3.Visible;
            label3.ForeColor = Color.Black;
            label2.ForeColor = Color.Gray;
        }

        private void label2_Click(object sender, EventArgs e)
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

        private void AtualizarSugestoesComboBox()
        {
            var texto = comboBox1.Text?.Trim();
            if (string.IsNullOrWhiteSpace(texto)) return;

            var codigosSelecionados = listaSelecionados.Select(u => u.Codigo).ToList();

            var usuariosEncontrados = ctx.Usuario
                .Where(u => (u.Nome.Contains(texto) || u.Email.Contains(texto)) && !codigosSelecionados.Contains(u.Codigo))
                .Take(20)
                .ToList();

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();

            foreach (var usuario in usuariosEncontrados)
            {
                comboBox1.Items.Add($"{usuario.Nome} {usuario.Email}");
            }

            comboBox1.EndUpdate();

            comboBox1.DroppedDown = true;

            comboBox1.SelectionStart = comboBox1.Text.Length;
            comboBox1.SelectionLength = 0;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChange || string.IsNullOrEmpty(comboBox1.Text)) return;
            AtualizarSugestoesComboBox();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is Usuario usuarioSelect)
            {
                if (!listaSelecionados.Any(u => u.Codigo == usuarioSelect.Codigo))
                {
                    listaSelecionados.Add(usuarioSelect);

                    var userControl = new UserControl1(usuarioSelect, UserControl1.TipoControl.Usuario);
                    userControl.BotaoClicado += UserControl_BotaoClicado;

                    listaControl.Add(userControl);
                    LoadLista();
                }

                ignoreTextChange = true;
                comboBox1.SelectedIndex = -1;
                comboBox1.Text = "";
                ignoreTextChange = false;
                comboBox1.Focus();
            }
        }

        private void UserControl_BotaoClicado(object sender, Usuario e)
        {
            var control = sender as UserControl1;
            if (control == null) return;

            listaControl.Remove(control);
            listaSelecionados.RemoveAll(u => u.Codigo == e.Codigo);

            LoadLista();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var nomeProjeto = textBox1.Text.Trim();
            if (nomeProjeto.Replace(" ", "").Length < 3)
            {
                SystemSounds.Beep.Play();
                "Nome do projeto não pode conter espaços em branco.".Alert();
                return;
            }

            var usuarioAtual = ctx.Usuario.FirstOrDefault(u => u.Nome == UserData.user.Nome);

            var projeto = new Projeto
            {
                Nome = nomeProjeto,
                CodUsuario = usuarioAtual.Codigo,
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

            "Projeto criado!".Information();
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Text = checkBox1.Checked ? "Desativado" : "Ativado";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var nomeProjeto = textBox1.Text;
            button1.Enabled = nomeProjeto.Length > 2;
            button1.BackColor = button1.Enabled ? SystemColors.Control : SystemColors.ControlDark;
            button1.UseVisualStyleBackColor = true;
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