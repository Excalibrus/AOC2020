using System;
using System.IO;
using Common;

namespace Day02
{
  class Program
  {
    static void Main(string[] args)
    {
      int correctPasswords = 0;
      var fileLines = File.ReadAllLines("day02.txt".ToApplicationPath());
      foreach (string fileLine in fileLines)
      {
        var line = ParseLine(fileLine);
        // Part 1
        // int occurences = GetNumberOfCharOccurences(line.Password, line.FindingChar);
        // if (occurences >= line.Minimum && occurences <= line.Maximum)
        // {
        //   correctPasswords++;
        // }
        // Part 2
        if (IsPasswordCorrect(line))
        {
          correctPasswords++;
        }
      }
    }

    static PasswordLine ParseLine(string line)
    {
      PasswordLine passwordLine = new PasswordLine();
      var parts = line.Split(':');
      passwordLine.Password = parts[1];
      passwordLine.FindingChar = Convert.ToChar(parts[0].Substring(parts[0].Length - 1, 1));

      string numbersString = parts[0].Substring(0, parts[0].Length - 2).Trim();
      string[] numberParts = numbersString.Split('-');
      passwordLine.Minimum = int.Parse(numberParts[0]);
      passwordLine.Maximum = int.Parse(numberParts[1]);

      return passwordLine;
    }

    static bool IsPasswordCorrect(PasswordLine line)
    {
      return (line.Password[line.Minimum] == line.FindingChar && line.Password[line.Maximum] != line.FindingChar) ||
             (line.Password[line.Minimum] != line.FindingChar && line.Password[line.Maximum] == line.FindingChar);
    }

    static int GetNumberOfCharOccurences(string line, char findingChar)
    {
      int occurences = 0;
      foreach (char character in line)
      {
        if (character == findingChar)
        {
          occurences++;
        }
      }

      return occurences;
    }
  }
}