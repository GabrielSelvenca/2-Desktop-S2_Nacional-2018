using GabrielForm.Models;
using System;
using System.Windows.Forms;

namespace GabrielForm.Components
{
    public partial class TarefaControl : UserControl
    {
        public event EventHandler<Projeto_Tarefas> CheckBoxSelected;
        Projeto_Tarefas projeto_Tarefas;

        dbTarefasEntities ctx = new dbTarefasEntities();

        public TarefaControl(Projeto_Tarefas proj_tarefa)
        {
            InitializeComponent();

            projeto_Tarefas = proj_tarefa;
            this.Tag = projeto_Tarefas;

            checkBox1.Text = proj_tarefa.Descricao;
            checkBox1.Checked = (bool)projeto_Tarefas.isConcluida;

            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxSelected?.Invoke(this, projeto_Tarefas);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Tag.ToString() == "Off")
            {
                pictureBox1.Image = Properties.Resources.estrela_on;
                pictureBox1.Tag = "On";
                ctx.Database
                    .ExecuteSqlCommand("INSERT INTO Items_Favoritos (CodUsuario, CodTarefa) VALUES (@p0, @p1)",
                    UserData.user.Codigo,
                    projeto_Tarefas.Codigo
                    );
            }
            else if (pictureBox1.Tag.ToString() == "On")
            {
                pictureBox1.Image = Properties.Resources.estrela_off;
                pictureBox1.Tag = "Off";
                ctx.Database
                    .ExecuteSqlCommand("DELETE FROM Items_Favoritos WHERE CodUsuario = @p0 AND CodTarefa = @p1",
                    UserData.user.Codigo,
                    projeto_Tarefas.Codigo
                    );
            }
        }
    }
}
