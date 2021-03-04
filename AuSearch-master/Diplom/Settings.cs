using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace BW.Diplom
{
  public class Settings
  {
    private Point location = new Point(4, 4);
    public Point Location { get { return location; } set { location = value; } }
    private Size size = new Size(320, 160);
    public Size Size { get => size; set => size = value; }

    public int Colour { get => colour; set { colour = value; } }
    private int colour = Color.White.ToArgb();
  }
}
