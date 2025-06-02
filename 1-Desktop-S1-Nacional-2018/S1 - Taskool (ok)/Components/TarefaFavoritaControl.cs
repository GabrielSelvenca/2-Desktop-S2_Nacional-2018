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
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            pictureBox1.Tag = "On";
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Parent is Control parent)
            {
                while (parent != null && !(parent is ProjetoControl))
                    parent = parent.Parent;

                if (parent is ProjetoControl projetoControl)
                {
                    var tarefa = ctx.Projeto_Tarefas.FirstOrDefault(t => t.Codigo == favorito.CodTarefa);
                    projetoControl.AttConclusao(tarefa);
                }
            }
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