using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace Day05
{
  class Program
  {
    static void Main(string[] args)
    {
      SortedSet<int> seatIds = new SortedSet<int>();
      string[] fileLines = File.ReadAllLines("day05.txt".ToApplicationPath());
      foreach (string line in fileLines)
      {
        int minRow = 0;
        int maxRow = 127;
        int minCol = 0;
        int maxCol = 7;
        for (int i = 0; i < line.Length; i++)
        {
          char character = line[i];
          if (i < 7)
          {
            var newRange = GetNewRange(minRow, maxRow, character == 'B');
            minRow = newRange.NewMin;
            maxRow = newRange.NewMax;
          }
          else
          {
            var newRange = GetNewRange(minCol, maxCol, character == 'R');
            minCol = newRange.NewMin;
            maxCol = newRange.NewMax;
          }
        }

        int seatId = minRow * 8 + minCol;
        seatIds.Add(seatId);
      }

      int maxSeatId = seatIds.Max();
      for (int i = seatIds.Min; i < maxSeatId; i++)
      {
        if (!seatIds.Contains(i))
        {
          int mySeatId = i;
        }
      }
      
    }

    static (int NewMin, int NewMax) GetNewRange(int min, int max, bool takeUpper)
    {
      int i = (int)Math.Floor((decimal)(min + max) / 2);
      return takeUpper ? (NewMin: i + 1, NewMax: max) : (NewMin: min, NewMax: i);
    }
  }
}