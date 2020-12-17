using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day17
{
  class Program
  {
    private static Dictionary<(int x, int y, int z, int w), char> _matrix;
    private static List<(int x, int y, int z, int w)> _neighbourOffsets;
    private static (int x, int y, int z, int w) _maxIndex = (0,0,0,0);
    private static (int x, int y, int z, int w) _minIndex = (0,0,0,0);
    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();
      
      InitNeighbourOffsets();
      
      AddInitialState(File.ReadAllLines("input.txt".ToApplicationPath()));
      long partOne = Calculate(1);
      
      AddInitialState(File.ReadAllLines("input.txt".ToApplicationPath()));
      long partTwo = Calculate(2);

      watch.Stop();

      Console.WriteLine(
          $"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    private static int Calculate(int part)
    {
      // DrawMatrix();
      for (int i = 0; i < 6; i++)
      {
        AddOuterValues(part);
        DoCycle(part);
        // DrawMatrix();
      }

      int total = _matrix.Values.Count(x => x == '#');
      return total;
    }

    private static void InitNeighbourOffsets()
    {
      _neighbourOffsets = new List<(int x, int y, int z, int w)>();
      for(int x = -1; x < 2; x++)
      for(int y = -1; y < 2; y++)
      for(int z = -1; z < 2; z++)
      for(int w = -1; w < 2; w++)
        _neighbourOffsets.Add((x, y, z, w));
      _neighbourOffsets.Remove((0, 0, 0, 0));
    }

    private static void AddOuterValues(int part)
    {
      _minIndex.x--;
      _minIndex.y--;
      _minIndex.z--;
      _minIndex.w--;
      _maxIndex.x++;
      _maxIndex.y++;
      _maxIndex.z++;
      _maxIndex.w++;
      for (int x = _minIndex.x; x <= _maxIndex.x; x++)
      for (int y = _minIndex.y; y <= _maxIndex.y; y++)
      for (int z = _minIndex.z; z <= _maxIndex.z; z++)
      for (int w = part == 2 ? _minIndex.w : 0; w <= (part == 2 ? _maxIndex.w : 0); w++)
      {
        if(!_matrix.ContainsKey((x,y,z, w))) 
          _matrix.Add((x, y, z, w), '.');
      }
        
    }

    private static void AddInitialState(string[] lines)
    {
      _matrix = new Dictionary<(int x, int y, int z, int w), char>();
      _maxIndex = (lines.Length - 1, lines[0].Length - 1, 0, 0);
      for (int y = 0; y < lines.Length; y++)
      for (int x = 0; x < lines[y].Length; x++)
      {
        _matrix.Add((x, y, 0, 0), lines[y][x]);
      }
    }

    private static IEnumerable<(int, int, int, int)> GetNeighbourPositions((int x, int y, int z, int w) position)
    {
      foreach ((int nx, int ny, int nz, int nw) in _neighbourOffsets)
      {
        yield return (nx + position.x, ny + position.y, nz + position.z, nw + position.w);
      }
    }

    private static void DoCycle(int part)
    {
      Dictionary<(int x, int y, int z, int w), char> matrix = new Dictionary<(int x, int y, int z, int w), char>(_matrix);
      for (int x = _minIndex.x; x <= _maxIndex.x; x++)
      for (int y = _minIndex.y; y <= _maxIndex.y; y++)
      for (int z = _minIndex.z; z <= _maxIndex.z; z++)
      for (int w = part == 2 ? _minIndex.w : 0; w <= (part == 2 ? _maxIndex.w : 0); w++)
      {
        int activeNeighbours = GetNeighbourPositions((x, y, z, w)).Where(item => _matrix.ContainsKey(item)).Count(item => _matrix[(item.Item1, item.Item2, item.Item3, item.Item4)] == '#');
        if (_matrix[(x, y, z, w)] == '.' && activeNeighbours == 3)
        {
          matrix[(x, y, z, w)] = '#';
        }
        else if (_matrix[(x, y, z, w)] == '#' && activeNeighbours != 3 && activeNeighbours != 2)
        {
          matrix[(x, y, z, w)] = '.';
        }
      }

      _matrix = new Dictionary<(int x, int y, int z, int w), char>(matrix);
    }

    private static void DrawMatrix()
    {
      Console.WriteLine("Drawing matrix \n");
      for (int z = _minIndex.z; z <= _maxIndex.z; z++)
      {
        Console.WriteLine($"\nMatrix z = {z}");
        for (int x = _minIndex.x; x <= _maxIndex.x; x++)
        {
          for (int y = _minIndex.y; y <= _maxIndex.y; y++)
          {
            Console.Write(_matrix[(x,y,z, 0)]);
          }    
          Console.Write("\n");
        }  
      }
      Console.Write("\n\n\n");
    }
  }
}