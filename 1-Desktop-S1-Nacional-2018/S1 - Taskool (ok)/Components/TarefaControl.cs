using GabrielForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            checkBox1.Text = proj_tarefa.Descricao;

            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxSelected?.Invoke(this, projeto_Tarefas);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox1.Image == Properties.Resources.estrela_off1 ? Properties.Resources.estrela_on1 : Properties.Resources.estrela_off1;

            if (pictureBox1.Image == Properties.Resources.estrela_on1)
            {
                ctx.Database
                    .ExecuteSqlCommand("INSERT INTO Items_Favoritos (CodUsuario, CodTarefa) VALUES (@p0, @p1)",
                    UserData.user.Codigo,
                    projeto_Tarefas.Codigo
                    );
            }
            else
            {
                ctx.Database
                    .ExecuteSqlCommand("DELETE FROM Items_Favoritos WHERE CodUsuario = @p0 AND CodTarefa = @p1",
                    UserData.user.Codigo,
                    projeto_Tarefas.Codigo
                    );
            }
        }
    }
}
