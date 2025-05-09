using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GabrielForm
{
    public class CircleBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pea)
        {
            base.OnPaint(pea);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Height - 1, this.Width - 1);

            Region = new System.Drawing.Region(path);
        }
    }
}
