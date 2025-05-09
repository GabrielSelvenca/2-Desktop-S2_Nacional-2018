using GabrielForm.Components;
using GabrielForm.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GabrielForm
{
    public partial class CriarProjeto : parent
    {
        public CriarProjeto()
        {
            InitializeComponent();
        }

        List<Usuario> listaParticipantes = new List<Usuario>();

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void CriarProjeto_Load(object sender, System.EventArgs e)
        {
            LoadListaMembros(listaParticipantes);
        }

        // carrega lista de participantes para busca
        private void LoadListaBusca(List<Usuario> listaParticipantes)
        {
            flowLayoutPanel2.Controls.Clear();

            var usuario = ctx.Usuario.FirstOrDefault(x => x.Usuario1 == UserData.user.Usuario1);

            flowLayoutPanel2.Controls.Add(new UserControl1(usuario));

            foreach (var item in listaParticipantes)
            {
                UserControl1 userControl1 = new UserControl1(item);

                userControl1.BotaoClidado += BtnRemover;

                flowLayoutPanel2.Controls.Add(new UserControl1(item));
            }
        }

        // carrega a lista de membros no projeto.
        private void LoadListaMembros(List<Usuario> listaParticipantes)
        {
            flowLayoutPanel1.Controls.Clear();

            var usuario = ctx.Usuario.FirstOrDefault(x => x.Usuario1 == UserData.user.Usuario1);

            flowLayoutPanel1.Controls.Add(new UserControl1(usuario));

            foreach (var item in listaParticipantes)
            {
                UserControl1 userControl1 = new UserControl1(item);

                userControl1.BotaoClidado += BtnRemover;

                flowLayoutPanel1.Controls.Add(new UserControl1(item));
            }
        }

        private void BtnRemover(object sender, Usuario user)
        {
            DialogResult dialog = "Deseja mesmo remover o usuário?".Question();

            if (dialog == DialogResult.Yes)
            {
                listaParticipantes.Remove(user);
                LoadListaBusca(listaParticipantes);
            }
        }

        private void label3_Click(object sender, System.EventArgs e)
        {
            flowLayoutPanel1.Visible = !flowLayoutPanel1.Visible;
            panel2.Visible = !panel2.Visible;
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            flowLayoutPanel2.Visible = true;
            string userSearch = textBox1.Text;
            listaParticipantes = ctx.Usuario.Where(u=> u.Nome.Contains(userSearch) || u.Email.Contains(userSearch)).ToList();
            LoadListaBusca(listaParticipantes);
        }

        private void textBox1_Leave(object sender, System.EventArgs e)
        {
            flowLayoutPanel2.Visible = false;
            flowLayoutPanel2.Controls.Clear();
        }
    }
}
