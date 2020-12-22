using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common;

namespace Day21
{
  class Program
  {
    private static List<(List<string> ingrediens, List<string> alergens)> _parsedInput;
    private static List<(string alergen, List<string> ingredients)> _testList;
    private static Dictionary<string, List<string>> _ingredientsPerAlergen;
    private static List<string> _allIngredients;
    private static List<string> _allAlergens;

    static void Main(string[] args)
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      ParseData();
      var result = DoAlgorithm();
      int partOne = result.partOne;
      string partTwo = result.partTwo;

      watch.Stop();

      Console.WriteLine(
          $"Part one result: {partOne}\nPart two result: {partTwo}\nExecution time: {watch.ElapsedMilliseconds} ms");
    }

    static (int partOne, string partTwo) DoAlgorithm()
    {
      Dictionary<string, string> allergenIngredientPairs = new Dictionary<string, string>();
      while (allergenIngredientPairs.Count < _allAlergens.Count)
      {
        foreach (string alergen in _allAlergens)
        {
          // all ingredients of this alergen
          List<List<string>> listsOfIngredients = _testList.Where(x => x.alergen == alergen).Select(x => x.ingredients).ToList();
          if(listsOfIngredients.Count == 0) continue;
          // unique ingredients of all alergen instances
          List<string> uniqueIngredients = listsOfIngredients.Count == 1 ? listsOfIngredients.First() : listsOfIngredients.Skip(1).ToList().Aggregate(listsOfIngredients.First(), (current, ingredients) => current.Intersect(ingredients).Distinct().ToList());
          // remove lists of this alergen from list
          _testList = _testList.Where(x => x.alergen != alergen).ToList();
          
          if (uniqueIngredients.Count == 1)
          {
            allergenIngredientPairs.Add(alergen, uniqueIngredients.First());
            for (int index = 0; index < _testList.Count; index++)
            {
              (string alergen, List<string> ingredients) pair = _testList[index];
              pair.ingredients = pair.ingredients.Where(x => x != uniqueIngredients.First()).ToList();
              _testList[index] = pair;
            }
          }
          else if(uniqueIngredients.Count > 1)
          {
            _testList.Add((alergen, uniqueIngredients));
          }
          
        }
      }

      List<string> nonUsedIngredients = _allIngredients.Where(x => !allergenIngredientPairs.Values.Contains(x)).ToList();
      int nonUsedIngredientsSum = _parsedInput.Select(x => x.ingrediens).Sum(x => x.Count(y => nonUsedIngredients.Contains(y)));
      string ingredientsList = string.Join(",", allergenIngredientPairs.OrderBy(x => x.Key).Select(x => x.Value));

      return (nonUsedIngredientsSum, ingredientsList);
    }

    static void ParseData()
    {
      _parsedInput = new List<(List<string> ingrediens, List<string> alergens)>();
      _testList = new List<(string alergen, List<string> ingredients)>();
      _ingredientsPerAlergen = new Dictionary<string, List<string>>();
      _allAlergens = new List<string>();
      _allIngredients = new List<string>();
      bool parseRules = true;
      string[] lines = File.ReadAllLines("input.txt".ToApplicationPath());
      foreach (string line in lines)
      {
        if (string.IsNullOrWhiteSpace(line)) continue;
        string[] lineParts = line.Split('(');
        List<string> ingredients = lineParts[0].Split(new[]{ ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> allergens = lineParts[1].SubstringBetween("contains", ")")
            .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
        _parsedInput.Add((ingredients, allergens));
        _allAlergens.AddRange(allergens);
        _allAlergens = _allAlergens.Distinct().ToList();
        _allIngredients.AddRange(ingredients);
        foreach (string allergen in allergens)
        {
          _testList.Add((allergen, ingredients));
          if (!_ingredientsPerAlergen.ContainsKey(allergen))
          {
            _ingredientsPerAlergen.Add(allergen, new List<string>());
          }

          _ingredientsPerAlergen[allergen].AddRange(ingredients);
        }
      }

      foreach (string key in _ingredientsPerAlergen.Keys.ToList())
      {
        _ingredientsPerAlergen[key] = _ingredientsPerAlergen[key].Distinct().ToList();
      }

      _allAlergens = _allAlergens.Distinct().ToList();
      _allIngredients = _allIngredients.Distinct().ToList();
    }
  }
}