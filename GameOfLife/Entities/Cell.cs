using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Mechanics;
using GameOfLife.Helper;
using System.Collections;

namespace GameOfLife.Entities
{
  public class Cell : Entity
  {

    private bool _isAlive = false;
    private bool _isNewBorn = false;
    private int _positionX = -1;
    private int _positionY = -1;
    private int _padding = 2;
    private GameOfLife.Mechanics.Entities _parent = null;
    private SolidBrush _brush;
    
    public Cell(int posx, int posy, GameOfLife.Mechanics.Entities parent, EntityNames name = EntityNames.Cell)
      :base(parent, name)
    {
      this._isAlive = true;
      this._positionX = posx;
      this._positionY = posy;
      this.SetColor(255, 255, 255);
      this.Alpha = 0;
      this._parent = parent;
      this._isDrawable = true;

      this._brush = new SolidBrush(Color.FromArgb(this.Alpha, this.R, this.G, this.B));

      this._type = EntityType.Cell;
    }

    public Cell(int posx, int posy, GameOfLife.Mechanics.Entities parent, bool isUsedForTestPurposes)
      : this(posx, posy, parent)
    {
      this._isTestable = isUsedForTestPurposes;
    }

    public bool IsAlive { get { return this._isAlive; } }
    public bool IsNewBorn { get { return this._isNewBorn; } set { this._isNewBorn = value; } }
    public int PositionX { get { return this._positionX; } }
    public int PositionY { get { return this._positionY; } }

    public void SetAlive(bool isAlive) { this._isAlive = isAlive; }

    #region Override

    public override void OnStart()
    {
      if(this._isNewBorn)
      {
        this._isNewBorn = false;
        this._isAlive = true;
        this._isDrawable = true;
      }

      if (this._isAlive) 
        this.StartAliveFunctionality();
      else 
        this.StartDeadFunctionality();
    
      
    }

    public override void Draw()
    {
      if (!this._isDrawable || !this._isVisible)
        return;

      this._brush.Color = Color.FromArgb(this.Alpha, this.R, this.G, this.B);

      int x = ((this._positionX - this.Logic.RepositionX) * this.Logic.Zoom) + this._padding;
      int y = ((this._positionY - this.Logic.RepositionY) * this.Logic.Zoom) + this._padding;
      int width = this.Logic.Zoom - (this._padding * 2);

      this.Graphics.G.FillEllipse(this._brush, new Rectangle(x, y, width, width));
          
      base.Draw();
      this._parent.DrawableItems++;
    }

    public override void OnEnd()
    {
      if (this._isNewBorn)
        return;

      this._parent.CheckCellScope(this, 1);
      
      EntitiesCellResult result = this.NumberOfCellNeighbours(this._positionX, this._positionY);
      
      if (this._isAlive)
        EndAliveFuctionality(result);
      else
        EndDeadFuctionality(result);

      if (!this._isAlive)
        this._isDeletable = true;
    }

    protected override void SendStatistics()
    {
      //base.SendStatistics();
    }

    #endregion


    #region Cell specific methods

    private bool FindIsItDrawable()
    {
      int a; if(this._isTestable)
        a = 0;

      if (!this._isAlive)
        return false;

      if (this._positionX < this.Logic.RepositionX || this._positionY < this.Logic.RepositionY)
        return false;

      int numOfViewedPositionsX = (int)Math.Ceiling((double)this.Logic.Width / this.Logic.Zoom);
      int numOfViewedPositionsY = (int)Math.Ceiling((double)this.Logic.Height / this.Logic.Zoom);


      if (this._positionX > numOfViewedPositionsX + this.Logic.RepositionX || this._positionY > this.Logic.RepositionY + numOfViewedPositionsY)
        return false;

      //if (this._positionX + numOfViewedPositionsX > this.Logic.RepositionX || this._positionY + numOfViewedPositionsY > this.Logic.RepositionY)
      //  return false;

      return true;
    }


    private void StartAliveFunctionality()
    {

      this._isDrawable = this.FindIsItDrawable();
      if (this.Alpha < 250) this.Alpha=250;

    }

    private void StartDeadFunctionality()
    {
      this._isDrawable = false;
      if (this.Alpha > 0) this.Alpha-=20;
      if (this.Alpha == 0) this._isDeletable = true;
    }


    private void EndAliveFuctionality(EntitiesCellResult result)
    {
      if (result.CellAliveReal_ForNewBorn < 2)
        this._isAlive = false;

      else if (result.CellAliveReal_ForNewBorn >= 2 && result.CellAliveReal_ForNewBorn <= 3)
        return;

      else if (result.CellAliveReal_ForNewBorn > 3)
        this._isAlive = false;

    }
    private void EndDeadFuctionality(EntitiesCellResult result)
    {
      if (result.CellAliveReal_ForNewBorn == 3)
        this._isAlive = true;
    }

    #endregion

  }
}
