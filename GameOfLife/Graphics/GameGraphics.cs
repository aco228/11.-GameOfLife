using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GameOfLife.Mechanics;

namespace GameOfLife.Graphics
{
  public class GameGraphics
  {

    private System.Drawing.Graphics finalGraphics = null;
    private Bitmap bitmap = null;
    private System.Drawing.Graphics graphics = null;
    private Pen pen = null;
    private Logic logic = null;
    private GameForm form = null;
    private Color _defaultColor = Color.FromArgb(255, 0, 0, 0);
    private int _width;
    private int _height;
    

    public GameGraphics(GameForm form, Logic logic)
    {
      this.logic = logic;
      this._width = logic.Width;
      this._height = logic.Height;
      this.form = form;

      this.bitmap = new Bitmap(this._width, this._height);
      this.graphics = System.Drawing.Graphics.FromImage(this.bitmap);
      this.finalGraphics = form.CreateGraphics();
      this.pen = new Pen(Color.FromArgb(65, 255, 255, 255));
      this.graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    }
    
    public Point X = new Point();
    public Point Y = new Point();
    public Bitmap Bitmap { get { return this.bitmap; } }
    public System.Drawing.Graphics Graphics { get { return this.graphics; } }
    public System.Drawing.Graphics G { get { return this.graphics; } }
    public Pen Pen { get { return this.pen; } }
    public int Width { get { return this._width; } }
    public int Height { get { return this._height; } }

    public void SetDefaultColor(byte a, byte r, byte g, byte b)
    {
      this._defaultColor = Color.FromArgb(a, r, g, b);
    }

    public virtual void BeforeDraw()
    {
      if (this.logic.Width != this._width || this.logic.Height != this._height)
        this.Update();

      this.graphics.Clear(this._defaultColor);
      this.ColorModificationOnUpdate();
    }

    public virtual void Drawing()
    {

    }

    public virtual void AfterDraw()
    {
      this.finalGraphics.DrawImage(this.bitmap, 0, 0);
    }

    public virtual void Draw()
    {
      this.BeforeDraw();
      this.Drawing();
      this.AfterDraw();
    }

    private void Update()
    {
      this._width = this.logic.Width;
      this._height = this.logic.Height;

      this.bitmap.Dispose();
      this.graphics.Dispose();
      this.finalGraphics.Dispose();
      this.bitmap = new Bitmap(this._width, this._height);
      this.graphics = System.Drawing.Graphics.FromImage(this.bitmap);
      this.finalGraphics = this.form.CreateGraphics();
    }

    private int ta = 255;
    private int tr = 255; int tru = 1;
    private int tg = 255; int tgu = 2;
    private int tb = 255; int tbu = 4;


    private void ColorModificationOnUpdate()
    {
      if (this.logic.IsPaused)
        return;

      this._defaultColor = Color.FromArgb(this.ta, this.tr, this.tg, this.tb);

      this.tr += this.tru;
      if (tr >= 180 || tr <= 0)
      {
        this.tr = (tr > 100) ? 180 : 0;
        this.tru *= -1;
      }

      this.tb += this.tbu;
      if (tb >= 180 || tb <= 0)
      {
        this.tb = (tb > 100) ? 180 : 0;
        this.tbu *= -1;
      }

      this.tg += this.tgu;
      if (tg >= 180 || tg <= 0)
      {
        this.tg = (tg > 100) ? 180 : 0;
        this.tgu *= -1;
      }

    }

  }
}
