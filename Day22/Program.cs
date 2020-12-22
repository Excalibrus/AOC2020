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
      int partOne = PartOne(new Queue<int>(_playerOneCards), new Queue<int>(_playerTwoCards));
      int partTwo = PartTwo(new Queue<int>(_playerOneCards), new Queue<int>(_playerTwoCards), 1).sum;

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    static int PartOne(Queue<int> playerOneCards, Queue<int> playerTwoCards)
    {
      while (playerOneCards.Count > 0 && playerTwoCards.Count > 0)
      {
        int cardOne = playerOneCards.Dequeue();
        int cardTwo = playerTwoCards.Dequeue();
        if (cardOne > cardTwo)
        {
          playerOneCards.Enqueue(cardOne);
          playerOneCards.Enqueue(cardTwo);
        }
        else
        {
          playerTwoCards.Enqueue(cardTwo);
          playerTwoCards.Enqueue(cardOne);
        }
      }

      List<int> winningList = playerOneCards.Count == 0 ? playerTwoCards.ToList() : playerOneCards.ToList();
      winningList.Reverse();
      int result = winningList.Select((num, index) => new {num, index}).Aggregate(0, (i, item) => i + (item.index + 1) * item.num);
      return result;
    }

    static (int sum, bool p1win) PartTwo(Queue<int> playerOneCards, Queue<int> playerTwoCards, int game)
    {
      List<string> previousRoundCardsP1 = new List<string>();
      List<string> previousRoundCardsP2 = new List<string>();
      // Console.WriteLine($"Starting game {game}.");
      while (playerOneCards.Count > 0 && playerTwoCards.Count > 0)
      {
        string p1Hash = string.Join("", playerOneCards);
        string p2Hash = string.Join("", playerTwoCards);
        if (previousRoundCardsP1.Contains(p1Hash) || previousRoundCardsP2.Contains(p2Hash)) return (0, true); 
        previousRoundCardsP1.Add(p1Hash);
        previousRoundCardsP2.Add(p2Hash);
        int p1Card = playerOneCards.Dequeue();
        int p2Card = playerTwoCards.Dequeue();
        if (playerOneCards.Count >= p1Card && playerTwoCards.Count >= p2Card)
        {
          (int sum, bool p1win) res = PartTwo(
              new Queue<int>(playerOneCards.Take(p1Card)), 
              new Queue<int>(playerTwoCards.Take(p2Card)), game + 1);
          if (res.p1win)
          {
            playerOneCards.Enqueue(p1Card);
            playerOneCards.Enqueue(p2Card);
          }
          else
          {
            playerTwoCards.Enqueue(p2Card);
            playerTwoCards.Enqueue(p1Card);
          }
        }
        else
        {
          if (p1Card > p2Card)
          {
            playerOneCards.Enqueue(p1Card);
            playerOneCards.Enqueue(p2Card);
          }
          else
          {
            playerTwoCards.Enqueue(p2Card);
            playerTwoCards.Enqueue(p1Card);
          }
        }
      }

      bool p1Win = playerTwoCards.Count == 0;
      // Console.WriteLine($"Player {(p1Win ? 1 : 2)} is winner of game {game}.");
      if (game > 1) return (0, p1Win);
      List<int> winningList = p1Win ? playerOneCards.ToList() : playerTwoCards.ToList();
      winningList.Reverse();
      int result = winningList.Select((num, index) => new {num, index}).Aggregate(0, (i, item) => i + (item.index + 1) * item.num);
      return (result, p1Win);
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