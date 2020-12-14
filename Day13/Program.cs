using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime;
using Common;

namespace Day13
{
  class Program
  {
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();
      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());
      int partOne = GetPartOneResult(lines);
      BigInteger partTwo = GetPartTwoResult2(lines[1]);

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    private static int GetPartOneResult(string[] lines)
    {
      int.TryParse(lines[0], out int timestamp);
      List<int> busIds = lines[1].Split(',').Where(x => x != "x").Select(int.Parse).OrderBy(x => x).ToList();
      for (int i = timestamp; i < timestamp + 100000; i++)
      {
        int busId = busIds.FirstOrDefault(x => i % x == 0);
        if (busId > 0)
        {
          return busId * (i - timestamp);
        }
      }

      return 0;
    }

    private static long GetPartTwoResult(string line)
    {
      Dictionary<int,int> buses = line.Split(',').Select((x, i) => new {x, i}).Where(x => x.x != "x").ToDictionary(x => int.Parse(x.x), x => x.i + 1);
      long timestamp = 0;
      while (true)
      {
        if (buses.Take(2).All(bus => (timestamp + bus.Value - 1) % bus.Key == 0))
        {
          return timestamp + buses.Keys.Take(2).First();
        }
        timestamp += buses.Keys.First();
      }
    }
    
    private static BigInteger GetPartTwoResult2(string line)
    {
      Dictionary<int,int> buses = line.Split(',').Select((x, i) => new {x, i}).Where(x => x.x != "x").ToDictionary(x => int.Parse(x.x), x => x.i + 1);
      int take = 1;
      BigInteger timestamp = 0;
      BigInteger aggregate = buses.Keys.First();
      while (take <= buses.Count)
      {
        timestamp += aggregate;
        if (buses.Take(take).Any(bus => (timestamp + bus.Value - 1) % bus.Key != 0)) continue;
        if (buses.Count == take)
        {
          return timestamp;
        }
        take++;
        aggregate = buses.Keys.Take(take - 1).Multiply();
      }

      return -1;
    }
  }
}