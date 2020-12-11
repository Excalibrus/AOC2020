using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace Day04
{
  class Program
  {
    static void Main(string[] args)
    {
      string[] fileLines = File.ReadAllLines("day04.txt".ToApplicationPath());

      List<Passport> passports = ParseLinesInPassports(fileLines);

      int valid = passports.Count(x => x.IsValid);
      int validPartTwo = passports.Count(x => x.IsValidPartTwo);
    }

    static List<Passport> ParseLinesInPassports(string[] lines)
    {
      List<Passport> passports = new List<Passport>();
      Passport currentPassport = new Passport();
      for (int index = 0; index < lines.Length; index++)
      {
        string line = lines[index];
        if (line == string.Empty)
        {
          passports.Add(currentPassport);
          currentPassport = new Passport();
        }

        List<string> passwordParams = line.Split(' ').ToList();
        foreach (string param in passwordParams)
        {
          List<string> parameterDetails = param.Split(':').ToList();
          switch (parameterDetails[0])
          {
            case "byr":
              currentPassport.BirthYear = parameterDetails[1];
              break;
            case "iyr":
              currentPassport.IssueYear = parameterDetails[1];
              break;
            case "eyr":
              currentPassport.ExpirationYear = parameterDetails[1];
              break;
            case "hgt":
              currentPassport.Height = parameterDetails[1];
              break;
            case "hcl":
              currentPassport.HairColor = parameterDetails[1];
              break;
            case "ecl":
              currentPassport.EyeColor = parameterDetails[1];
              break;
            case "pid":
              currentPassport.PassportId = parameterDetails[1];
              break;
            case "cid":
              currentPassport.CountryId = parameterDetails[1];
              break;
          }
        }

        if (index == lines.Length - 1)
        {
          passports.Add(currentPassport);
        }
      }

      return passports;
    }
        
  }
}