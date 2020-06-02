using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Mechanics
{
  public enum EntityType
  {
    None = 0,
    Cell = 1,
    StaticGraphics = 2,
    ClickableGraphics = 3,
    TextGraphics = 4,
    Grid = 5
  }
}
