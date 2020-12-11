namespace Day08
{
  public class CommandLine
  {
    public int Id { get; set; }
    public CommandType Type { get; set; }
    public int Number { get; set; }
    public bool Visited { get; set; }
    public bool Changed { get; set; }

    public CommandLine()
    {
      Visited = false;
      Changed = false;
    }
  }
}