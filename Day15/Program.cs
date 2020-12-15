using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day15
{
  class Program
  {
    private static Dictionary<int, List<int>> _memory;
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      long partOne = GetMaxNumberSpoken(2020);
      long partTwo = GetMaxNumberSpoken(30000000);

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    private static int GetMaxNumberSpoken(int max)
    {
      SetInitInput(0,3,1,6,7,5);

      int lastSpoken = _memory.Last().Key;
      for (int i = _memory.Count; i < max; i++)
      {
        lastSpoken =
            _memory[lastSpoken].Count > 1
                ? _memory[lastSpoken].Last() - _memory[lastSpoken][_memory[lastSpoken].Count - 2]
                : 0;
        AddToMemory(lastSpoken, i+1);
      }

      return lastSpoken;
    }

    private static void AddToMemory(int num, int step)
    {
      if (!_memory.ContainsKey(num))
      {
        _memory.Add(num, new List<int>());
      }
      _memory[num].Add(step);
    }

    private static void SetInitInput(params int[] numbers)
    {
      _memory = new Dictionary<int, List<int>>();
      for (int i = 0; i < numbers.Length; i++)
      {
        _memory.Add(numbers[i], new List<int> {i + 1});
      }
    }
  }
}