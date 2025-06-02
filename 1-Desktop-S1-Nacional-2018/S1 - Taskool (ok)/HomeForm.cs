using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Windows.Forms;
using System.Linq;
using WMPLib;
using GabrielForm.Components;
using GabrielForm.Models;
using System.Drawing;

namespace GabrielForm
{
    public partial class HomeForm : parent
    {
        WindowsMediaPlayer media = new WindowsMediaPlayer();


        List<string> musicas = new List<string>();
        int posicao = 0;

        List<Control> menuPanelOriginalControls = new List<Control>();

        public HomeForm()
        {
            InitializeComponent();
            this.Text = "Menu Principal | Taskool";
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            panel3.Visible = !panel3.Visible;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            var pergunta = "Deseja sair mesmo?".Question();
            if (pergunta == DialogResult.Yes)
            {
                UserData.user = null;
                Close();
                new Form1().Show();
            }
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString("HH:mm");
            musicas = Directory.GetFiles("musicas", "*").ToList();

            musicas.Sort();

            label8.Text = Path.GetFileName(musicas[posicao]);

            media.URL = musicas[posicao];
            media.PlayStateChange += Media_PlayStateChange;
            media.controls.stop();

            LoadMensagem();
            LoadJson();
            LoadProjetos();
        }

        private void Media_PlayStateChange(int NewState)
        {
            if (NewState == 8)
            {
                if (posicao == musicas.Count)
                    posicao = 0;

                media.URL = musicas[posicao];
                label8.Text = Path.GetFileName(musicas[posicao]);
            }
        }

        public class Mensagem
        {
            public string mensagem { get; set; }
            public string autor { get; set; }
        }

        Random random = new Random();
        private void LoadJson()
        {
            var caminhoJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mensagens.json");
            var conteudo = File.ReadAllText(caminhoJson);
            var json = JsonConvert.DeserializeObject<List<Mensagem>>(conteudo);
            int indicelist = random.Next(json.Count);

            label6.Text = json[indicelist].mensagem;
            label10.Text = json[indicelist].autor;

        }

        public void LoadProjetos()
        {
            var codUsuario = UserData.user.Codigo;

            var projetosId = ctx.Database.SqlQuery<int>("SELECT CodProjeto FROM Projeto_Membros WHERE CodMembro = @p0", codUsuario).ToList();
            var favoritos = ctx.Database.SqlQuery<int>("SELECT CodTarefa FROM Items_Favoritos WHERE CodUsuario = @p0", codUsuario).ToList();

            if (projetosId.Count < 1) return;

            flowLayoutPanel1.Controls.Clear();

            if (favoritos.Count > 0)
            {
                var favoritosControl = new FavoritosControl(favoritos.Count);
                flowLayoutPanel1.Controls.Add(favoritosControl);
                favoritosControl.Click += FavoritosControl_Click;
            }

            var projetos = ctx.Projeto.Where(p => projetosId.Contains(p.Codigo)).OrderBy(p => p.Codigo).ToList();

            foreach (var projeto in projetos)
            {
                var projetoControl = new ProjetoListaControl(projeto);
                flowLayoutPanel1.Controls.Add(projetoControl);
                projetoControl.Click += ProjetoControl_Click;
            }
        }

        private void FavoritosControl_Click(object sender, EventArgs e)
        {
            if (sender is FavoritosControl control)
            {
                ResetarCores();

                if (menuPanelOriginalControls.Count == 0)
                    menuPanelOriginalControls.AddRange(contentPanel.Controls.Cast<Control>().ToList());


                contentPanel.Controls.Clear();


                var favoritosDetalhados = ctx.Database.SqlQuery<FavoritoModel>(
                    @"SELECT 
        t.Codigo AS CodTarefa, 
        t.Descricao AS NomeTarefa, 
        p.Nome AS NomeProjeto
      FROM Items_Favoritos f
      INNER JOIN Projeto_Tarefas t ON f.CodTarefa = t.Codigo
      INNER JOIN Projeto p ON t.CodProjeto = p.Codigo
      WHERE f.CodUsuario = @p0",
                    UserData.user.Codigo).ToList();


                var projetoView = new FavoritosContentControl(favoritosDetalhados, ctx, this)
                {
                    Dock = DockStyle.Fill
                };

                contentPanel.Controls.Add(projetoView);
                PutStyle(control, Color.DeepSkyBlue);
            }
        }

        private void ProjetoControl_Click(object sender, EventArgs e)
        {
            if (sender is ProjetoListaControl control && control.Tag is Projeto projeto)
            {
                var projetoSelecionado = ctx.Projeto.FirstOrDefault(p => p.Codigo == projeto.Codigo);
                if (projetoSelecionado != null)
                {
                    ResetarCores();

                    if (menuPanelOriginalControls.Count == 0)
                        menuPanelOriginalControls.AddRange(contentPanel.Controls.Cast<Control>().ToList());


                    contentPanel.Controls.Clear();

                    var projetoView = new ProjetoControl(projetoSelecionado, this)
                    {
                        Dock = DockStyle.Fill
                    };

                    contentPanel.Controls.Add(projetoView);
                    PutStyle(control, Color.DeepSkyBlue);
                }
            }
        }

        private void ResetarCores()
        {
            foreach(Control control in flowLayoutPanel1.Controls)
            {
                if (control is ProjetoListaControl || control is FavoritosControl)
                {
                    PutStyle(control, Color.Black);
                }
            }
        }

        public void AtualizarFavoritosAbertos()
        {
            if (contentPanel.Controls.Count > 0 &&
                contentPanel.Controls[0] is FavoritosContentControl)
            {
                FavoritosControl_Click(flowLayoutPanel1.Controls
                    .OfType<FavoritosControl>()
                    .FirstOrDefault(), EventArgs.Empty);
            }
        }

        private void PutStyle(Control control, Color cor)
        {
            foreach(Control c in control.Controls)
            {
                if (c.HasChildren)
                    PutStyle(c, cor);

                if (c is Splitter splitter)
                    splitter.BackColor = cor;
                else if (c is Label lb)
                    lb.ForeColor = cor;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            idioma = "PT";
            LoadMensagem();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            idioma = "EN";
            LoadMensagem();
        }


        string idioma = "PT";
        private void LoadMensagem()
        {
            string mensagem = "";
            if (idioma == "PT")
            {
                if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
                    mensagem = $"Boa Tarde, {UserData.user.Nome}";
                else if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour <= 23)
                    mensagem = $"Boa noite, {UserData.user.Nome}";
                else if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 4)
                    mensagem = $"Boa madrugada, {UserData.user.Nome}";
                else if (DateTime.Now.Hour >= 4 && DateTime.Now.Hour < 12)
                    mensagem = $"Bom dia, {UserData.user.Nome}";

            }
            else if (idioma == "EN")
            {
                if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
                    mensagem = $"Good afternoon, {UserData.user.Nome}";
                else if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour <= 23)
                    mensagem = $"Good evening, {UserData.user.Nome}";
                else if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 4)
                    mensagem = $"Good sun-up, {UserData.user.Nome}";
                else if (DateTime.Now.Hour >= 4 && DateTime.Now.Hour < 12)
                    mensagem = $"Good morning, {UserData.user.Nome}";
            }

            label5.Text = mensagem;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString("HH:mm");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "Play")
            {
                button5.Text = "Pause";
                media.controls.play();
            }
            else
            {
                button5.Text = "Play";
                media.controls.pause();
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            new ConfigColors(this).Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Hide();
            new CriarProjeto().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        { 
            Hide();
            new CriarProjeto().Show();
        }

        public void CarrregarMenu()
        {
            contentPanel.Controls.Clear();

            foreach (var control in menuPanelOriginalControls)
            {
                contentPanel.Controls.Add(control);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            CarrregarMenu();
        }
    }
}
