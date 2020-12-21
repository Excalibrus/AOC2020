using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Common;

namespace Day20
{
  class Program
  {
    private static char[,] _bigImage;
    private static Dictionary<int, (int x, int y, char[,] photo, int index)> _bigImageParts;
    private static Dictionary<string, (List<(int x, int y)> cor, string cPos)> _lineCoordinates;
    private static Dictionary<int, (List<(string pos, int tile, int index, char[,] photo)> neighbours, char[,] image)> _tileMapper;
    private static List<(int x, int y)> _seaMonster;
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());
      List<Tile> tiles = new List<Tile>();
      List<string> tileLines = new List<string>();
      for (int i = 0; i < lines.Length; i++)
      {
        if (string.IsNullOrWhiteSpace(lines[i]))
        {
          tiles.Add(new Tile(tileLines));
          tileLines = new List<string>();
          continue;
        }
        tileLines.Add(lines[i]);
        if (i == lines.Length - 1)
        {
          tiles.Add(new Tile(tileLines));
        }
      }

      foreach (Tile tile in tiles)
      {
        // tile.DrawTile(4);
      }

      GenerateLines(tiles.First().Photos.First().GetLength(0));

      _tileMapper = new Dictionary<int, (List<(string pos, int tile, int index, char[,] photo)> neighbours, char[,] image)>();
      
      MapTiles(tiles.First().Id, 0, tiles);
      

      BigInteger partOne = _tileMapper.Where(x => x.Value.neighbours.Count == 2).Select(x => x.Key).Multiply();
      
      string[] seaMonsterLines = File.ReadAllLines("sea-monster.txt".ToApplicationPath());
      _seaMonster = new List<(int x, int y)>();
      for (int i = 0; i < seaMonsterLines.Length; i++)
      {
        for (int j = 0; j < seaMonsterLines[i].Length; j++)
        {
          if (seaMonsterLines[i][j] == '#')
          {
            _seaMonster.Add((i,j));
          }
        }
      }
      
      _bigImageParts = new Dictionary<int, (int x, int y, char[,] photo, int index)>();
      var firstItem = _tileMapper.First();
      // DrawTile(firstItem.Value.image);
      AddPhotosToParts(firstItem.Key, (0, 0, tiles.First(x => x.Id == firstItem.Key).Photos.First(), -1), "");
      NormalizeParts();
      GenerateBigPhoto();

      int partTwo = CountSeaMonsters();

      watch.Stop();

      Console.WriteLine($"Part one result: {partOne}\n" +
                        $"Part two result: {partTwo}\n" +
                        $"Execution time: {watch.ElapsedMilliseconds} ms\n" +
                        "");
    }

    private static void MapTiles(int tileId, int photoIndex, List<Tile> tiles)
    {
      if (_tileMapper.ContainsKey(tileId) && _tileMapper[tileId].neighbours.Count > 1) return;
      Tile tile = tiles.First(x => x.Id == tileId);
      List<(string pos, int tile, int index, char[,] photo)> neighbours = new List<(string pos, int tile, int index, char[,] photo)>();
      foreach (var coordinate in _lineCoordinates)
      {
        char[] currentTileLine = tile.GetLine(coordinate.Value.cor, photoIndex);
        foreach (Tile compareTile in tiles.Where(compareTile => !tile.Id.Equals(compareTile.Id)))
        {
          for (int index = 0; index < compareTile.Photos.Count; index++)
          {
            char[] comparingTileLine = compareTile.GetLine(_lineCoordinates[coordinate.Value.cPos].cor, index);
            if (new string(currentTileLine).Equals(new string(comparingTileLine)))
            {
              neighbours.Add((coordinate.Value.cPos, compareTile.Id, index, compareTile.Photos[index]));
            }
          }
        }
      }
      _tileMapper.Add(tileId, (neighbours, tile.Photos[photoIndex]));
      foreach (var neighbour in neighbours)
      {
        MapTiles(neighbour.tile, neighbour.index, tiles);
      }
    }

    private static void AddPhotosToParts(int tileId, (int x, int y, char[,] photo, int index) parentCor, string pos)
    {
      if (_bigImageParts.ContainsKey(tileId))
      {
        if (_bigImageParts[tileId].index != parentCor.index)
        {
          _bigImageParts[tileId] = (
              _bigImageParts[tileId].x, 
              _bigImageParts[tileId].y, 
              parentCor.photo,
              parentCor.index);
        }

        return;
      }
      if (!string.IsNullOrWhiteSpace(pos))
      {
        if (pos == "top") parentCor.y++;
        if (pos == "right") parentCor.x--;
        if (pos == "bottom") parentCor.y--;
        if (pos == "left") parentCor.x++;
      }
      _bigImageParts.Add(tileId, parentCor);
      
      foreach (var part in _tileMapper[tileId].neighbours)
      {
        AddPhotosToParts(part.tile, (parentCor.x, parentCor.y, part.photo, part.index), part.pos);
      }
    }

    private static void NormalizeParts()
    {
      int minY = _bigImageParts.Values.Select(x => x.y).Min();
      int minX = _bigImageParts.Values.Select(x => x.x).Min();
      if (minY < 0)
      {
        foreach (int key in _bigImageParts.Keys.ToList())
        {
          _bigImageParts[key] = (_bigImageParts[key].x, _bigImageParts[key].y + (-1 * minY), _bigImageParts[key].photo, _bigImageParts[key].index);
        }
      }

      if (minX < 0)
      {
        foreach (int key in _bigImageParts.Keys.ToList())
        {
          _bigImageParts[key] = (_bigImageParts[key].x + (-1 * minX), _bigImageParts[key].y, _bigImageParts[key].photo, _bigImageParts[key].index);
        }
      }
    }

    private static void GenerateBigPhoto()
    {
      int photoWithoutBorderSize = _bigImageParts.Values.Select(x => x.photo).First().GetLength(0) - 2;
      int size = _bigImageParts.Values.Select(x => x.x).Max();
      _bigImage = new char[photoWithoutBorderSize * (size + 1), photoWithoutBorderSize * (size + 1)];
      for (int i = 0; i <= size; i++)
      {
        for (int j = 0; j <= size; j++)
        {
          var imagePart = _bigImageParts.First(x => x.Value.x == j && x.Value.y == i);
          char[,] photoWithoutBorders = GetPhotoWithoutBorders(imagePart.Value.photo);
          // Console.WriteLine($"Modifying id {imagePart.Key}");
          for (int x = 0; x < photoWithoutBorderSize; x++)
          {
            for (int y = 0; y < photoWithoutBorderSize; y++)
            {
              _bigImage[photoWithoutBorderSize * i + x, photoWithoutBorderSize * j + y] = photoWithoutBorders[x, y];
            }
          }
        } 
      }
    }

    private static int CountSeaMonsters()
    {
      List<int> counters = new List<int>();
      List<char[,]> photos = new List<char[,]>{ _bigImage.Clone() as char[,] };
      for (int ind = 0; ind < 8; ind++)
      {
        int counter = 0;
        char[,] currentPhoto = photos.Last();
        
        int maxX = _seaMonster.Select(x => x.x).Max();
        int maxY = _seaMonster.Select(x => x.y).Max();
        for (int i = 0; i < currentPhoto.GetLength(0) - maxX; i++)
        {
          for (int j = 0; j < currentPhoto.GetLength(1) - maxY; j++)
          {
            if (_seaMonster.All(monsterCor => currentPhoto[i + monsterCor.x, j + monsterCor.y] == '#'))
            {
              _seaMonster.ForEach(x => currentPhoto[i + x.x, j + x.y] = 'O');
              counter++;
            }
          }
        }

        if (counter > 0)
        {
          int finalCounter = 0;
          for (int i = 0; i < currentPhoto.GetLength(0); i++)
          {
            for (int j = 0; j < currentPhoto.GetLength(1); j++)
            {
              if (currentPhoto[i, j] == '#')
              {
                finalCounter++;
              }
            }
          }
          counters.Add(finalCounter);
          DrawTile(currentPhoto);
        }

        char[,] newPhoto = ind < 4
            ? PhotoRotator.RotatePhotoCounterClockwise(currentPhoto)
            : PhotoRotator.FlipPhoto(photos[ind - 4]);
        photos.Add(newPhoto);
      }

      return counters.Max();
    }
    private static char[,] GetPhotoWithoutBorders(char[,] photo)
    {
      char[,] newPhoto = new char[photo.GetLength(0) - 2, photo.GetLength(1) - 2];
      for (int i = 1; i < photo.GetLength(0) - 1; i++)
      {
        for (int j = 1; j < photo.GetLength(1) - 1; j++)
        {
          newPhoto[i - 1, j - 1] = photo[i, j];
        }  
      }
      // Console.WriteLine("Before");
      // DrawTile(photo);
      // Console.WriteLine("After");
      // DrawTile(newPhoto);
      // Console.WriteLine();
      return newPhoto;
    }

    private static void DrawTile(char[,] tile)
    {
      Console.WriteLine();
      Console.ForegroundColor = ConsoleColor.DarkGray;
      for (int i = 0; i < tile.GetLength(0); i++)
      {
        for (int j = 0; j < tile.GetLength(1); j++)
        {
          if (tile[i, j] == 'O')
          {
            // ConsoleColor prevBack = Console.BackgroundColor; 
            ConsoleColor prevFor = Console.ForegroundColor; 
            // Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(tile[i,j]);
            // Console.BackgroundColor = prevBack;
            Console.ForegroundColor = prevFor;
          }
          else
          {
            Console.Write(tile[i,j]);  
          }
          
        }
        Console.Write("\n");
      }
    } 

    private static void GenerateLines(int size)
    {
      _lineCoordinates = new Dictionary<string, (List<(int x, int y)> cor, string cPos)>();
      _lineCoordinates.Add("top", (new List<(int x, int y)>(), "bottom"));
      _lineCoordinates.Add("right", (new List<(int x, int y)>(), "left"));
      _lineCoordinates.Add("bottom", (new List<(int x, int y)>(), "top"));
      _lineCoordinates.Add("left", (new List<(int x, int y)>(), "right"));
      for (int x = 0; x < size; x++)
      {
        _lineCoordinates["top"].cor.Add((0, x));
        _lineCoordinates["bottom"].cor.Add((size-1, x));
        _lineCoordinates["left"].cor.Add((x, 0));
        _lineCoordinates["right"].cor.Add((x, size-1));
      }
    }
  }
}