using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Mechanics
{
  public class Logic
  {
    private int _zoom = 25;
    private int _repositionX = 100;
    private int _repositionY = 100;
    private int _width = 0;
    private int _height = 0;
    private bool _closeAplication = false;
    private bool _testActivated = false;
    private bool _drawModification = false;
    private bool _isPaused = true;
    private Entities _entites = null;
    private GameForm _form;

    public Logic(GameForm form)
    {
      this._form = form;
      this._width = form.ClientSize.Width;
      this._height = form.ClientSize.Height;

      form.SizeChanged += form_SizeChanged;
      form.KeyDown += form_KeyDown;
      form.KeyUp += form_KeyUp;
      form.FormClosing += form_FormClosing;
    }
            
    #region Properties

    public int Zoom { get { return this._zoom; } }
    public int RepositionX { get { return this._repositionX; } }
    public int RepositionY { get { return this._repositionY; } }
    public int Width { get { return this._width; } }
    public int Height { get { return this._height; } }
    public bool CloseApplication { get { return this._closeAplication; } }
    public bool TestActivated { get { return this._testActivated; } }
    public bool IsPaused { get { return this._isPaused; } }
    public bool Modification { get { return this._drawModification; } set { this._drawModification = value; } }
        
    public void ZoomIn(int br = 0)
    {
      if (br > 0)
        this._zoom += br;
      else
        this._zoom++;

      if (this._zoom > 150) this._zoom = 150;
    }

    public void ZoomOut(int br = 0)
    {
      if (br > 0)
        this._zoom -= br;
      else
        this._zoom--;

      if (this._zoom < 6) this._zoom = 6;

    }

    public void RepositeX(int move)
    {
      this._repositionX += move;
      if (this._repositionX < 0)
        this._repositionX = 0;
    }

    public void RepositeY(int move)
    {
      this._repositionY += move;
      if (this._repositionY < 0)
        this._repositionY = 0;
    }

    public void Pause(bool? pause = null)
    {
      if (pause.HasValue)
        this._isPaused = pause.Value;
      
      this._isPaused = (this._isPaused) ? false : true;
      this._drawModification = true;
    }

    public void SetEntities(Entities entities)
    {
      this._entites = entities;
    }
    
    #endregion

    #region Events
        
    private void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      switch(e.KeyCode)
      {
        // For zooming
        case System.Windows.Forms.Keys.Add:
          this.ZoomIn();
          this._drawModification = true;
          break;

        case System.Windows.Forms.Keys.Subtract:
          this.ZoomOut();
          this._drawModification = true;
          break;

        // For moving grid
        case System.Windows.Forms.Keys.A:
        case System.Windows.Forms.Keys.Left:
          this.RepositeX(-1);
          this._drawModification = true;
          break;

        case System.Windows.Forms.Keys.D:
        case System.Windows.Forms.Keys.Right:
          this.RepositeX(1);
          this._drawModification = true;
          break;

        case System.Windows.Forms.Keys.W:
        case System.Windows.Forms.Keys.Up:
          this.RepositeY(-1);
          this._drawModification = true;
          break;

        case System.Windows.Forms.Keys.S:
        case System.Windows.Forms.Keys.Down:
          this.RepositeY(1);
          this._drawModification = true;
          break;

        // Test button
        case System.Windows.Forms.Keys.T:
          this._testActivated = true;
          this._drawModification = true;
          break;

        // Pause
        case System.Windows.Forms.Keys.Space:
          this.Pause();
          this._drawModification = true;
          break;

        // Delete all cell
        case System.Windows.Forms.Keys.X:
          this.DeleteCells();
          break;

        // Open grid
        case System.Windows.Forms.Keys.O:
          this.OpenFileDialog();
          break;

        case System.Windows.Forms.Keys.P:
          this.SaveFileDialog();
          break;

        // Closing program
        case System.Windows.Forms.Keys.Escape:
          this._form.Close();
          break;
          
      }
    }
      
    private void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      switch(e.KeyCode)
      {
        // Close test button
        case System.Windows.Forms.Keys.T:
          this._testActivated = false;
          break;
      }
    }

    private void form_SizeChanged(object sender, EventArgs e)
    {
      this._width = this._form.ClientSize.Width;
      this._height = this._form.ClientSize.Height;
      this._drawModification = true;
    }

    private void form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
    {
      this._closeAplication = true;
      this._form.Dispose();
    }

    #endregion

    #region NonGeneric stuff sve im jebem
    // Used for opening and saving cells 

    public void OpenFileDialog()
    {
      System.Windows.Forms.OpenFileDialog file = new System.Windows.Forms.OpenFileDialog();
      file.Filter = "GameOfLife files|*.gof";
      file.Title = "Отвори сачувани положај ћелија";

      if (file.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;

      this._entites.Reset();

      using(GameFile gameFile = new GameFile(this._entites))
        gameFile.OpenFile(file.FileName);

      this._drawModification = true;
    }

    public void SaveFileDialog()
    {
      System.Windows.Forms.SaveFileDialog file = new System.Windows.Forms.SaveFileDialog();
      file.Filter = "GameOfLife files|*.gof";
      file.Title = "Сачувај положај ћелија";

      if (file.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;

      using (GameFile gameFile = new GameFile(this._entites))
        gameFile.SaveFile(file.FileName);

    }

    public void DeleteCells()
    {
      this._entites.Reset();
      this._drawModification = true;
    }

    #endregion

  }
}
