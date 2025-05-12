using GabrielForm.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GabrielForm
{
    public partial class CriarProjeto : parent
    {
        List<Usuario> listaSelecionados = new List<Usuario>();
        List<UserControl1> listaControl = new List<UserControl1>();
        private Timer filterTimer = new Timer();
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
            flowLayoutPanel1.Visible = false;
            panel2.Visible = true;
        }

        private void label2_Click(object sender, System.EventArgs e)
        {
            flowLayoutPanel1.Visible = true;
            panel2.Visible = false;
        }

        private void LoadLista()
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (var user in listaControl)
            {
                user.Width = flowLayoutPanel1.ClientSize.Width - flowLayoutPanel1.Margin.Horizontal;

                flowLayoutPanel1.Controls.Add(user);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text)) return;

            string nomeUsuario = comboBox1.Text.Split('(')[0].Trim();

            var usuarioSelect = ctx.Usuario.FirstOrDefault(u=> u.Nome == nomeUsuario);

            if (usuarioSelect != null)
            {
                if (!listaSelecionados.Any(lista => lista.Nome == nomeUsuario))
                {
                    listaSelecionados.Add(usuarioSelect);

                    var userControl = new UserControl1(
                        usuarioSelect,
                        UserControl1.TipoControl.Usuario
                    );

                    listaControl.Add(userControl);

                    LoadLista();
                }
            }

            ignoreTextChange = true;
            comboBox1.Text = "";
            ignoreTextChange = false;
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
            foreach(var user in listaSelecionados)
            {
                var membro = new Projeto_Tarefas
                {
                    
                }
            }
        }
    }
}