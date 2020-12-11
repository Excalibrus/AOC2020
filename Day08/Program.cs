using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day08
{
  class Program
  {
    static void Main(string[] args)
    {
      int tries = 1000;

      List<double> executionTimes = new List<double>();
      for (int runNumber = 0; runNumber < tries; runNumber++)
      {
        Stopwatch watch = new System.Diagnostics.Stopwatch();
            
        watch.Start();
        string[] fileLines = File.ReadAllLines("day08.txt".ToApplicationPath());

        List<CommandLine> commandLines = ParseCommandLines(fileLines);

        int accumulatorPartOne = GetAccumulatorPartOne(commandLines).Accumulator;
        int accumulatorPartTwo = GetAccumulatorPartTwo(commandLines);
      
        watch.Stop();
        
        executionTimes.Add(watch.Elapsed.TotalMilliseconds * 1000000);
      }
      
      
      // Console.WriteLine($"Result of part one is: {accumulatorPartOne}");
      // Console.WriteLine($"Result of part two is: {accumulatorPartTwo}");
      //
      // Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
      var average = executionTimes.Sum() / (double)tries;
      Console.WriteLine($"C#: Average execution time of {tries} executions: {average} ns");
    }

    static List<CommandLine> ParseCommandLines(string[] fileLines)
    {
      int id = 0;
      List<CommandLine> commandLines = new List<CommandLine>();
      foreach (string fileLine in fileLines)
      {
        string[] lineParts = fileLine.Split(' ');
        if (int.TryParse(lineParts[1], out int number))
        {
          CommandLine commandLine = new CommandLine
          {
              Id = ++id,
              Type = lineParts[0].Equals("acc", StringComparison.InvariantCultureIgnoreCase) ? CommandType.Accumulator : (lineParts[0].Equals("jmp", StringComparison.InvariantCultureIgnoreCase) ? CommandType.Jump : CommandType.NoOperation),
              Number = number,
              Visited = false
          };  
          commandLines.Add(commandLine);
        }
      }

      return commandLines;
    }

    static (bool Success, int Accumulator) GetAccumulatorPartOne(List<CommandLine> commandLines)
    {
      int accumulator = 0;
      int tries = 0;
      for (int i = 0; i < commandLines.Count; i++)
      {
        tries++;
        CommandLine commandLine = commandLines[i];
        if (commandLine.Visited)
        {
          return (Success: false, Accumulator: accumulator);
        }

        commandLine.Visited = true;
        if (commandLine.Type == CommandType.NoOperation)
        {
          continue;
        }

        if (commandLine.Type == CommandType.Accumulator)
        {
          accumulator += commandLine.Number;
        }
        else if (commandLine.Type == CommandType.Jump)
        {
          i += commandLine.Number - 1;
        }
      }

      return (Success: true, Accumulator: accumulator);
    }

    static int GetAccumulatorPartTwo(List<CommandLine> commandLines)
    {
      int triesCounter = 0;
      
      while (triesCounter < 100000)
      {
        triesCounter++;
        commandLines.ForEach(x => x.Visited = false);
        if (!TryChange(commandLines))
        {
          break;
        }
        var result = GetAccumulatorPartOne(commandLines);
        if (result.Success)
        {
          return result.Accumulator;
        }
        RevertLastChanged(commandLines);
      }

      return 0;
    }

    static bool TryChange(List<CommandLine> commandLines)
    {
      CommandLine commandLine = commandLines.OrderBy(x => x.Id).FirstOrDefault(x => !x.Changed && x.Type != CommandType.Accumulator);

      if (commandLine != null)
      {
        commandLine.Type = commandLine.Type == CommandType.Jump ? CommandType.NoOperation : CommandType.Jump;
        commandLine.Changed = true;
        return true;
      }

      return false;
    }

    static void RevertLastChanged(List<CommandLine> commandLines)
    {
      CommandLine commandLine = commandLines.OrderByDescending(x => x.Id).FirstOrDefault(x => x.Changed && x.Type != CommandType.Accumulator);

      if (commandLine != null)
      {
        commandLine.Type = commandLine.Type == CommandType.Jump ? CommandType.NoOperation : CommandType.Jump;
      }
    }
  }
}