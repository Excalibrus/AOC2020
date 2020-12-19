using System.Collections.Generic;

namespace Day19
{
  public class Rule
  {
    public string Character { get; set; }
    public bool IsCharacter => !string.IsNullOrWhiteSpace(Character);
    public List<List<int>> SubRulesIds { get; set; }

    public Rule()
    {
      SubRulesIds = new List<List<int>>();
    }
  }
}