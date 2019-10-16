using System;
using System.Reflection;
using System.Resources;


namespace Liven {
  public static class Resources {
    private static Assembly _asm = Assembly.GetExecutingAssembly();
    private static ResourceManager _mResource =
      new ResourceManager(
          "ResourceGenerator",
          _asm);

    public static System.Drawing.Icon Icon =
      _mResource.GetObject("icon.ico") as System.Drawing.Icon;
  }
}
