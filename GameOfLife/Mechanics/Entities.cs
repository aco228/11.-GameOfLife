using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Graphics;
using GameOfLife.Entities;
using GameOfLife.Helper;

namespace GameOfLife.Mechanics
{
  public class Entities
  {
    private GameGraphics _graphics = null;
    private List<Entity> _list = null;
    private Logic _logic = null;
    private int _xyLimit = 500;

    #region Statistics info

    private int _aliveItems = 0;
    private int _drawableItems = 0;

    #endregion

    public Entities(GameGraphics g)
    {
      this._graphics = g;
      this._list = new List<Entity>();
    }

    public Entities(Entities parent)
    {
      // copy
      this._graphics = parent._graphics;
      this._list = parent._list;
      this._logic = parent._logic;
    }

    #region Properties

    public List<Entity> List { get { return this._list; } }
    public GameGraphics Graphics { get { return this._graphics; } }
    public Logic Logic { get { return this._logic; } }

    public int Count
    {
      // Prebrojava zive celije!
      get
      {
        int back = 0;
        foreach (Entity e in this._list)
          if (e.Type == EntityType.Cell)
            back++;

        return back;
      }
    }

    public Entity At(int pos) { return this._list.ElementAt(pos); }
    public Entity Add(Entity newly) { this._list.Add(newly); return newly; }
    public void SetLogic(Logic logic) { this._logic = logic; }
    public int DrawableItems { get { return this._drawableItems; } set { this._drawableItems = value; } }
    public int AliveItems { get { return this._aliveItems; } set { this._aliveItems = value; } }

    public void Reset()
    {
      for (int i = 0; i < this._list.Count; i++)
      {
        if (this._list.ElementAt(i).Type == EntityType.Cell)
        {
          this._list.RemoveAt(i);
          i--;
        }
      }
    }

    public virtual Entity Get(EntityNames uid)
    {
      foreach (Entity e in this._list)
        if (uid == e.ID)
          return e;

      return null;
    }

    public void AddNewCell(int X, int Y, bool repositionIsDone = true, bool instantAdd = false, bool userAdd = false)
    {
      int posX, posY;
      if (!repositionIsDone)
      {
        // Repozicija nije uradjena.
        posX = ((int)Math.Ceiling((double)X / (double)this._logic.Zoom)) + this.Logic.RepositionX - 1;
        posY = ((int)Math.Ceiling((double)Y / (double)this._logic.Zoom)) + this.Logic.RepositionY - 1;
      }
      else
      {
        // Repozicija je prevedena
        posX = X; posY = Y;
      }

      if (posX < 0 || posX > _xyLimit || posY < 0 || posY > this._xyLimit)
        return;

      Cell cell = null;      

      if(!instantAdd)
      {
        // Provjera da li na tom mjestu postoji celija ili ne
        foreach (Entity e in this._list)
        {
          if (e.Type != EntityType.Cell) continue;
          cell = e as Cell;

          if (cell.PositionX == posX && cell.PositionY == posY)
          {
            if (userAdd)
            {
              // Slucaj kada korisnik dodaje novu celiju
              cell.SetAlive((cell.IsAlive) ? false : true);
              return;
            }

            if (!cell.IsAlive) cell.SetAlive(true);
            return;

          }
        }
      }

      cell = new Cell(posX, posY, this);
      cell.IsNewBorn = true; cell.SetAlive(true);

      this._list.Insert(0, cell);
      //this._list.Add(cell);
      this.Logic.Modification = true;
    }

    #endregion

    #region Events

    public void OnStart()
    {
      for (int i = 0; i < this._list.Count; i++)
      {
        if (this._list.ElementAt(i).IsDeletable)
        {
          this._list.RemoveAt(i);
          i--; continue;
        }

        this._list.ElementAt(i).OnStart();
      }
    }

    public void Draw()
    {
      this._drawableItems = 0;
      this._aliveItems = 0;
      foreach (Entity e in this._list)
      {
        e.Draw();

        //this.Statistics(e);
      }

      System.Diagnostics.Debug.WriteLine("DRAWABLE " + _drawableItems + "; ISALIVE: " + _aliveItems);

    }

    public void OnEnd()
    {
      if (this.Logic.Modification)
      {
        this.OnEndWithModifications();
        return;
      }
      
      for (int i = 0; i < this._list.Count; i++)
        this._list.ElementAt(i).OnEnd();

      //foreach (Entity e in this._list)
      //  e.OnEnd();
    }

    private void OnEndWithModifications()
    {
      foreach (Entity e in this._list)
      {
        if (e.Type == EntityType.Cell)
          continue;

        e.OnStart();
      }
    }

    private void Statistics(Entity entity)
    {
      if (entity.Type == EntityType.Cell && (entity as Cell).IsAlive)
        this._aliveItems++;

      if (entity.IsDrawable)
        this._drawableItems++;
    }

    #endregion

    #region Calculations

    public EntitiesCellResult NumberOfCellNeighbours(int posX, int posY, bool countNewBornCells = false)
    {

      EntitiesCellResult result = new EntitiesCellResult();

      if (posX == 2 && posY == 2)
        result.CellsAlive = 0;

      foreach (Entity e in this._list)
      {
        if (e.Type != EntityType.Cell) continue;
        Cell cell = e as Cell;

        if (cell.PositionX == posX && cell.PositionY == posY)
        {
          result.CellLocated = cell;
          continue;
        }

        //if (!cell.IsAlive)
        //  continue;

        if (!countNewBornCells && cell.IsNewBorn)
          continue;

        int difx = posX - cell.PositionX;
        int dify = posY - cell.PositionY;

        if (difx >= -1 && difx <= 1 && dify >= -1 && dify <= 1)
        {
          if (cell.IsAlive) result.CellsAlive++;
          else result.CellsDead++;
          if (cell.IsNewBorn) result.CellNewBorns++;
        }
      }

      return result;
    }

    public void CheckCellScope(Cell original, int depth)
    {
      if (!original.IsAlive || original.IsNewBorn)
        return;

      for (int y = original.PositionY - depth; y <= original.PositionY + depth; y++)
      {
        if (y < 0 || y > this._xyLimit) { y++; continue; }
        for (int x = original.PositionX - depth; x <= original.PositionX + depth; x++)
        {
          if (x < 0 || x > this._xyLimit) { x++; continue; }
          if (original.PositionX == x && original.PositionY == y) continue;

          EntitiesCellResult result = this.NumberOfCellNeighbours(x, y);
          if (result.CellsAlive == 0 && result.CellsDead == 0) continue;

          if (result.CellAliveReal_ForNewBorn == 3)
          {
            if (result.CellLocated == null)
            {
            //  Cell newCell = new Cell(x, y, this); newCell.IsNewBorn = true;
            //  this._list.Add(newCell);
            //  Console.WriteLine("New cell is born " + x + "x" + y);
              this.AddNewCell(x, y, true, false, false);
            }
            else if (!result.CellLocated.IsAlive)
            {
              result.CellLocated.IsNewBorn = true;
              result.CellLocated.SetAlive(true);
            }
          }

        }
      }
    }

    #endregion

  }
}
