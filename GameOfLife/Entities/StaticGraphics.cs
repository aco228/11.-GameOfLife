using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GameOfLife.Mechanics;

namespace GameOfLife.Entities
{
  public class StaticGraphics : Entity
  {
    public StaticGraphics(Bitmap image, PlacementType type, GameOfLife.Mechanics.Entities parent, EntityNames name = EntityNames.StaticGraphics, int posx = 0, int posy = 0)
      :base(parent, name)
    {
      this.X.X = posx; this.Y.Y = posy;
      this._image = image;
      this._placement = type;
      this._width = this._image.Width;
      this._height = this._image.Height;
      this._type = EntityType.StaticGraphics;
      this._calculatePlacement = true;
    }

    private Bitmap _image = null;
    private double _scale = 1.0;
    protected bool _isResized = false;

    public double Scale { get { return this._scale; } }
    public override int Width { get { return (int)Math.Ceiling(base.Width / this._scale); } }
    public override int Height { get { return (int)Math.Ceiling(base.Height / this._scale); } }

    public override void OnStart()
    {
      base.OnStart();
    }

    public void SetScale(double scale)
    {
      this._scale = scale;
    }

    public void ResizeWidth(int width)
    {
      this._isResized = true;

      double ow = this._width * 1.0;
      double iw = width * 1.0;

      double factor = (this._height <= width) ? ow / iw : iw / ow;

      this._height = (int)Math.Ceiling(this._height * factor);
      this._width = (int)Math.Ceiling(this._width * factor);
    }

    public void ResizeHeight(int height)
    {
      this._isResized = true;

      double oh = this._height * 1.0;
      double ih = height * 1.0;

      double factor = (this._height <= height) ? oh / ih : ih / oh;

      this._height = (int)Math.Ceiling(this._height * factor);
      this._width = (int)Math.Ceiling(this._width * factor);
    }

    public override void Draw()
    {
      if (!this._isVisible) return;
      base.Draw();

      this.Graphics.G.DrawImage(this._image, X.X, Y.Y, this.Width, this.Height);
    }

    public override void OnEnd()
    {
      base.OnEnd();
    }
  }
}
