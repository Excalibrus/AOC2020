using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day22
{
  class Program
  {
    private static Queue<int> _playerOneCards;
    private static Queue<int> _playerTwoCards;
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      ParseData();
      int partOne = PartOne();
      int partTwo = 0;

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    static int PartOne()
    {
      int round = 1;
      while (_playerOneCards.Count > 0 && _playerTwoCards.Count > 0)
      {
        int cardOne = _playerOneCards.Dequeue();
        int cardTwo = _playerTwoCards.Dequeue();
        if (cardOne > cardTwo)
        {
          _playerOneCards.Enqueue(cardOne);
          _playerOneCards.Enqueue(cardTwo);
        }
        else
        {
          _playerTwoCards.Enqueue(cardTwo);
          _playerTwoCards.Enqueue(cardOne);
        }

        round++;
      }

      List<int> winningList = _playerOneCards.Count == 0 ? _playerTwoCards.ToList() : _playerOneCards.ToList();
      winningList.Reverse();
      int result = winningList.Select((num, index) => new {num, index}).Aggregate(0, (i, item) => i + (item.index + 1) * item.num);
      return result;
    }
    
    static void ParseData()
    {
      _playerOneCards = new Queue<int>();
      _playerTwoCards = new Queue<int>();
      bool playerOne = true;
      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());
      foreach (string line in lines)
      {
        if (line.Contains("Player 2"))
        {
          playerOne = false;
          continue;
        }
        if(string.IsNullOrWhiteSpace(line) || line.Contains("Player 1")) continue;
        if (playerOne)
        {
          _playerOneCards.Enqueue(int.Parse(line));
        }
        else
        {
          _playerTwoCards.Enqueue(int.Parse(line));
        } 
      }

    }
  }
}