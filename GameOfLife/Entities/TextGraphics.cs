using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GameOfLife.Mechanics;

namespace GameOfLife.Entities
{
  class TextGraphics : Entity
  {

    protected string[] _textParts = null;
    protected Font _font;
    protected int _fontSize = 25;
    protected SolidBrush _brush;
    protected SolidBrush _shadowBrush;

    public override int Width { get { return (int)Math.Ceiling(this.Graphics.G.MeasureString(this.Text, this._font).Width); } }
    public override int Height { get { return this._fontSize + 5; } }

    public string Text
    {
      get
      {
        string back = "";
        foreach (string s in this._textParts)
        {
          if (back != "") back += " ";
          back += s;
        }
        return back;
      }
    }

    public void Modifiy(int i, string mod)
    {
      if (i > this._textParts.Length)
        return;

      this._textParts[i] = mod;
    }
    
    public TextGraphics(string text, PlacementType type, GameOfLife.Mechanics.Entities parent, EntityNames name = EntityNames.TextGraphics, int posX = 0, int posY = 0)
      :base(parent, name)
    {
      this._textParts = new string[] { text };
      this._type = EntityType.TextGraphics;
      this.X.X = posX; this.Y.Y = posY;
      this._placement = type;
      this._font = new Font("Calibri", this._fontSize);
      this._brush = new SolidBrush(Color.FromArgb(this._r, this._g, this._b));
      this._shadowBrush = new SolidBrush(Color.FromArgb(250, 0, 0, 0));
      this.SetColor(255, 255, 255);
      this._calculatePlacement = true;
    }

    public TextGraphics(string[] textData, PlacementType type, GameOfLife.Mechanics.Entities parent, EntityNames name = EntityNames.TextGraphics, int posX = 0, int posY = 0)
      :this(textData.ToString(), type, parent, name, posX, posY)
    {
      this._textParts = textData;
    }

    public void SetFont(string font, int size)
    {
      this._fontSize = size;
      this._font = new Font(font, this._fontSize);
    }

    public override void SetColor(byte r, byte g, byte b)
    {
      base.SetColor(r, g, b);
      this._brush.Color = Color.FromArgb(this._r, this._g, this._b);
    }

    public void SetShadowColor(byte a, byte r, byte g, byte b)
    {
      this._shadowBrush.Color = Color.FromArgb(a, r, g, b);
    }


    public override void OnStart()
    {
      base.OnStart();
    }

    public override void Draw()
    {
      if (!this._isVisible) return;
      
      base.Draw();

      this.Graphics.G.DrawString(this.Text, this._font, this._shadowBrush, this.X.X + 1, this.Y.Y + 1);
      this.Graphics.G.DrawString(this.Text, this._font, this._brush, this.X.X, this.Y.Y);
    }

    public override void OnEnd()
    {
      base.OnEnd();
    }


  }
}
