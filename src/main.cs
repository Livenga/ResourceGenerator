using System;
using System.Drawing;
using System.Windows.Forms;


namespace Liven {
  public class ResGenMain {
    [STAThread]
    public static void Main(string[] args) {
      Application.EnableVisualStyles();
      Application.Run(new Liven.Forms.ResGenForm());
    }
  }
}
