using GabrielForm.Resources;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Windows.Forms;

namespace GabrielForm
{
    public partial class Form1 : parent
    {
        public Form1()
        {
            InitializeComponent();
            label4.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("osk.exe");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (IsKeyLocked(Keys.CapsLock))
                label4.Visible = true;
            else label4.Visible = false;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            new Cadastrar().Show();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imagem | *.png;*.jpg;";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = System.Drawing.Image.FromFile(ofd.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                "É necessário informar o usuário!".Information();
                return;
            }

            var user = ctx.Usuario.FirstOrDefault(x => x.Usuario1 == textBox1.Text || x.Email == textBox1.Text);

            var listaUsers = ctx.Usuario.ToList();

            if (user == null)
            {
                "Imagem ou usuário não reconhecido".Alert();
                SystemSounds.Beep.Play();

                return;
            }

            var ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);

            bool imageValid = Enumerable.SequenceEqual(user.Foto, ms.ToArray());


            var caminhoPasta = "C:\\USER_LOGS";
            var caminhoArquivo = $"{caminhoPasta}\\{user.Nome.Split(' ').First()}{user.Codigo}.txt";
            if (!imageValid)
            {
                if (!Directory.Exists(caminhoPasta))
                    Directory.CreateDirectory(caminhoPasta);

                var ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[0];
                if (File.Exists(caminhoArquivo))
                {
                    var text = File.ReadAllText(caminhoArquivo);
                    File.WriteAllText(caminhoArquivo, $"{text} \n{DateTime.Now.ToString("dd/MM/yyyy")};{DateTime.Now.ToString("HH:mm")};{user.Usuario1};{ip}");
                }
                else
                {
                    File.WriteAllText(caminhoArquivo, $"Data;Hora;Usuário.IP \n{DateTime.Now.ToString("dd/MM/yyyy")};{DateTime.Now.ToString("HH:mm")};{user.Usuario1};{ip}");
                }
                SystemSounds.Beep.Play();
                "Imagem ou usuário não reconhecido".Alert();
                return;
            }
            UserData.user = user;


            Hide();
            new HomeForm().Show();
        }
    }
}
