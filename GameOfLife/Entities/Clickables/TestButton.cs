using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Entities;
using GameOfLife.Mechanics;

namespace GameOfLife.Entities.Clickables
{
  public class TestButton : ClickableGraphics
  {
    public TestButton(GameOfLife.Mechanics.Entities parent, Logic logic, PlacementType type, EntityNames name = EntityNames.TestButton, int posx = 0, int posy = 0)
      : base(Resources.button, type, parent, name, posx, posy)
    {
      this._logic = logic;
    }

    private Logic _logic = null;

    public override void OnMouseDown()
    {
      this._logic.Pause();      
      base.OnMouseDown();
    }

    public override void OnMouseUp()
    {
      Console.WriteLine("onMouseUp"); ;
      base.OnMouseUp();
    }

  }
}
