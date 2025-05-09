using GabrielForm.Models;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GabrielForm.Resources
{
    public partial class Cadastrar : parent
    {
        public Cadastrar()
        {
            InitializeComponent();
            this.Text = "Cadastro  |  Taskoool";
            this.FormClosed += Cadastrar_FormClosed;
            panel6.BackColor = panel2.BackColor = panel3.BackColor = panel4.BackColor = Color.Transparent;
        }

        private void Cadastrar_FormClosed(object sender, FormClosedEventArgs e)
        {
            new Form1().Show();
        }

        private void Cadastrar_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            panel6.BackColor = Color.LightBlue;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Transparent;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            panel2.BackColor = Color.LightBlue;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Transparent;
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            panel3.BackColor = Color.LightBlue;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            panel3.BackColor = Color.Transparent;
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            panel4.BackColor = Color.LightBlue;
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var noAscent = Encoding.GetEncoding("ISO-8859-8").GetBytes(textBox5.Text);
            var apelido = Encoding.UTF8.GetString(noAscent).ToLower();
            string[] parts = apelido.Split(' ');

            if (parts.Count() < 2)
            {
                "É necessário ter ao menos um sobrenome para gerar usuário aleatório.".Alert();
                return;
            }

            string newUsuario = $"{parts[0]}.{parts.Last()}{dateTimePicker1.Value.Year.ToString().Substring(2, 2)}";

            if (SearchUser(newUsuario))
            {
                if (parts.Count() < 3)
                {
                    "Não foi possível gerar usuário aleatório!".Alert();
                    return;
                }

                newUsuario = $"{parts[0]}.{parts[parts.Length - 2]}{dateTimePicker1.Value.Year.ToString().Substring(2, 2)}";
                if (!SearchUser(newUsuario))
                {
                    "Não foi possível gerar usuário aleatório!".Alert();
                    return;
                }
            }

            textBox4.Text = newUsuario;
        }

        private bool ValidEmail(string email) 
        {
            string format = @"^[a-zA-Z0-9._%+-]+@[a-z]+\.[a-z]{2,}$";
            Regex regex = new Regex(format);
            return regex.IsMatch(email);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                "Todos os campos são obrigatórios!".Alert();
                return;
            }
            if (!ValidEmail(textBox2.Text))
            {
                "Email deve seguir padrão da industria.".Alert();
                return;
            }

            var ms = new MemoryStream();
            circleBox1.Image.Save(ms, circleBox1.Image.RawFormat);

            Usuario user = new Usuario
            {
                Nome = textBox5.Text,
                Email = textBox2.Text,
                Telefone = textBox3.Text,
                Usuario1 = textBox4.Text,
                Foto = ms.ToArray()
            };

            ctx.Usuario.Add(user);
            ctx.SaveChanges();

            "Usuário cadastrado com sucesso!".Information();
            Hide();
            new Form1().Show();
        }

        private bool SearchUser(string user)
        {
            var us = ctx.Usuario.FirstOrDefault(x => x.Usuario1 == user);

            return us != null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = " Imagem | *.png;*.jpg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                circleBox1.Image = Image.FromFile(ofd.FileName);
            }
        }
    }
}
