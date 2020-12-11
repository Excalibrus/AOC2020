using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace Day06
{
  class Program
  {
    static void Main(string[] args)
    {
      string[] fileLines = File.ReadAllLines("day06.txt".ToApplicationPath());

      int partOne = GetAnswersPartOne(fileLines);
      int partTwo = GetAnswersPartTwo(fileLines);
    }

    static int GetAnswersPartOne(string[] fileLines)
    {
      int answers = 0;
      string currentGroup = string.Empty;
      foreach (string fileLine in fileLines)
      {
        if (string.IsNullOrWhiteSpace(fileLine))
        {
          answers += currentGroup.Distinct().Count();
          currentGroup = string.Empty;
        }
        else
        {
          currentGroup += fileLine;
        }
      }
      answers += currentGroup.Distinct().Count();
      return answers;
    }
    
    static int GetAnswersPartTwo(string[] fileLines)
    {
      int answers = 0;
      List<string> currentGroup = new List<string>();
      foreach (string fileLine in fileLines)
      {
        if (string.IsNullOrWhiteSpace(fileLine))
        {
          answers += string.Join("", currentGroup).Distinct().Count(x => currentGroup.All(y => y.Contains(x)));
          currentGroup = new List<string>();
        }
        else
        {
          currentGroup.Add(fileLine);
        }
      }
      answers += string.Join("", currentGroup).Distinct().Count(x => currentGroup.All(y => y.Contains(x)));
      return answers;
    }
  }
}