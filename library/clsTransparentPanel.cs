using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace aTRS
{
    public class TransparentPanel : Panel
    {
        //protected Graphics graphics;
        //abstract protected void OnDraw();
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    // Update the private member so we can use it in the OnDraw method
        //    this.graphics = e.Graphics;

        //    // Set the best settings possible (quality-wise)
        //    this.graphics.TextRenderingHint =
        //        System.Drawing.Text.TextRenderingHint.AntiAlias;
        //    this.graphics.InterpolationMode =
        //        System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
        //    this.graphics.PixelOffsetMode =
        //        System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        //    this.graphics.SmoothingMode =
        //        System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //    // Calls the OnDraw subclass method
        //    OnDraw();
        //} 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Black)), this.ClientRectangle);
            pe.Graphics.DrawRectangle(Pens.Transparent,
              pe.ClipRectangle.Left,
              pe.ClipRectangle.Top,
              this.Width - 1,
              this.Height - 1);
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
    }
}
