using System.Collections.Generic;

namespace Day19
{
  public class Path
  {
    public int Num { get; set; }
    public string Char { get; set; }
    public List<Path> Children { get; set; }

    public Path()
    {
      Children = new List<Path>();
    }
  }
}