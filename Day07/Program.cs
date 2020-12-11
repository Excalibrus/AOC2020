using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Common;

namespace Day07
{
  class Program
  {
    static void Main(string[] args)
    {
      string[] fileLines = File.ReadAllLines("day07.txt".ToApplicationPath());
      
      List<Bag> bags = new List<Bag>();
      foreach (string fileLine in fileLines)
      {
        string[] lineParts = fileLine.Split(new[] { "contain" }, StringSplitOptions.RemoveEmptyEntries);
        if (lineParts.Length == 2)
        {
          string[] strings = lineParts[0].Split(' ');
          string type = strings[0];
          string color = strings[1];
          Bag bag = new Bag
          {
              Color = $"{type} {color}",
              Number = 1
          };
          List<string> innerBagParts = lineParts[1].Split(',').ToList();
          foreach (string innerBagPart in innerBagParts)
          {
            string[] split = innerBagPart.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(split[0], out int numberOfBags))
            {
              if (numberOfBags > 0)
              {
                bag.InnerBags.Add(new Bag
                {
                    Number = numberOfBags,
                    Color = $"{split[1]} {split[2]}"
                });  
              }
            }
          }
          bags.Add(bag);
        }
      }

      int totalNumberOfBags = bags.Count;

      int numberOfMyBags = CountBagsOfColorPartOne(bags, bags, "shiny gold");
      int numberOfMyBagsPartTwo = CountBagsOfColorPartTwo(bags,  "shiny gold", 1); //1370
    }

    static int CountBagsOfColorPartOne(List<Bag> bags, List<Bag> originalBags, string color)
    {
      int total = 0;
      foreach (Bag bag in bags)
      {
        if (bag.InnerBags.Any(x => x.Color.Equals(color, StringComparison.InvariantCultureIgnoreCase)))
        {
          total++;
        }
        else
        {
          if (bag.InnerBags.Count == 0)
          {
            Bag parentBag = originalBags.FirstOrDefault(x => x.Color == bag.Color && x.Id != bag.Id);
            if (parentBag != null)
            {
              int innerCount = CountBagsOfColorPartOne(new List<Bag>{ parentBag }, originalBags, color);
              if (innerCount > 0)
              {
                total++;
                break;
              }
            }
          }
          else
          {
            total += CountBagsOfColorPartOne(bag.InnerBags, originalBags, color);
          }  
        }
        
      }

      return total;
    }

    static int CountBagsOfColorPartTwo(List<Bag> bags, string color, int parentNum)
    {
      int total = 0;
      int fullChildren = 0;
      int emptyChildren = 0;
      Bag bag = bags.FirstOrDefault(x => x.Color == color);
      if (bag != null)
      {
        if (bag.InnerBags.Count == 0)
        {
          return 0;
        }

        
        foreach (Bag innerBag in bag.InnerBags)
        {
          int countBagsOfColorPartTwo = CountBagsOfColorPartTwo(bags, innerBag.Color, innerBag.Number);
          if (countBagsOfColorPartTwo != 0)
          {
            fullChildren += countBagsOfColorPartTwo + innerBag.Number;
          }
          else
          {
            emptyChildren += innerBag.Number;
          }
          
        }

        total = parentNum * (emptyChildren + fullChildren);

      }


      return total;
    }
  }
}