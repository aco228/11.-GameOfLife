using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Mechanics;

namespace GameOfLife.Entities.Clickables
{
  class TestSaveButton : ClickableGraphics
  {
    public TestSaveButton(GameOfLife.Mechanics.Entities parent, Logic logic, PlacementType type, int posx = 0, int posy = 0)
      : base(Resources.test_save, type, parent, EntityNames.TestDeleteButton, posx, posy)
    {
      this._logic = logic;
    }

    private Logic _logic = null;

    public override void OnMouseUp()
    {
      base.OnMouseUp();
      this._logic.SaveFileDialog();
    }

  }
}
