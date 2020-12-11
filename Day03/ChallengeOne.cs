namespace Day03
{
  public static class ChallengeOne
  {
    public static int CountTrees(string[,] forestMatrix, int rightMoves, int downMoves)
    {
      int numberOfTrees = 0;
      int right = 0;
      int down = 0;
      while (down < forestMatrix.GetLength(0))
      {
        if (forestMatrix[down, right] == "#")
        {
          numberOfTrees++;
        }

        right += rightMoves;
        down += downMoves;
        if (right >= forestMatrix.GetLength(1))
        {
          right -= forestMatrix.GetLength(1);
        }
      }

      return numberOfTrees;
    }
  }
}