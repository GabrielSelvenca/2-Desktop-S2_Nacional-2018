using System;
using System.Drawing;
using System.Windows.Forms;

namespace GabrielForm
{
    public partial class ConfigColors : parent
    {
        public ConfigColors()
        {
            InitializeComponent();
            this.Text = "Select Color | Taskool";
            maskedTextBox1.Mask = "rgb(000, 000, 000)";
            maskedTextBox1.TextChanged += MaskedTextBox1_TextChanged;
        }

        private void MaskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox1.MaskCompleted)
            {
                string text = maskedTextBox1.Text.Replace("rgb(", "").Replace(")", "");
                string[] partsMascara = text.Split('.');


                cor = Color.FromArgb(int.Parse(partsMascara[0]), int.Parse(partsMascara[1]), int.Parse(partsMascara[2]));

                panel1.BackColor = cor;

                textBox1.Text = $"#{cor.R:X2}{cor.G:X2}{cor.B:X2}";


            }
        }

        public ConfigColors(HomeForm homeForm)
        {
            InitializeComponent();
            this.Text = "Select Color | Taskool";
            HomeForm = homeForm;
            maskedTextBox1.Mask = "rgb(000, 000, 000)";
            maskedTextBox1.TextChanged += MaskedTextBox1_TextChanged;
        }

        Color cor = new Color();

        public HomeForm HomeForm { get; }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog dl = new ColorDialog();

            if (dl.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = dl.Color;
                cor = dl.Color;
                textBox1.Text = $"#{cor.R:X2}{cor.G:X2}{cor.B:X2}";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserData.user.Senha = $"#{cor.R:X2}{cor.G:X2}{cor.B:X2}";


            var user = ctx.Usuario.Find(UserData.user.Codigo);
            user.Senha = $"#{cor.R:X2}{cor.G:X2}{cor.B:X2}";


            ctx.Entry(user).CurrentValues.SetValues(user);
            ctx.SaveChanges();

            HomeForm.Close();

            Close();
            new HomeForm().Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var newCor = textBox1.Text.Replace("#", "");
            if (string.IsNullOrEmpty(newCor))
                return;

            cor = ColorTranslator.FromHtml($"#{newCor}");

            maskedTextBox1.Text = $"{cor.R}{cor.G}{cor.B}";
            panel1.BackColor = cor;

        }
    }
}
