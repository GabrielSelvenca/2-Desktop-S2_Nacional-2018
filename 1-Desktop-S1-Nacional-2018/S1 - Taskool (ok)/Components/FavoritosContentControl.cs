using GabrielForm.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GabrielForm.Components
{
    public partial class FavoritosContentControl : UserControl
    {
        private List<FavoritoModel> favoritos;
        dbTarefasEntities ctx;
        HomeForm homeForm;

        public FavoritosContentControl(List<FavoritoModel> favoritos, dbTarefasEntities contex, HomeForm hf)
        {
            InitializeComponent();

            this.favoritos = favoritos;
            this.ctx = contex;
            this.homeForm = hf;

            label1.Text = "Favoritos";
            label1.ForeColor = Color.LightBlue;

            CarregarFavoritos();
        }

        private void CarregarFavoritos()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (var favorito in favoritos)
            {
                var favControl = new TarefaFavoritaControl(favorito, ctx, homeForm);
                flowLayoutPanel1.Controls.Add(favControl);
            }
        }
    }
}
