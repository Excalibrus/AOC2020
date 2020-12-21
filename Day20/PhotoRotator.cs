namespace Day20
{
  public static class PhotoRotator
  {
    public static char[,] RotatePhotoCounterClockwise(char[,] photo)
    {
      int rows = photo.GetLength(0);
      int cols = photo.GetLength(1);
      char[,] newPhoto = new char[rows, cols];
      for (int x = 0; x < rows / 2; x++) { 
        for (int y = x; y < cols - x - 1; y++) { 
          newPhoto[x, y] = photo[y, rows - 1 - x]; 
          newPhoto[y, rows - 1 - x] = photo[rows - 1 - x, rows - 1 - y]; 
          newPhoto[rows - 1 - x, rows - 1 - y] = photo[rows - 1 - y, x]; 
          newPhoto[rows - 1 - y, x] = photo[x,y]; 
        } 
      }

      return newPhoto;
    }
    
    public static char[,] FlipPhoto(char[,] photo)
    {
      int rows = photo.GetLength(0);
      int cols = photo.GetLength(1);
      char[,] newPhoto = new char[rows, cols];
      for (int x = 0; x < rows; x++)
      {
        for (int y = 0; y < cols; y++)
        {
          newPhoto[x, y] = photo[rows - 1 - x, y];
        }
      }

      return newPhoto;
    }
  }
}