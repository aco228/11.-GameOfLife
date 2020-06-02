using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GameOfLife.Mechanics;

namespace GameOfLife.Entities
{
  public class ClickableGraphics : StaticGraphics
  {

    protected bool _clicked = false;

    public ClickableGraphics(Bitmap image, PlacementType type, GameOfLife.Mechanics.Entities parent, EntityNames name = EntityNames.ClickableGraphics, int posx = 0, int posy = 0)
      : base(image, type, parent, name, posx, posy)
    {
      this._isClickable = true;
      this._type = EntityType.ClickableGraphics;
    }

    public bool Clicked { get { return this._clicked; } }

    public virtual bool IsClicked(int x, int y)
    {

      if(x >= this.PositionX  && x <= (this.PositionX + this._width))
        if (y >= this.PositionY  && y <= this.PositionY + this._height)
          return true;

      return false;
    }

    public virtual void OnMouseDown() 
    {
      this._clicked = true;
    }

    public virtual void OnMouseUp() 
    {
      this._clicked = false;
    }

  }
}
