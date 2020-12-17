using System.Collections.Generic;
using System.Linq;

namespace Day16
{
  public class Ticket
  {
    public List<long> Numbers { get; set; }
    public Dictionary<int, List<string>> MatchingRules { get; set; }

    public Ticket(string line)
    {
      MatchingRules = new Dictionary<int, List<string>>();
      Numbers = line.Split(',').Select(long.Parse).ToList();
    }

    public Ticket()
    {
      MatchingRules = new Dictionary<int, List<string>>();
      Numbers = new List<long>();
    }
  }
}