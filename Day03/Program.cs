using System.IO;
using Common;

namespace Day03
{
  class Program
  {
    static void Main(string[] args)
    {
      string[] fileLines = File.ReadAllLines("day03.txt".ToApplicationPath());
      string[,] forestMatrix = new string [fileLines.Length, fileLines[0].Length];
      for (int i = 0; i < fileLines.Length; i++)
      {
        for (int j = 0; j < fileLines[i].Length; j++)
        {
          forestMatrix[i, j] = fileLines[i][j].ToString();
        }
      }

      int t11 = ChallengeOne.CountTrees(forestMatrix, 1, 1);
      int t31 = ChallengeOne.CountTrees(forestMatrix, 3, 1);
      int t51 = ChallengeOne.CountTrees(forestMatrix, 5, 1);
      int t71 = ChallengeOne.CountTrees(forestMatrix, 7, 1);
      int t12 = ChallengeOne.CountTrees(forestMatrix, 1, 2);

      int result = t11 * t31 * t51 * t71 * t12;
    }
  }
}