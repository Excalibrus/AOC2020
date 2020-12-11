using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day11
{
  class Program
  {
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();
      string[] fileLines = File.ReadAllLines("input.txt".ToApplicationPath());

      Seat[,] seatMatrix = ParseSeatMatrix(fileLines);
      int partOne = CountSeats(seatMatrix, 1);
      seatMatrix = ParseSeatMatrix(fileLines);
      int partTwo = CountSeats(seatMatrix, 2);
      watch.Stop();
      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    private static Seat[,] ParseSeatMatrix(string[] fileLines)
    {
      Seat[,] matrix = new Seat[fileLines.Length, fileLines[0].Length];
      for (int x = 0; x < fileLines.Length; x++)
      {
        for (int y = 0; y < fileLines[x].Length; y++)
        {
          matrix[x, y] = new Seat
          {
              IsFloor = fileLines[x][y] == '.',
              IsOccupied = fileLines[x][y] == '#'
          };
        }
      }

      return matrix;
    }

    static int CountSeats(Seat[,] matrix, int part)
    {
      int maxConter = 100000;
      int counter = 0;
      do
      {
        bool applyRulesIteration = ApplyRulesIteration(matrix, part);
        // DrawMatrix(matrix);
        if (applyRulesIteration)
        {
          counter++;
        }
        else
        {
          break;
        }
      } while (counter < maxConter);

      return CountOccupiedSeats(matrix);
    }

    private static bool ApplyRulesIteration(Seat[,] matrix, int part)
    {
      bool changeInIteration = false;
      for (int x = 0; x < matrix.GetLength(0); x++)
      {
        for (int y = 0; y < matrix.GetLength(1); y++)
        {
          if (matrix[x, y].IsFloor)
          {
            continue;
          }

          List<Seat> adjacentSeats = GetAdjacentSeats(matrix, x, y, part);
          if (matrix[x, y].IsOccupied && adjacentSeats.Count(seat => !seat.IsFloor && seat.IsOccupied) >= (part == 1 ? 4 : 5))
          {
            matrix[x, y].IsOccupiedChangeState = false;
            changeInIteration = true;
          }
          else if (!matrix[x, y].IsOccupied && !adjacentSeats.Any(seat => seat.IsOccupied))
          {
            matrix[x, y].IsOccupiedChangeState = true;
            changeInIteration = true;
          }
        }
      }

      foreach (Seat seat in matrix)
      {
        if (seat.IsOccupiedChangeState.HasValue)
        {
          seat.IsOccupied = seat.IsOccupiedChangeState.Value;
        }
        
      }

      return changeInIteration;
    }

    private static List<Seat> GetAdjacentSeats(Seat[,] matrix, int x, int y, int part)
    {
      List<Seat> seats = new List<Seat>();
      // left
      for (int x2 = x - 1; x2 >= 0; x2--)
      {
        if(part == 1)
        {
          seats.Add(matrix[x2, y]);
          break;
        }
        if (matrix[x2, y].IsFloor) continue;
        seats.Add(matrix[x2, y]);
        break;
      }
      
      // right
      for (int x2 = x + 1; x2 < matrix.GetLength(0); x2++)
      {
        if(part == 1)
        {
          seats.Add(matrix[x2, y]);
          break;
        }
        if (matrix[x2, y].IsFloor) continue;
        seats.Add(matrix[x2, y]);
        break;
      }
      
      // up
      for (int y2 = y - 1; y2 >= 0; y2--)
      {
        if(part == 1)
        {
          seats.Add(matrix[x, y2]);
          break;
        }
        if (matrix[x, y2].IsFloor) continue;
        seats.Add(matrix[x, y2]);
        break;
      }
      
      // down
      for (int y2 = y + 1; y2 < matrix.GetLength(1); y2++)
      {
        if(part == 1)
        {
          seats.Add(matrix[x, y2]);
          break;
        }
        if (matrix[x, y2].IsFloor) continue;
        seats.Add(matrix[x, y2]);
        break;
      }
      
      // diagonal left up
      for (int x2 = x - 1, y2 = y - 1; x2 >= 0 && y2 >= 0; x2--, y2--)
      {
        if(part == 1)
        {
          seats.Add(matrix[x2, y2]);
          break;
        }
        if (matrix[x2, y2].IsFloor) continue;
        seats.Add(matrix[x2, y2]);
        break;
      }
      
      // diagonal left down
      for (int x2 = x - 1, y2 = y + 1; x2 >= 0 && y2 < matrix.GetLength(1); x2--, y2++)
      {
        if(part == 1)
        {
          seats.Add(matrix[x2, y2]);
          break;
        }
        if (matrix[x2, y2].IsFloor) continue;
        seats.Add(matrix[x2, y2]);
        break;
      }
      
      // diagonal right up
      for (int x2 = x + 1, y2 = y - 1; x2 < matrix.GetLength(0) && y2 >= 0; x2++, y2--)
      {
        if(part == 1)
        {
          seats.Add(matrix[x2, y2]);
          break;
        }
        if (matrix[x2, y2].IsFloor) continue;
        seats.Add(matrix[x2, y2]);
        break;
      }
      
      // diagonal right down
      for (int x2 = x + 1, y2 = y + 1; x2 < matrix.GetLength(0) && y2 < matrix.GetLength(1); x2++, y2++)
      {
        if(part == 1)
        {
          seats.Add(matrix[x2, y2]);
          break;
        }
        if (matrix[x2, y2].IsFloor) continue;
        seats.Add(matrix[x2, y2]);
        break;
      }
      
      return seats;
    }

    private static void DrawMatrix(Seat[,] matrix)
    {
      for (int x = 0; x < matrix.GetLength(0); x++)
      {
        for (int y = 0; y < matrix.GetLength(1); y++)
        {
          if (matrix[x, y].IsFloor)
          {
            Console.Write(".");
          }
          else if (matrix[x, y].IsOccupied)
          {
            Console.Write("#");
          }
          else
          {
            Console.Write("L");
          }

          if (y == matrix.GetLength(1) - 1)
          {
            Console.Write("\n");
          }
        }

        if (x == matrix.GetLength(0) - 1)
        {
          Console.Write("\n\n");
        }
      }
    }

    private static int CountOccupiedSeats(Seat[,] matrix)
    {
      int count = 0;
      for (int x = 0; x < matrix.GetLength(0); x++)
      {
        for (int y = 0; y < matrix.GetLength(1); y++)
        {
          if (matrix[x, y].IsOccupied && !matrix[x, y].IsFloor)
          {
            count++;
          }
        }
      }

      return count;
    }
  }
}