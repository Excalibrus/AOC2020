using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day12
{
  class Program
  {
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      List<Instruction> instructions = GetInstructions();

      Ship ship = new Ship { Direction = Direction.East};
      instructions.ForEach(instruction => ship.Move(instruction));
      
      Ship ship2 = new Ship { WaypointNorth = 1, WaypointEast = 10};
      instructions.ForEach(instruction => ship2.MovePartTwo(instruction));
      
      watch.Stop();

      Console.WriteLine($"Part one result: {ship.GetManhattanDistance()}\nPart two result: {ship2.GetManhattanDistance()}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    private static List<Instruction> GetInstructions()
    {
      List<Instruction> instructions = 
          File.ReadAllLines("input.txt".ToApplicationPath())
              .Select(inp => 
                  new Instruction{ Letter = inp.Substring(0, 1), Number = int.Parse(inp.Substring(1, inp.Length - 1))})
              .ToList();
      return instructions;
    }
  }
}