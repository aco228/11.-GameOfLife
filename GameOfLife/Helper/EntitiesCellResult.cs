using GameOfLife.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Helper
{
  public class EntitiesCellResult
  {
    public int CellsAlive = 0;
    public int CellsDead = 0;
    public int CellNewBorns = 0;
    public Cell CellLocated = null;

    public int CellAliveReal_ForNewBorn { get { return (this.CellsAlive + this.CellsDead) - this.CellNewBorns; } }
    public int CellAliveReal_ForDead { get { return this.CellsAlive + this.CellNewBorns; } }
  }
}
