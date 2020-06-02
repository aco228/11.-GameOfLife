using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Graphics;
using System.Drawing;
using GameOfLife.Entities;

namespace GameOfLife.Mechanics
{
  public abstract class Entity : Entities
  {

    private GameGraphics _graphics;
    private Logic _logic;
    public byte Alpha = 255;
    protected byte _r = 0, _g = 0, _b = 0;
    protected int _width;
    protected int _height;
    protected bool _isVisible = true;
    protected bool _calculatePlacement = false;
    protected bool _isDeletable = false;
    protected bool _isDrawable = false;
    protected bool _isTestable = false;
    protected bool _isClickable = false;
    protected EntityNames _uid = EntityNames.None;
    protected EntityType _type = EntityType.None;
    protected PlacementType _placement = PlacementType.TopLeft;
    protected Entities _parent = null;

    public Entity(Entities parent, EntityNames name = EntityNames.None)
      : base(parent)
    { 
      this._graphics = parent.Graphics;
      this._logic = parent.Logic;
      this._parent = parent;
      this._uid = name;
    }

    public Entity(Entities parent, bool isUsedForTestPurposes)
      : this(parent)
    {
      this._isTestable = isUsedForTestPurposes;
    }

    public int R { get { return this._r; } }
    public int G { get { return this._g; } }
    public int B { get { return this._b; } }
    public int PositionX { get { return this.X.X; } }
    public int PositionY { get { return this.Y.Y; } }
    public virtual int Width { get { return this._width; } }
    public virtual int Height { get { return this._height; } }
    public GameGraphics Graphics { get { return this._graphics; } }
    public Logic Logic { get { return this._logic; } }
    public bool IsDeletable { get { return this._isDeletable; } }
    public EntityType Type { get { return this._type; } }
    public bool IsDrawable { get { return this._isDrawable; } }
    public EntityNames ID { get { return this._uid; } }
    public PlacementType Placement { get { return this._placement; } set { this._placement = value; } }
    public Point X = new Point(0, 0);
    public Point Y = new Point(0, 0);
    private int _tempX = -1, _tempY = -1;

    public virtual void SetColor(byte r, byte g, byte b)
    {
      this._r = r; this._g = g; this._b = b;
    }

    public virtual void SetAsTest()
    {
      this._isTestable = true;
    }

    public void SetVisibility(bool isVisible)
    {
      this._isVisible = isVisible;
    }

    #region Override methods

    public virtual void OnStart() { }
    public virtual void Draw() 
    {
      if (this._calculatePlacement)
        this.CalculatePlacement();
    }
    public virtual void OnEnd() { }

    protected virtual void SendStatistics() 
    {
      if (this._isDrawable)
        this._parent.DrawableItems++;
    }

    #endregion

    #region PlacementType

    private void CalculatePlacement()
    {
      if(this._tempX == -1 || this._tempY == -1)
      {
        this._tempX = this.X.X; this._tempY = this.Y.Y;
      }


      if (this._placement == PlacementType.TopLeft)
      {
        this.X.X = this._tempX;
        this.Y.Y = this._tempY;
      }
      else if(this._placement == PlacementType.TopCenter)
      {
        this.X.X = ((this.Graphics.Width / 2) - this.Width / 2) + this._tempX;
        this.Y.Y = this._tempY;
      }
      else if (this._placement == PlacementType.TopRight)
      {
        this.X.X = this.Graphics.Width - this.Width - this._tempX;
        this.Y.Y = this._tempY;
      }
      else if (this._placement == PlacementType.Center)
      {
        this.X.X = ((this.Graphics.Width / 2) - this.Width / 2) + this._tempX;
        this.Y.Y = ((this.Graphics.Height / 2) - this.Height / 2) + this._tempY;
      }
      else if (this._placement == PlacementType.BottomLeft)
      {
        this.X.X = this._tempX;
        this.Y.Y = this.Graphics.Height - this.Height - this._tempY;
      }
      else if(this._placement == PlacementType.BottomCenter)
      {
        this.X.X = ((this.Graphics.Width / 2) - this.Width / 2) + this._tempX;
        this.Y.Y = (this.Graphics.Height - this.Height) - this._tempY;
      }
      else if (this._placement == PlacementType.BottomRight)
      {
        this.X.X = this.Graphics.Width - this.Width - this._tempX;
        this.Y.Y = this.Graphics.Height - this.Height - this._tempY;
      }

    }

    #endregion

  }
}
