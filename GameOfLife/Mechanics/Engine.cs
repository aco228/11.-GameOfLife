using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GameOfLife.Graphics;
using GameOfLife.Entities;
using GameOfLife.Entities.Clickables;

namespace GameOfLife.Mechanics
{
  public class Engine
  {

    private GameForm form = null;
    private GameGraphics graphics = null;
    private Entities entites = null;
    private Logic logic = null;
    
    // Frame counting
    private int _tempSeconds = -1;
    private int _fps = -1;
    private int _frames = 0;
    private bool _firstRun = true;

    public Engine(GameForm form, GameGraphics graphics)
    { this.Load(form, graphics); }

    public Engine(GameForm form)
    { this.Load(form, null); }

    private void Load(GameForm form, GameGraphics graphics = null)
    {
      this.form = form;
      this.logic = new Logic(form);

      if (graphics == null)
        this.graphics = new GameGraphics(this.form, this.logic);
      else
        this.graphics = graphics;

      this.entites = new Entities(this.graphics);
      this.entites.SetLogic(this.logic);
      this.logic.SetEntities(this.entites);

      // Za dodavnje celija od strane igraca
      this.form.MouseDown += form_MouseDown;
      this.form.MouseUp += form_MouseUp;

      this.InitiateStartElements();
    }

    private void InitiateStartElements()
    {

      var cell = this.entites.Add(new Cell(1, 1, this.entites, EntityNames.TestCell)) as Cell;
      cell.SetAlive(false);

      this.entites.Add(new Cell(2, 1, this.entites));
      this.entites.Add(new Cell(3, 2, this.entites));
      this.entites.Add(new Cell(1, 3, this.entites));
      this.entites.Add(new Cell(2, 3, this.entites));
      this.entites.Add(new Cell(3, 3, this.entites));

      /// Cell with test features
      //this.entites.Add(new Cell(92, 92, this.entites, true));                               /
      // Static graphics
      //this.entites.Add(new StaticGraphics(Resources.download, StaticGraphicsSnapType.BottomRight, this.entites, 25, 10));
      // Static graphics       
      //this.entites.Add(new StaticGraphics(Resources.download, StaticGraphicsSnapType.Center, this.entites));         
      // Clickable graphics
      var testButton = this.entites.Add(new TestButton(this.entites, this.logic, PlacementType.BottomRight)) as TestButton;
      testButton.SetScale(3.5);

      this.entites.Add(new Grid(this.entites));

      //TextGraphics
      var postoji = this.entites.Add(
        new TextGraphics(
          new string[] { "Постоји", "0", "ћелија" }, 
          PlacementType.TopLeft, 
          this.entites, 
          EntityNames.TextNumberOfCells, 
          5, 5)) 
        as TextGraphics;
      postoji.SetFont("Arial", 10);

      var demoTxt = this.entites.Add(
        new TextGraphics(
          "Демо верзија 'Game of life' Александар Конатар и Стефан Шкулетић", 
          PlacementType.TopCenter, 
          this.entites, 
          EntityNames.TextDemoDescription, 
          0, 5)) 
        as TextGraphics;
      demoTxt.SetFont("Calibri", 15);

      var fps = this.entites.Add(
        new TextGraphics(
          new string[] { "FPS:", "0" }, 
          PlacementType.BottomLeft, 
          this.entites, 
          EntityNames.TextFPS, 
          5, 5))
        as TextGraphics;
      fps.SetFont("Calibri", 10);


      var zoom = this.entites.Add(
        new TextGraphics(
          new string[] { "Увеличање:", "0" }, 
          PlacementType.BottomLeft, 
          this.entites, 
          EntityNames.TextZoom, 
          5, fps.Height + 5)) 
        as TextGraphics;
      zoom.SetFont("Calibri", 10);

      var position = this.entites.Add(
        new TextGraphics(
          new string[] { "Позиција:", "0", " x ", "0" }, 
          PlacementType.BottomLeft, 
          this.entites, 
          EntityNames.TextPosition, 
          5, fps.Height + zoom.Height + 5)) 
        as TextGraphics;
      position.SetFont("Calibri", 10);

      var paused = this.entites.Add(
        new TextGraphics(
          "ПАУЗИРАНО!", 
          PlacementType.Center, 
          this.entites, 
          EntityNames.TextPaused)) 
        as TextGraphics;
      paused.Alpha = 150;
      paused.SetVisibility(false);



      var testDeleteButton = this.entites.Add(
        new TestDeleteButton(
          this.entites,
          this.logic,
          PlacementType.BottomRight,
          testButton.Width + 5, 5))
        as TestDeleteButton;
      testDeleteButton.ResizeHeight(testButton.Height);

      var testSaveButton = this.entites.Add(
        new TestSaveButton(
          this.entites,
          this.logic,
          PlacementType.BottomRight,
          testDeleteButton.PositionX + testDeleteButton.Width + 5, 5))
        as TestSaveButton;
      testSaveButton.ResizeHeight(testButton.Height);

      var testOpenButton = this.entites.Add(
        new TestOpenButton(
          this.entites,
          this.logic,
          PlacementType.BottomRight,
          testSaveButton.PositionX + testSaveButton.Width + 5, 5))
        as TestOpenButton;
      testOpenButton.ResizeHeight(testButton.Height);

      var testPlayButton = this.entites.Add(
        new TestPlayButton(
          this.entites, this.logic,
          PlacementType.BottomRight,
          testOpenButton.PositionX + testOpenButton.Width + 5, 5))
        as TestPlayButton;
      testPlayButton.ResizeHeight(testButton.Height);


    }

    #region Form Events

    private void form_MouseDown(object sender, MouseEventArgs e)
    {
      foreach(Entity en in this.entites.List)
      {
        if (en.Type != EntityType.ClickableGraphics) continue;
        ClickableGraphics click = en as ClickableGraphics;

        if(click.IsClicked(e.X, e.Y))
        {
          click.OnMouseDown();
          return;
        }
      }

      //if (!this.logic.IsPaused)
      //  return;

      this.entites.AddNewCell(e.X, e.Y, false, false, true);
      this.logic.Modification = true;
    }


    private void form_MouseUp(object sender, MouseEventArgs e)
    {
      foreach(Entity en in this.entites.List)
      {
        if (en.Type != EntityType.ClickableGraphics) continue;
         ClickableGraphics click = en as ClickableGraphics;

        if(click.Clicked)
        {
          click.OnMouseUp();
          return;
        }
      }
    }

    #endregion

    public int FPS { get { return this._fps; } }

    public void Run()
    {
      this.Loop();
    }
    
    private void Loop()
    {
      this._tempSeconds = DateTime.Now.Second;
      
      for(;;)
      {
        Application.DoEvents();

        // Exit
        if (logic.CloseApplication)
          break;

        if (logic.TestActivated)
          RunTest();

        if (logic.IsPaused && !this._firstRun && !this.logic.Modification)
          continue;

        this.FpsCounter();

        this.graphics.BeforeDraw();
        this.entites.OnStart();

        this.graphics.Drawing();
        this.entites.Draw();
        this.AditionalGraphics();
        
        this.entites.OnEnd();
        this.graphics.AfterDraw();
        
        this.logic.Modification = false;
        this._firstRun = false;
        this._frames++;
      }
    }

    private void AditionalGraphics()
    {
      var paused = (this.entites.Get(EntityNames.TextPaused) as TextGraphics);
      var cellCount = (this.entites.Get(EntityNames.TextNumberOfCells) as TextGraphics);
      var position = (this.entites.Get(EntityNames.TextPosition) as TextGraphics);
      var zoom = (this.entites.Get(EntityNames.TextZoom) as TextGraphics);

      paused.SetVisibility(this.logic.IsPaused);
      paused.Draw();

      if (logic.IsPaused)
        paused.Draw();
      
      position.Modifiy(1, this.logic.RepositionX.ToString());
      position.Modifiy(3, this.logic.RepositionY.ToString());
      zoom.Modifiy(1, this.logic.Zoom.ToString());
      cellCount.Modifiy(1, this.entites.Count.ToString());
    }

    private void FpsCounter()
    {
      if(DateTime.Now.Second != this._tempSeconds)
      {
        this._fps = this._frames;
        this._frames = 0;
        this._tempSeconds = DateTime.Now.Second;
        (this.entites.Get(EntityNames.TextFPS) as TextGraphics).Modifiy(1, this._fps.ToString());
      }
    }

    private void RunTest()
    {
      System.Diagnostics.Debug.WriteLine("");
      int a = 0;
      System.Diagnostics.Debug.WriteLine("Test working ");
      System.Diagnostics.Debug.WriteLine("");  
    }

  }
}
