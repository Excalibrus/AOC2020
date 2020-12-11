using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace Day09
{
  class Program
  {
    static void Main(string[] args)
    {
      string[] fileLines = File.ReadAllLines("day09.txt".ToApplicationPath());
      List<long> numbers = fileLines.Select(x => long.Parse(x)).ToList();

      long numberPartOne = GetNumberPartOne(numbers, 25);
      long numberPartTwo = GetNumberPartTwo(numbers, numberPartOne);
    }

    static long GetNumberPartOne(List<long> numbers, int preamble)
    {
      for (int i = preamble; i < numbers.Count; i++)
      {
        long currentNumber = numbers[i];
        List<long> preambleList = numbers.GetRange(i - preamble, preamble);
        bool isThereValidSumInPreambleList = false;
        foreach (int preambleNumber in preambleList)
        {
          if (preambleList.Contains(currentNumber - preambleNumber))
          {
            isThereValidSumInPreambleList = true;
            break;
          }
        }

        if (!isThereValidSumInPreambleList)
        {
          return currentNumber;
        }
      }

      return -1;
    }

    static long GetNumberPartTwo(List<long> numbers, long targetNumber)
    {
      int indexOfTargetNumber = numbers.IndexOf(targetNumber);
      for (int i = 0; i < indexOfTargetNumber; i++)
      {
        for (int j = 0; j < i; j++)
        {
          List<long> subList = numbers.GetRange(j, i - j).ToList();
          if (subList.Sum() == targetNumber && subList.Count > 1)
          {
            return subList.Min() + subList.Max();
          }
        }
        
      }

      return -1;
    }
  }
}