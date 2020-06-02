using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameOfLife.Entities;

namespace GameOfLife.Mechanics
{
  public class GameFile : IDisposable
  {
    public GameFile(Entities entities)
    {
      this._entities = entities;
    }

    protected GameOfLife.Mechanics.Entities _entities = null;
    protected string _fileName = string.Empty;
    protected StreamReader _reader = null;
    protected StreamWriter _writer = null;

    private bool File(string filePath)
    {
      FileInfo file = new FileInfo(filePath);
      if (!file.Exists)
        return false;

      this._fileName = filePath;
      return true;
    }

    public void OpenFile(string file)
    {
      if (!this.File(file))
        return;

      this._reader = new StreamReader(this._fileName);
      string line;
      while((line = this._reader.ReadLine()) != null)
      {
        string[] data = line.Trim().Split('#');
        if (data.Length != 2) continue;

        int num1 = -1, num2 = -1;

        Int32.TryParse(data[0], out num1);
        Int32.TryParse(data[1], out num2);

        if (num1 == -1 || num2 == -1) continue;

        this._entities.Add(new Cell(num1, num2, this._entities));
      }
    }

    public void SaveFile(string file)
    {
      if (!this.File(file))
      {
        var newFile = System.IO.File.Create(file);
        this._fileName = file;
        newFile.Close();
      }

      this._writer = new StreamWriter(this._fileName, true, Encoding.UTF8);

      foreach(Entity e in this._entities.List)
      {
        if (e.Type != EntityType.Cell)
          continue;

        Cell c = e as Cell;

        if (!c.IsAlive)
          continue;

        this._writer.WriteLine(string.Format("{0}#{1}", c.PositionX, c.PositionY));
      }

      this._writer.Close();
    }


    public void Dispose()
    {
    }
  }
}
