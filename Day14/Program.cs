using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day14
{
  class Program
  {
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();
      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());

      long partOne = GetSum(lines, 1);
      long partTwo = GetSum(lines, 2);

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    static long GetSum(string[] lines, int part)
    {
      Dictionary<long, long> memory = new Dictionary<long, long>();
      string currentMask = string.Empty;
      lines.ToList().ForEach(line =>
      {
        string[] lineSplit = line.Split('=');
        if (line.StartsWith("mas")) currentMask = lineSplit[1].Trim();
        else
        {
          if (part == 1)
          {
            memory[long.Parse(lineSplit[0].SubstringBetween("[", "]"))] = 
                long.Parse(lineSplit[1])
                    .ToBinaryString(currentMask.Length)
                    .Select((item, index) => currentMask[index] != 'X' ? currentMask[index] : item)
                    .BinaryToNumber();  
          }
          else
          {
            char[] reverseBinary = long.Parse(lineSplit[0].SubstringBetween("[", "]"))
                .ToBinaryString(currentMask.Length)
                .Select((item, i) => currentMask[i] == '1' || currentMask[i] == 'X' ? currentMask[i] : item)
                .Reverse()
                .ToArray();
            
            List<int> floatingIndexes = reverseBinary.Select((item, index) => new {item, index}).Where(x => x.item == 'X').Select(x => x.index).ToList();

            for (long i = 0; i < Math.Pow(2, floatingIndexes.Count); i++)
            {
              string currentCombo = i.ToBinaryString(floatingIndexes.Count);
              for (int j = 0; j < currentCombo.Length; j++)
              {
                reverseBinary[floatingIndexes[j]] = currentCombo[j];
              }

              memory[new string(reverseBinary).ReverseString().BinaryToNumber()] = long.Parse(lineSplit[1]);
            }
          }
        }
      });
      return memory.Sum(x => x.Value);
    }
  }
}