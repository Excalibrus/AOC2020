using System;

namespace Day12
{
  public class Ship
  {
    public int North { get; set; }
    public int East { get; set; }
    public int WaypointNorth { get; set; }
    public int WaypointEast { get; set; }
    public Direction Direction { get; set; }

    public void Move(Instruction instruction)
    {
      if (instruction.Letter == "N") North += instruction.Number;
      else if (instruction.Letter == "E") East += instruction.Number;
      else if (instruction.Letter == "S") North -= instruction.Number;
      else if (instruction.Letter == "W") East -= instruction.Number;
      else if (instruction.Letter == "R")
      {
        int directionMoves = instruction.Number / 90;
        Direction = (int) Direction + directionMoves > 4
            ? (Direction)((int) Direction - 4 + directionMoves)
            : Direction + directionMoves;
      }

      else if (instruction.Letter == "L")
      {
        int directionMoves = instruction.Number / 90;
        Direction = (int) Direction - directionMoves < 1
            ? (Direction)((int) Direction + 4 - directionMoves)
            : Direction - directionMoves;
      }

      else if (instruction.Letter == "F")
      {
        if (Direction == Direction.North) North += instruction.Number;
        if (Direction == Direction.East) East += instruction.Number;
        if (Direction == Direction.South) North -= instruction.Number;
        if (Direction == Direction.West) East -= instruction.Number;
      }
    }
    
    public void MovePartTwo(Instruction instruction)
    {
      if (instruction.Letter == "N") WaypointNorth += instruction.Number;
      else if (instruction.Letter == "E") WaypointEast += instruction.Number;
      else if (instruction.Letter == "S") WaypointNorth -= instruction.Number;
      else if (instruction.Letter == "W") WaypointEast -= instruction.Number;
      else if (instruction.Letter == "R" || instruction.Letter == "L")
      {
        if (instruction.Number == 180)
        {
          WaypointNorth *= -1;
          WaypointEast *= -1;  
        }
        else
        {
          int swapTemp = WaypointNorth;
          WaypointNorth = WaypointEast;
          WaypointEast = swapTemp;
          if (instruction.Letter == "L" && instruction.Number == 90 || instruction.Letter == "R" && instruction.Number == 270)
          {
            WaypointEast *= -1;
          }
          else
          {
            WaypointNorth *= -1;
          }
        }

      }
      
      else if (instruction.Letter == "F")
      {
        North += WaypointNorth * instruction.Number;
        East += WaypointEast * instruction.Number;
      }
    }

    public int GetManhattanDistance()
    {
      return Math.Abs(North) + Math.Abs(East);
    }
  }
}