using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

using GameOfLife.Mechanics;

namespace GameOfLife.Entities
{
  public class Grid : Entity
  {
    private Bitmap _bitmap = null;
    private System.Drawing.Graphics _graphics = null;
    private Pen tabPen = new Pen(Color.FromArgb(50, 255, 255, 255));
    private Pen linePen = new Pen(Color.FromArgb(35, 255, 255, 255));
    private bool _hasGraphics = false;
    private int _tempWidth = -1, _tempHeight = -1;
    private int _tempRepoX = -1, _tempRepoY = -1;
    private int _tempZoom = -1;
    private int _tabSize = 5;

    public Grid(GameOfLife.Mechanics.Entities parent, EntityNames name = EntityNames.Grid) 
      : base(parent, name) 
    {
      this._type = EntityType.Grid;
    }

    #region Override

    public override void OnStart()
    {
      if(!this._hasGraphics)
        this.DrawTemporaryGraphics();

      if (this._tempRepoX != this.Logic.RepositionX || this._tempRepoY != this.Logic.RepositionY)
        this.DrawTemporaryGraphics();

      if(this._tempWidth != this.Logic.Width || this._tempHeight != this.Logic.Height)
        this.DrawTemporaryGraphics();

      if (this._tempZoom != this.Logic.Zoom)
        this.DrawTemporaryGraphics();

    }

    public override void Draw()
    {
      if (!this._isVisible) return;
      if (!this._hasGraphics) return;

      this.Graphics.G.DrawImage(this._bitmap, 0, 0);
      base.Draw();
    }

    public override void OnEnd()
    {
      //base.OnEnd();
    }

    #endregion

    #region Lines specific

    private void DrawTemporaryGraphics()
    {
      this._hasGraphics = true;

      this._tempWidth = this.Logic.Width;
      this._tempHeight = this.Logic.Height;
      this._tempZoom = this.Logic.Zoom;
      this._tempRepoX = this.Logic.RepositionX;
      this._tempRepoY = this.Logic.RepositionY;

      if (this._bitmap != null) this._bitmap.Dispose();
      this._bitmap = new Bitmap(this._tempWidth, this._tempHeight);

      if (this._graphics != null) this._graphics.Dispose();
      this._graphics = System.Drawing.Graphics.FromImage(this._bitmap);

      this.DrawVerticalLines();
      this.DrawHorizontalLines();
    }

    private void DrawVerticalLines()
    {
      int itab = this._tempRepoX % this._tabSize;
      this.X.Y = 0; this.Y.Y = this._tempWidth;

      for(int i = 0; i < this._tempWidth; i+= this.Logic.Zoom)
      {
        this.X.X = i; this.Y.X = i;
        itab = DrawLine(itab);
      }
    }

    private void DrawHorizontalLines()
    {
      int itab = this._tempRepoY % this._tabSize;
      this.X.X = 0; this.Y.X = this._tempWidth;

      for (int i = 0; i < this._tempWidth; i+= this.Logic.Zoom)
      {
        this.X.Y = i; this.Y.Y = i;
        itab = DrawLine(itab);
      }
    }

    private int DrawLine(int itab)
    {
      if(itab == this._tabSize - 1)
      {
        this._graphics.DrawLine(this.tabPen, this.X, this.Y);
        return 0;
      }

      this._graphics.DrawLine(this.linePen, this.X, this.Y);
      return ++itab;
    }

    public void SetLineColor(byte a, byte r, byte g, byte b)
    {
      this.linePen.Color = Color.FromArgb(a, r, g, b);
    }

    public void SetTabColor(byte a, byte r, byte g, byte b)
    {
      this.tabPen.Color = Color.FromArgb(a, r, g, b);
    }

    #endregion
  }
}
