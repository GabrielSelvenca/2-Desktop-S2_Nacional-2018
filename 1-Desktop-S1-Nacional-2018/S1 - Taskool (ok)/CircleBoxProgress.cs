using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class CircleBoxProgress : Control
{
    public int Percent { get; set; }

    public CircleBoxProgress()
    {
        this.DoubleBuffered = true;
        this.Size = new Size(150, 150);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        GraphicsPath path = new GraphicsPath();
        path.AddEllipse(0, 0, Width -1, Height -1);
        Region = new Region(path);

        int espressura = 10;
        Rectangle area = new Rectangle(espressura, espressura, Width - espressura * 2, Height - espressura * 2);

        using (Pen fundo = new Pen(Color.LightGray, espressura))
            g.DrawArc(fundo, area, -90f, 360);

        using (Pen progresso = new Pen(Color.LightSkyBlue, espressura))
            g.DrawArc(progresso, area, -90f, (int)(360 * Percent / 100.0));

        string text = $"{Percent}%";
        using (Font fonte = new Font("Segoe UI", 10, FontStyle.Regular))
        {
            SizeF sz = g.MeasureString(text, fonte);
            g.DrawString(text, fonte, Brushes.Black,
                Width / 2 - sz.Width / 2,
                Height / 2 - sz.Height / 2);
        }
    }
}
