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
    private static int MAX_DEPTH = 10000;
    private static Dictionary<int, int> DepthCounter { get; set; }
    private static List<string> Messages { get; set; }
    private static Dictionary<int, int> MessageIndexer { get; set; }
    private static Dictionary<int, bool?> MessageSuccess { get; set; }
    private static Dictionary<int, string> MessageResult { get; set; }
    private static Dictionary<int, List<int>> MessagePath { get; set; }
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      string[] lines = File.ReadAllLines("input-test2.txt".ToApplicationPath());
      var parsedInput = ParseRulesAndMessages(lines);

      Messages = parsedInput.Messages;
      MessageIndexer = Messages.Select((i, j) => new {j}).ToDictionary(x => x.j, x => 0);
      MessageSuccess = Messages.Select((i, j) => new {j}).ToDictionary(x => x.j, x => (bool?)null);
      MessagePath = Messages.Select((i, j) => new {j}).ToDictionary(x => x.j, x => new List<int>());
      MessageResult = Messages.Select((i, j) => new {j}).ToDictionary(x => x.j, x => string.Empty);
      int successesP1 = 0;
      for (int i = 0; i < Messages.Count; i++)
      {
        var response = ValidateMessage2(Messages[i], i, 0, 0, parsedInput.Rules);
        if (response.Any(x => x.Success && x.Index == Messages[i].Length))
        {
          successesP1++;
        }
      }

      // List<string> generatedCombinations = GetMessage(0, parsedInput.Rules);
      // List<int> lengths = generatedCombinations.Select(x => x.Length).Distinct().ToList();

      int partOne = successesP1;
      
      DepthCounter = parsedInput.Rules.ToDictionary(x => x.Key, x => 0);

      MessagePath = Messages.Select((i, j) => new {j}).ToDictionary(x => x.j, x => new List<int>());
      MessageSuccess = Messages.Select((i, j) => new {j}).ToDictionary(x => x.j, x => (bool?)null);
      MessageResult = Messages.Select((i, j) => new {j}).ToDictionary(x => x.j, x => string.Empty);
      parsedInput.Rules[8].SubRulesIds.Add(new List<int> {42, 8});
      parsedInput.Rules[11].SubRulesIds.Add(new List<int> {42, 11, 31});
      int successCounter = 0;
      for (int i = 0; i < Messages.Count; i++)
      {
        var response = ValidateMessage2(Messages[i], i, 0, 0, parsedInput.Rules);
        successCounter += response.Any(x => x.Success && x.Index == Messages[i].Length) ? 1 : 0;
        // if (i == 11)
        // {
        //   PrintTree(response.Path, "", true);
        // }
      }

      int partTwo = successCounter;

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\n" +
                        $"Part two result: {partTwo}\n" +
                        $"Execution time: {watch.ElapsedMilliseconds} ms\n" +
                        "");
      // $"Generated combinations: {generatedCombinations.Count}\n" +
      // $"Close to max: {lengths.Max(x => x) * 100 / Messages.Max(x => x.Length)}%");
    }

    private static List<ValidationResponse> ValidateMessage2(string message, int messageId, int messageIndex, int ruleNumber, Dictionary<int, Rule> rules)
    {
      if (rules[ruleNumber].IsCharacter)
      {
        return new List<ValidationResponse>
        {
            new ValidationResponse(
                messageIndex,
                true,
                messageIndex < message.Length && message.Substring(messageIndex, 1) == rules[ruleNumber].Character,
                new Path {Num = ruleNumber, Char = rules[ruleNumber].Character}
            )
        };
      }
      Path path = new Path{ Num = ruleNumber};
      if (rules[ruleNumber].SubRulesIds.SelectMany(x => x).ToList().Contains(ruleNumber))
      {
        DepthCounter[ruleNumber]++;
        Console.WriteLine($"Number {ruleNumber} going depth {DepthCounter[ruleNumber]}");
        if(DepthCounter[ruleNumber] > MAX_DEPTH) return new List<ValidationResponse>{ new ValidationResponse(messageIndex, false, false, null) };
      }

      Dictionary<int, int> groupIndexes = new Dictionary<int, int>();
      int myIndex = 0;
      for (int i = 0; i < rules[ruleNumber].SubRulesIds.Count; i++)
      {
        List<int> successIndexes = new List<int>();
        myIndex = messageIndex;
        int successRules = 0;
        List<int> subRulesIds = rules[ruleNumber].SubRulesIds[i];
        for (int subRuleIndex = 0; subRuleIndex < subRulesIds.Count; subRuleIndex++)
        {
          int ruleId = subRulesIds[subRuleIndex];
          var validation = ValidateMessage2(message, messageId, myIndex, ruleId, rules);
          List<ValidationResponse> successfulValidations = validation.Where(x => x.Success).ToList();
          if (successfulValidations.Count == 0)
          {
            break; //todo
          }
          else if (successfulValidations.Count == 1 && successfulValidations.First().CharacterFound)
          {
            successRules++;
            myIndex++;
          }
          else if (successfulValidations.Count >= 1)
          {
            List<int> newIndexes = successfulValidations.Select(x => x.Index).ToList();
            if (newIndexes.Count > 0)
            {
              successRules++;
              myIndex = newIndexes.Max();
              successIndexes = newIndexes;
            }
          }
        }

        if (myIndex != messageIndex && successRules == subRulesIds.Count)
        {
          groupIndexes.Add(i, myIndex);
        }
      }

      return groupIndexes.Values.Select(x => new ValidationResponse(x, false, true, path)).ToList();
    }

    private static List<ValidationResponse> ValidateMessage(string message, int messageId, int messageIndex, int ruleNumber, Dictionary<int, Rule> rules)
    {
      if (rules[ruleNumber].IsCharacter)
      {
        return new List<ValidationResponse>
        {
            new ValidationResponse(
                messageIndex,
                true,
                messageIndex < message.Length && message.Substring(messageIndex, 1) == rules[ruleNumber].Character,
                new Path {Num = ruleNumber, Char = rules[ruleNumber].Character}
            )
        };
      }
      
      Path path = new Path{ Num = ruleNumber};
      if (rules[ruleNumber].SubRulesIds.SelectMany(x => x).ToList().Contains(ruleNumber))
      {
        DepthCounter[ruleNumber]++;
        Console.WriteLine($"Number {ruleNumber} going depth {DepthCounter[ruleNumber]}");
        if(DepthCounter[ruleNumber] > MAX_DEPTH) return new List<ValidationResponse>{ new ValidationResponse(messageIndex, false, false, null) };
      }

      Dictionary<int, List<int>> subRuleIndexes = rules[ruleNumber].SubRulesIds.Select((x, i) => new {i}).ToDictionary(x => x.i, x => new List<int>());
      for (int subRulesIdsIndex = 0; subRulesIdsIndex < rules[ruleNumber].SubRulesIds.Count; subRulesIdsIndex++)
      {
        List<int> subRuleIds = rules[ruleNumber].SubRulesIds[subRulesIdsIndex];
        List<ValidationResponse> successValidations = new List<ValidationResponse>();
        List<Path> paths = new List<Path>();
        if (MessageSuccess[messageId].HasValue) break;
        int successes = 0;
        int index = messageIndex;
        string test = string.Empty;
        for (int i = 0; i < subRuleIds.Count; i++)
        {
          int ruleId = subRuleIds[i];
          var validation = ValidateMessage(message, messageId, index, ruleId, rules);
          List<ValidationResponse> successfulValidations = validation.Where(x => x.Success).ToList();
          successValidations.AddRange(successfulValidations);
          if (successfulValidations.Count == 0)
          {
            List<KeyValuePair<int, List<int>>> previousIndexes =
                subRuleIndexes.Where(x => x.Value.Any(y => y != 0) && x.Key < i).ToList();
            int? maxPreviousId = previousIndexes.Any() ? previousIndexes.Max(x => x.Key) : (int?) null;
            if (!maxPreviousId.HasValue) break;
            List<int> indexes = subRuleIndexes[maxPreviousId.Value].Where(x => x != 0).ToList();
            for (int j = 0; j < indexes.Count; j++)
            {
              i = indexes[j];
              indexes[j] = 0;
              break;
            }
          }
          else if (successfulValidations.Count == 1 && successfulValidations.First().CharacterFound)
          {
            paths.Add(validation.First().Path);
            successes++;
            index++;
          }
          else if (successfulValidations.Count >= 1)
          {
            List<int> newIndexes = successfulValidations.Select(x => x.Index).ToList();
            if (newIndexes.Count > 0)
            {
              successes++;
              index = newIndexes.First();
              subRuleIndexes[subRulesIdsIndex].AddRange(newIndexes);
            }
          }
        }
        // foreach (int ruleId in subRuleIds)
        // {
        //   var validation = ValidateMessage(message, messageId, index, ruleId, rules);
        //   if (!validation.Any(x => x.Success)) break;
        //   if (validation.Count == 1 && validation.First().CharacterFound)
        //   {
        //     paths.Add(validation.First().Path);
        //     index++;
        //   }
        //   successValidations.AddRange(validation.Where(x => x.Success).ToList());
        //   successes++;    
        //   // foreach (ValidationResponse validationResponse in validation.Where(x => x.Success))
        //   // {
        //   //   paths.Add(validation.Path);
        //   //   index = ;
        //   //   successes++;
        //   //   test += validation.CharacterFound ? rules[ruleId].Character : string.Empty;
        //   // }
        //   
        // }

        if (successes == subRuleIds.Count)
        {
          MessageResult[messageId] = test + MessageResult[messageId];
          MessagePath[messageId].AddRange(subRuleIds);
          if (index == message.Length)
          {
            MessageSuccess[messageId] = true;
          }

          path.Children.AddRange(paths);
          subRuleIndexes[subRulesIdsIndex].Add(index);
          // return new List<ValidationResponse> {new ValidationResponse(messageIndex, false, true, path)};
          // if (message.Substring(messageIndex, test.Length) == test)
          // {
          //   MessageResult[messageId] = test + MessageResult[messageId];  
          //   messageIndex = index;
          //   MessagePath[messageId].AddRange(subRuleIds);
          //   if (messageIndex == message.Length)
          //   {
          //     MessageSuccess[messageId] = true;
          //   }
          //
          //   path.Children.AddRange(paths);
          //   return (index, false, true, path);
          // }
          //
          // return (index, false, false, null);
        }
      }

      return subRuleIndexes.Values.Any(x => x.Any())
          ? subRuleIndexes.Values.Select(x => x.Any() ? x.Max() : -1).Distinct().Where(x => x != -1)
              .Select(x => new ValidationResponse(x, false, true, path)).ToList()
          : new List<ValidationResponse> {new ValidationResponse(messageIndex, false, false, path)};
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
    
    public static void PrintTree(Path tree, string indent, bool last)
    {
      if(tree is null) return;
      Console.WriteLine(indent + "+- " + tree.Num + " " + tree.Char);
      indent += last ? "   " : "|  ";

      for (int i = 0; i < tree.Children.Count; i++)
      {
        PrintTree(tree.Children[i], indent, i == tree.Children.Count - 1);
      }
    }
  }
}