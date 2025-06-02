using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabrielForm
{
    public static class FavoritoService
    {
        public static void mudarFavorito(PictureBox pb, int codigo, dbTarefasEntities ctx, HomeForm homeForm)
        {
            if (pb.Tag.ToString() == "Off")
            {
                pb.Image = Properties.Resources.estrela_on;
                pb.Tag = "On";
                ctx.Database
                    .ExecuteSqlCommand("INSERT INTO Items_Favoritos (CodUsuario, CodTarefa) VALUES (@p0, @p1)",
                    UserData.user.Codigo,
                    codigo
                    );
            }
            else if (pb.Tag.ToString() == "On")
            {
                pb.Image = Properties.Resources.estrela_off;
                pb.Tag = "Off";

                int codUsuario = UserData.user.Codigo;
                int codTarefa = codigo;

                int linhasAfetadas = ctx.Database.ExecuteSqlCommand(
                    "DELETE FROM Items_Favoritos WHERE CodUsuario = @p0 AND CodTarefa = @p1",
                    UserData.user.Codigo,
                    codigo
                );
            }
            homeForm.LoadProjetos();
            homeForm.AtualizarFavoritosAbertos();
        }
    }
}
