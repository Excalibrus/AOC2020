using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day16
{
  class Program
  {
    private static List<TicketRule> _rules = new List<TicketRule>();
    private static List<Ticket> _tickets = new List<Ticket>();
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      ParseData();
      long partOne = PartOne();
      long partTwo = PartTwo();

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    static void ParseData()
    {
      bool parseRules = true;
      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());
      foreach (string line in lines)
      {
        if(string.IsNullOrWhiteSpace(line)) continue;
        if (line.Contains("your ticket") || line.Contains("nearby tickets"))
        {
          parseRules = false;
          continue;
        }
        if (parseRules)
        {
          _rules.Add(new TicketRule(line));
        }
        else
        {
          _tickets.Add(new Ticket(line));
        }
      }
    }

    static long PartOne()
    {
      List<long> invalidTickets = new List<long>();
      foreach (Ticket ticket in _tickets.Skip(1))
      {
        invalidTickets.AddRange(ticket.Numbers.Where(num => !_rules.Any(rule => rule.IsValid(num))).ToList());
      }

      return invalidTickets.Sum();
    }

    static long PartTwo()
    {
      List<Ticket> validTickets = new List<Ticket>();
      foreach (Ticket ticket in _tickets)
      {
        if (ticket.Numbers.All(num => _rules.Any(rule => rule.IsValid(num))))
        {
          // for (int i = 0; i < ticket.Numbers.Count; i++)
          // {
          //   List<string> rules = _rules.Where(x => ticket.Numbers.All(x.IsValid)).Select(x => x.Name).ToList();
          //   ticket.MatchingRules.Add(i, rules);
          // }

          validTickets.Add(ticket);
        }
      }

      Dictionary<int, List<string>> possibleWords = new Dictionary<int, List<string>>();
      for (int i = 0; i < validTickets.First().Numbers.Count; i++)
      {
        List<long> indexNumbers = validTickets.Select(x => x.Numbers[i]).ToList();
        possibleWords.Add(
            i, 
            _rules.Where(rule => indexNumbers.All(rule.IsValid)).Select(x => x.Name).ToList()
            );
      }

      while (possibleWords.Any(x => x.Value.Count > 1))
      {
        Dictionary<int,string> singles = possibleWords.Where(x => x.Value.Count == 1).ToDictionary(x => x.Key, x => x.Value.First());
        for (int i = 0; i < possibleWords.Count; i++)
        {
          if (!singles.ContainsKey(i) && possibleWords[i].Any(x => singles.Values.Contains(x)))
          {
            possibleWords[i] = possibleWords[i].Except(singles.Values.ToList()).ToList();
          }
        }
      }
      
      // List<string> matchedWords = new List<string>();
      //
      // for (int i = 0; i < validTickets.First().Numbers.Count; i++)
      // {
      //   Ticket aggregatedTicket = validTickets.Skip(1).Aggregate(
      //       validTickets.First(), 
      //       (t1, t2) => new Ticket
      //       {
      //           MatchingRules = new Dictionary<int, List<string>>
      //           {
      //               {i, t1.MatchingRules[i].Intersect(t2.MatchingRules[i]).ToList()}
      //           }
      //       });
      //   matchedWords.AddRange(aggregatedTicket.MatchingRules[i]);
      // }

      long result = validTickets.First().Numbers
          .Where((item, i) =>
          {
            return possibleWords[i].First().StartsWith("departure");
          })
          .Aggregate((num1, num2) => num1 * num2);
      return result;
    }
  }
}