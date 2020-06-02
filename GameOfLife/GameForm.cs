using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GameOfLife.Graphics;
using GameOfLife.Mechanics;

namespace GameOfLife
{
  public partial class GameForm : Form
  {

    private Engine engine = null;

    public GameForm()
    {
      InitializeComponent();
      this.Width = 1000;
      this.Height = 650;

      this.engine = new Engine(this);
    }


    private void GameForm_Load(object sender, EventArgs e)
    {
      //int a = 1 % 5;
      //MessageBox.Show(a.ToString());

      //Cursor.Hide();
      this.Text = "Demo verzija 'Game of life', projekat za Matematicko Modeliranje";
      this.Focus();
      this.Show();
      this.engine.Run();

    }



  }
}
