using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day18
{
  class Program
  {
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());
      
      long partOne = Process(lines, 1);
      long partTwo = Process(lines, 2);

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    private static long Process(string[] lines, int part)
    {
      long sum = 0;
      foreach (string line in lines)
      {
        List<char> postfix = Library.ConvertInfixToPostFix(line.ToCharArray().Where(x => x != ' ').ToList(), part).ToList();
        sum += Library.CalculatePostfix(postfix);
      }

      return sum;
    }
  }
}