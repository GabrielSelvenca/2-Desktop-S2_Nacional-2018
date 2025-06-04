using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabrielForm.Components
{
    public partial class TarefaFavoritaControl : UserControl
    {
        private FavoritoModel favorito;
        private Projeto_Tarefas tarefaAtual;
        HomeForm hf;
        dbTarefasEntities ctx;

        public TarefaFavoritaControl(FavoritoModel favorito, dbTarefasEntities ctx, HomeForm homeForm)
        {
            InitializeComponent();

            this.hf = homeForm;
            this.favorito = favorito;
            this.ctx = ctx;

            label1.Text = favorito.NomeProjeto;
            checkBox1.Text = favorito.NomeTarefa;
            tarefaAtual = ctx.Projeto_Tarefas.FirstOrDefault(p => p.Codigo == favorito.CodTarefa);
            checkBox1.Checked = (bool)tarefaAtual.isConcluida;
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            pictureBox1.Tag = "On";
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            var tarefaAtual = ctx.Projeto_Tarefas.FirstOrDefault(p => p.Codigo == favorito.CodTarefa);
            tarefaAtual.isConcluida = checkBox1.Checked;
            ctx.SaveChanges();
            hf.LoadProjetos();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FavoritoService.mudarFavorito(pictureBox1, favorito.CodTarefa, ctx, hf);
            var qtd = ctx.Database.SqlQuery<int>("SELECT CodTarefa FROM Items_Favoritos WHERE CodUsuario = @p0", UserData.user.Codigo).ToList();

            if (qtd.Count() <= 0)
            {
                hf.CarrregarMenu();
            }
        }
    }
}