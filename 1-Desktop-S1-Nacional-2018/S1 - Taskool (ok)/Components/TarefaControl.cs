using GabrielForm.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace GabrielForm.Components
{
    public partial class TarefaControl : UserControl
    {
        public event EventHandler<Projeto_Tarefas> CheckBoxSelected;
        Projeto_Tarefas projeto_Tarefas;

        dbTarefasEntities ctx = new dbTarefasEntities();

        HomeForm homeForm;

        ProjetoControl projeto_ctrl;

        public TarefaControl(Projeto_Tarefas proj_tarefa, HomeForm hF, ProjetoControl proj_ctrl)
        {
            InitializeComponent();

            this.homeForm = hF;
            this.projeto_ctrl = proj_ctrl;

            projeto_Tarefas = proj_tarefa;
            this.Tag = projeto_Tarefas;

            checkBox1.Text = proj_tarefa.Descricao;
            checkBox1.Checked = (bool)projeto_Tarefas.isConcluida;

            bool ehFavorita = VerificarFavorito();

            pictureBox1.Image = ehFavorita ? Properties.Resources.estrela_on : Properties.Resources.estrela_off;
            pictureBox1.Tag = ehFavorita ? "On" : "Off";

            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
        }

        private bool VerificarFavorito()
        {
            var codUsuario = UserData.user.Codigo;

            int favorito = ctx.Database.SqlQuery<int>("SELECT COUNT(*) FROM Items_Favoritos WHERE CodTarefa = @p0 AND CodUsuario = @p1", projeto_Tarefas.Codigo, codUsuario).FirstOrDefault();

            return favorito > 0;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxSelected?.Invoke(this, projeto_Tarefas);
            projeto_ctrl.AtualizarProgressoCircular();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FavoritoService.mudarFavorito(pictureBox1, projeto_Tarefas.Codigo, ctx, homeForm);
        }
    }
}
