using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Common;

namespace Day19
{
  class Program
  {
    private static List<string> Messages { get; set; }
    private static Dictionary<int, Rule> Rules { get; set; }

    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());
      ParseRulesAndMessages(lines);

      int partOne = Messages.Sum(t => ValidateMessage(t, 0, 0).Any(x => x == t.Length) ? 1 : 0);

      Rules[8].SubRulesIds.Add(new List<int> {42, 8});
      Rules[11].SubRulesIds.Add(new List<int> {42, 11, 31});

      int partTwo = Messages.Sum(t => ValidateMessage(t, 0, 0).Any(x => x == t.Length) ? 1 : 0);

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\n" +
                        $"Part two result: {partTwo}\n" +
                        $"Execution time: {watch.ElapsedMilliseconds} ms\n" +
                        "");
    }

    private static List<int> ValidateMessage(string message, int index, int ruleNumber)
    {
      if (Rules[ruleNumber].IsCharacter)
      {
        if (index < message.Length && message.Substring(index, 1) == Rules[ruleNumber].Character)
        {
          return new List<int> {index + 1};
        }

        return new List<int>();
      }

      List<int> subRuleGroupIndexes = new List<int>();
      foreach (List<int> subRuleIdsGroup in Rules[ruleNumber].SubRulesIds)
      {
        List<int> relevantIndexes = GetRelevantIndexes(message, index, subRuleIdsGroup);
        if (relevantIndexes.Any())
        {
          subRuleGroupIndexes.AddRange(relevantIndexes);
        }
      }

      return subRuleGroupIndexes;
    }

    private static List<int> GetRelevantIndexes(string message, int index, List<int> ruleIds)
    {
      List<int> relevantIndexes = new List<int>();
      foreach (int subRuleId in ruleIds)
      {
        if (relevantIndexes.Any())
        {
          List<int> newIndexes = new List<int>();
          foreach (int relevantIndex in relevantIndexes)
          {
            List<int> validationResponses = ValidateMessage(message, relevantIndex, subRuleId);
            if (validationResponses.Count > 0)
            {
              newIndexes.AddRange(validationResponses);
            }
          }

          if (newIndexes.Count == 0)
          {
            return new List<int>();
          }

          relevantIndexes = newIndexes.Distinct().ToList();
        }
        else
        {
          List<int> validationResponses = ValidateMessage(message, index, subRuleId);
          if (validationResponses.Count == 0)
          {
            return new List<int>();
          }

          relevantIndexes.AddRange(validationResponses);
        }
      }

      return relevantIndexes.Where(x => message.Length >= x).ToList();
    }


    private static void ParseRulesAndMessages(string[] lines)
    {
      Messages = new List<string>();
      Rules = new Dictionary<int, Rule>();
      bool parseMessages = false;
      foreach (string line in lines)
      {
        if (string.IsNullOrWhiteSpace(line))
        {
          parseMessages = true;
          continue;
        }

        if (parseMessages)
        {
          Messages.Add(line);
        }
        else
        {
          Rule rule = new Rule();

          string[] parts = line.Split(':');
          int ruleId = int.Parse(parts[0]);
          if (parts[1].Contains('"'))
          {
            rule.Character = parts[1].SubstringBetween("\"", "\"");
            Rules.Add(ruleId, rule);
          }
          else
          {
            string[] orParts = parts[1].Split('|');
            foreach (string orPart in orParts)
            {
              rule.SubRulesIds.Add(
                  orPart.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                      .ToList());
            }

            Rules.Add(ruleId, rule);
          }
        }
      }
    }
  }
}