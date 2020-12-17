using System;

namespace Day16
{
  public class TicketRule
  {
    public string Name { get; set; }
    public long Min1 { get; set; }
    public long Max1 { get; set; }
    public long Min2 { get; set; }
    public long Max2 { get; set; }

    public TicketRule(string line)
    {
      string[] lineParts = line.Split(':');
      Name = lineParts[0];
      string[] ranges = lineParts[1].Split(new[] {"or"}, StringSplitOptions.RemoveEmptyEntries);
      string[] range1 = ranges[0].Split('-');
      string[] range2 = ranges[1].Split('-');
      Min1 = long.Parse(range1[0]);
      Max1 = long.Parse(range1[1]);
      Min2 = long.Parse(range2[0]);
      Max2 = long.Parse(range2[1]);
    }
    
    public bool IsValid(long number)
    {
      return Min1 <= number && Max1 >= number ||
             Min2 <= number && Max2 >= number;
    }
  }
}