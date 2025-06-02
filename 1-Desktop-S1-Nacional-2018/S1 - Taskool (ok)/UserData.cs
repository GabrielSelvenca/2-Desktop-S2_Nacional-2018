using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabrielForm
{
    public static class UserData
    {
        public static Usuario user { get; set; }
        public static Color Hex {  get; set; }
    }

    public class FavoritoModel
    {
        public int CodTarefa { get; set; }
        public string NomeTarefa { get; set; }
        public string NomeProjeto { get; set; }
    }
}
