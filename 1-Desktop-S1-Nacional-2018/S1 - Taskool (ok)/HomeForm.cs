using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Windows.Forms;
using System.Linq;
using WMPLib;

namespace GabrielForm
{
    public partial class HomeForm : parent
    {
        WindowsMediaPlayer media = new WindowsMediaPlayer();
        public HomeForm()
        {
            InitializeComponent();
            this.Text = "Home Page | Taskool";
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



        List<string> musicas = new List<string>();
        int posicao = 0;

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
            ;

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
    }
}
