using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day19
{
  class Program
  {
    private static int MAX_DEPTH = 3;
    private static Dictionary<int, int> DepthCounter { get; set; }
    private static List<string> Messages { get; set; }
    
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());
      var parsedInput = ParseRulesAndMessages(lines);

      Messages = parsedInput.Messages;
      // MAX_DEPTH = parsedInput.Messages.Max(x => x.Length);
      DepthCounter = parsedInput.Rules.ToDictionary(x => x.Key, x => 0);

      parsedInput.Rules[8].SubRulesIds.Add(new List<int>{ 42, 8 });
      parsedInput.Rules[11].SubRulesIds.Add(new List<int>{ 42, 11, 31 });
      List<string> generatedCombinations = GetMessage(0, parsedInput.Rules);
      List<int> lengths = generatedCombinations.Select(x => x.Length).Distinct().ToList();

      int partOne = parsedInput.Messages.Intersect(generatedCombinations).Count();
      int partTwo = 0;

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms\nGenerated combinations: {generatedCombinations.Count}\nClose to max: {lengths.Max(x => x) * 100 / Messages.Max(x => x.Length)}%");
      
    }
    private static List<string> GetMessage(int number, Dictionary<int, Rule> rules)
    {
      if (rules[number].IsCharacter) return new List<string> {rules[number].Character};
      List<string> messages = new List<string>();
      if (rules[number].SubRulesIds.SelectMany(x => x).ToList().Contains(number))
      {
        DepthCounter[number]++;
        Console.WriteLine($"Number {number} going depth {DepthCounter[number]}");
        if(DepthCounter[number] > MAX_DEPTH) return new List<string>();
      }
      foreach (List<int> subRulesIds in rules[number].SubRulesIds)
      {
        List<string> messages2 = new List<string>();
        foreach (List<string> innerMessages in subRulesIds.Select(subRulesId => GetMessage(subRulesId, rules)))
        {
          if (innerMessages.Count == 1)
          {
            if (messages2.Count == 0)
            {
              messages2.AddRange(innerMessages);
            }
            else
            {
              messages2 = messages2.Select(x => x + innerMessages.First()).ToList();
            }
          }
          else
          {
            if (messages2.Any())
            {
              List<string> finalMessages = new List<string>();
              foreach (string message in messages2)
              {
                finalMessages.AddRange(innerMessages.Select(innerMessage => message + innerMessage));
              }

              messages2 = finalMessages;
            }
            else
            {
              messages2 = innerMessages;
            }
          }
        }
        messages.AddRange(messages2);
      }

      return messages.Where(x => Messages.Any(mes => mes.Contains(x))).ToList();
    }

    private static (Dictionary<int, Rule> Rules, List<string> Messages) ParseRulesAndMessages(string[] lines)
    {
      Dictionary<int, Rule> rules = new Dictionary<int, Rule>();
      List<string> messages = new List<string>();
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
          messages.Add(line);
        }
        else
        {
          Rule rule = new Rule();
          
          string[] parts = line.Split(':');
          if (parts[1].Contains('"'))
          {
            rule.Character = parts[1].SubstringBetween("\"", "\"");
            rules.Add(int.Parse(parts[0]), rule);
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
            rules.Add(int.Parse(parts[0]), rule);
          }
        }
      }

      return (rules, messages);
    }
  }
}