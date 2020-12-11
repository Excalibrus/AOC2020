using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace Day10
{
  class Program
  {
    static void Main(string[] args)
    {
      string[] fileLines = File.ReadAllLines("input.txt".ToApplicationPath());
      List<int> numbers = fileLines.Select(x => int.Parse(x)).Where(x => x > 0).OrderBy(x => x).ToList();

      numbers.Add(0);
      numbers.Add(numbers.Max() + 3);

      numbers = numbers.OrderBy(x => x).ToList();
      
      int partOne = GetResultPartOne(numbers);
      long partTwo = GetResultPartTwo(numbers);
    }

    static int GetResultPartOne(List<int> numbers)
    {
      int numberOfOnes = 0;
      int numberOfThrees = 0;
      for (int i = 1; i < numbers.Count; i++)
      {
        int number = numbers[i];
        int prevNumber = numbers[i-1];
        if (number - prevNumber == 1)
        {
          numberOfOnes++;
        }
        else if (number - prevNumber == 3)
        {
          numberOfThrees++;
        }
      }


      return numberOfOnes * numberOfThrees;
    }

    static long GetResultPartTwo(List<int> numbers)
    {
      Dictionary<int, long> combinations = new Dictionary<int, long>
      {
          { 0, 1 } // first node
      };
      for (int i = 1; i <= numbers.Max(); i++)
      {
        if (numbers.Contains(i) || i == numbers.Max())
        {
          long num = 0;
          if (combinations.ContainsKey(i-3))
          {
            num += combinations[i - 3];
          }
          if (combinations.ContainsKey(i-2))
          {
            num += combinations[i - 2];
          }
          if (combinations.ContainsKey(i-1))
          {
            num += combinations[i - 1];
          }
          combinations.Add(i, num);
        }
      }

      return combinations[numbers.Max()];

    }

    // static long GetResultPartTwo(List<int> numbers)
    // {
    //   int combinationCounter = 0;
    //   for (int i = 0; i < 3; i++)
    //   {
    //     bool finishedWithCount = false;
    //     while (!finishedWithCount)
    //     {
    //       int index = GetIndexToRemove(numbers);
    //       if (index == -1)
    //       {
    //         break;
    //       }
    //       numbers.RemoveAt(index);
    //       combinationCounter++;
    //     }
    //   }
    //   
    //
    //   long finalNumber = 0;
    //   for (int i = 1; i <= combinationCounter; i++)
    //   {
    //     finalNumber += (long)(combinationCounter.Factorial() / (combinationCounter - i).Factorial());
    //   }
    //   
    //   return finalNumber + 1;
    // }
    //
    // static int GetIndexToRemove(List<int> numbers)
    // {
    //   for (int i = 1; i < numbers.Count - 1; i++)
    //   {
    //     if (numbers[i + 1] - numbers[i - 1] <= 3)
    //     {
    //       return i;
    //     }
    //   }
    //
    //   return -1;
    // }
    
    // static long GetResultPartTwo(List<int> numbers)
    // {
    //   int combinationCounter = 0;
    //   // for (int i = 1; i < numbers.Count - 1; i++)
    //   // {
    //   //   if (numbers[i + 1] - numbers[i - 1] <= 3)
    //   //   {
    //   //     combinationCounter++;
    //   //   }
    //   // }
    //
    //   List<int> diffs = new List<int>();
    //   for (int i = 1; i <= numbers.Count - 1; i++)
    //   {
    //     diffs.Add(numbers[i] - numbers[i-1]);
    //   }
    //
    //   combinationCounter = diffs.Count(x => x < 3 && x > 0);
    //
    //   long finalNumber = 0;
    //   for (int i = 1; i <= combinationCounter; i++)
    //   {
    //     finalNumber += (long)(combinationCounter.Factorial() / (i.Factorial() * (combinationCounter - i).Factorial()));
    //   }
    //
    //   return finalNumber + 1;
    // }
  }
}