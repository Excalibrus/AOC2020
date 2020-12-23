using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day23
{
  class Program
  {
    private static bool OUTPUT_ENABLED = false;
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      List<int> numbers = File.ReadAllText("input.txt".ToApplicationPath()).Select(x => int.Parse(x.ToString())).ToList();

      string partOne = PartOne(new List<int>(numbers), numbers.First(), 100);
      int partTwo = 0;

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    static string PartOne(List<int> startingNumbers, int current, int moves)
    {
      for (int i = 1; i <= moves; i++)
      {
        WriteOutput($"-- Move {i} --");
        int currentIndex = startingNumbers.IndexOf(current);
        WriteOutput($"Cups: {string.Join(" ", startingNumbers.Select((x, id) => id == i ? "(" + x + ")" : x.ToString()).ToList())}");
        int take1Index = currentIndex + 1 < startingNumbers.Count
            ? currentIndex + 1
            : startingNumbers.Count - currentIndex - 1; 
        int take2Index = currentIndex + 2 < startingNumbers.Count
            ? currentIndex + 2
            : currentIndex + 2 - startingNumbers.Count;
        int take3Index = currentIndex + 3 < startingNumbers.Count
            ? currentIndex + 3
            : currentIndex + 3 - startingNumbers.Count;

        int num1 = startingNumbers[take1Index];
        int num2 = startingNumbers[take2Index];
        int num3 = startingNumbers[take3Index];
        
        WriteOutput($"Pick up: {num1}, {num2}, {num3}");
        
        startingNumbers.Remove(num1);
        startingNumbers.Remove(num2);
        startingNumbers.Remove(num3);

        int destination = startingNumbers.Where(x => x != current).OrderByDescending(x => x)
            .FirstOrDefault(x => x < current);
        if (destination == 0)
        {
          destination = startingNumbers.Where(x => x != current).Max();
        }
        WriteOutput($"Destination: {destination}");

        int insertIndex = startingNumbers.IndexOf(destination) + 1;
        if (insertIndex == startingNumbers.Count) insertIndex = 0;
        startingNumbers.InsertRange(insertIndex, new []{ num1, num2, num3 });
        int newCurrentIndex = startingNumbers.IndexOf(current) + 1;
        if (newCurrentIndex == startingNumbers.Count) newCurrentIndex = 0;
        current = startingNumbers[newCurrentIndex];
        WriteOutput();
      }

      WriteOutput("-- Final --");
      WriteOutput($"Cups: {string.Join(" ", startingNumbers)}");
      WriteOutput();
      string answer = string.Empty;
      int takeIndex = startingNumbers.IndexOf(1);
      startingNumbers.Remove(1);
      while (startingNumbers.Count > 0)
      {
        if (takeIndex == startingNumbers.Count) takeIndex = 0;
        answer += startingNumbers[takeIndex].ToString();
        startingNumbers.RemoveAt(takeIndex);
      }

      return answer;
    }

    private static void WriteOutput(string text = "")
    {
      if (OUTPUT_ENABLED)
      {
        Console.WriteLine(text);
      } 
    }
  }
}