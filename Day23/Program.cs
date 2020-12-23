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

      CircleList<int> partOneNumbers = new CircleList<int>(numbers);
      string partOne = DoAlgorithm(partOneNumbers,  100, 1);

      for (int i = numbers.Max() + 1; i <= 1000000; i++)
      {
        numbers.Add(i);
      }

      CircleList<int> milNumbers = new CircleList<int>(numbers);
      string partTwo = DoAlgorithm(milNumbers,  10000000, 2);

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    static string DoAlgorithm(CircleList<int> startingNumbers, int moves, int part)
    {
      Dictionary<LinkedListNode<int>, List<LinkedListNode<int>>> destinationCache = new Dictionary<LinkedListNode<int>, List<LinkedListNode<int>>>();

      LinkedListNode<int> currentNode = startingNumbers.First;
      List<LinkedListNode<int>> maxNodes = new List<LinkedListNode<int>>();
      List<int> max5 = startingNumbers.ToList().Where(x => x > currentNode.Value).OrderByDescending(x => x).Take(5).ToList();
      foreach (int i in max5)
      {
        maxNodes.Add(startingNumbers.Find(i));
      }
      destinationCache.Add(currentNode, new List<LinkedListNode<int>>());
      List<int> smaller5tmp = startingNumbers.ToList().Where(x => x < currentNode.Value).OrderByDescending(x => x).Take(5).ToList();
      foreach (int i in smaller5tmp)
      {
        destinationCache[currentNode].Add(startingNumbers.Find(i));
      }
      
      LinkedListNode<int> nextNode = currentNode.NextOrFirst();
      while (nextNode.Value != currentNode.Value)
      {
        if (nextNode.Value < 15)
        {
          destinationCache.Add(nextNode, new List<LinkedListNode<int>>());
          List<int> smaller5 = startingNumbers.ToList().Where(x => x < nextNode.Value).OrderByDescending(x => x).Take(5).ToList();
          foreach (int i in smaller5)
          {
            destinationCache[nextNode].Add(startingNumbers.Find(i));
          }
        }
        else
        {
          destinationCache.Add(nextNode, nextNode.PreviousOfNumber(5).ToList());  
        }
        
        nextNode = nextNode.NextOrFirst();
      }
      
      for (int i = 1; i <= moves; i++)
      {
        // if (i % 100000 == 0)
        // {
        //   Console.WriteLine("Move " + i);
        // }
        // WriteOutput($"-- Move {i} --");
        // WriteOutput($"Cups: {string.Join(" ", startingNumbers.ToList().Select((x, id) => id == i - 1 ? "(" + x + ")" : x.ToString()).ToList())}");
        
        List<LinkedListNode<int>> numbers = startingNumbers.RemoveNextThree(currentNode).ToList();
        // WriteOutput($"Pick up: {numbers[0].Value}, {numbers[1].Value}, {numbers[2].Value}");

        LinkedListNode<int> destination = GetDestination(destinationCache, maxNodes, currentNode, numbers[0].Value, numbers[1].Value, numbers[2].Value);
        // WriteOutput($"Destination: {destination.Value}");
        
        startingNumbers.InsertAfterNode(destination, numbers[0], numbers[1], numbers[2]);
        currentNode = currentNode.NextOrFirst();
        // WriteOutput();
      }

      // WriteOutput("-- Final --");
      // WriteOutput($"Cups: {string.Join(" ", startingNumbers.ToList())}");
      // WriteOutput();
      string answer = string.Empty;
      LinkedListNode<int> firstValue = startingNumbers.Find(1);
      if (firstValue != null)
      {
        if (part == 1)
        {
          LinkedListNode<int> nextValue = firstValue.NextOrFirst();
          int cachedValue = nextValue.Value;
          answer += cachedValue;
          nextValue = nextValue.NextOrFirst();
          
          while (nextValue.Value != cachedValue)
          {
            if (nextValue.Value != 1)
            {
              answer += nextValue.Value;
            }
            nextValue = nextValue.NextOrFirst();
          }
        }
        else
        {
          List<LinkedListNode<int>> nextValues = firstValue.NextOfNumber(3).ToList();
          answer = ((long)nextValues[0].Value * (long)nextValues[1].Value).ToString();
        }
      }

      return answer;
    }

    private static LinkedListNode<int> GetDestination(Dictionary<LinkedListNode<int>, List<LinkedListNode<int>>> cache, List<LinkedListNode<int>> maxNumbers, LinkedListNode<int> currentValue, params int[] notIncludedValues)
    {
      var destination = cache[currentValue].FirstOrDefault(x => !notIncludedValues.Contains(x.Value) && x.Value != currentValue.Value);
      if (destination == null)
      {
        destination = maxNumbers.FirstOrDefault(x => !notIncludedValues.Contains(x.Value) && x.Value != currentValue.Value);
      }

      return destination;
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