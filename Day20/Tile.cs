using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Common;

namespace Day20
{
  public class Tile
  {
    public int Id { get; set; }
    public List<char[,]> Photos { get; set; }

    public Tile(List<string> tileLines)
    {
      Photos = new List<char[,]>();
      char[,] photo = new char[tileLines.Count - 1, tileLines[1].Length];
      for (int i = 0; i < tileLines.Count; i++)
      {
        if (i == 0)
        {
          Id = int.Parse(tileLines[i].SubstringBetween("Tile ", ":"));
        }
        else
        {
          for (int j = 0; j < tileLines[i].Length; j++)
          {
            photo[i-1, j] = tileLines[i][j];
          }
        }
      }
      Photos.Add(photo);
      for (int i = 0; i < 3; i++)
      {
        char[,] rotatedPhoto = PhotoRotator.RotatePhotoCounterClockwise(Photos[i]);
        Photos.Add(rotatedPhoto);
      }

      for (int i = 0; i < 4; i++)
      {
        char[,] flippedPhoto = PhotoRotator.FlipPhoto(Photos[i]);
        Photos.Add(flippedPhoto);
      }
    }

    public char[] GetLine(List<(int x, int y)> positions, int photoIndex)
    {
      return positions.Select(position => Photos[photoIndex][position.x, position.y]).ToArray();
    }

    public void DrawTile(int index)
    {
      Console.WriteLine($"Tile {Id}:\n");
      for (int i = 0; i < Photos[index].GetLength(0); i++)
      {
        for (int j = 0; j < Photos[index].GetLength(1); j++)
        {
          Console.Write(Photos[index][i,j]);
        }
        Console.Write("\n");
      }
      Console.WriteLine();
    }


  }
}