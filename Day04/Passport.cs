using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
  public class Passport
  {
    public string BirthYear { get; set; }
    public string IssueYear { get; set; }
    public string ExpirationYear { get; set; }
    public string Height { get; set; }
    public string HairColor { get; set; }
    public string EyeColor { get; set; }
    public string PassportId { get; set; }
    public string CountryId { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(BirthYear) &&
        !string.IsNullOrWhiteSpace(IssueYear) &&
        !string.IsNullOrWhiteSpace(ExpirationYear) &&
        !string.IsNullOrWhiteSpace(Height) &&
        !string.IsNullOrWhiteSpace(HairColor) &&
        !string.IsNullOrWhiteSpace(EyeColor) &&
        !string.IsNullOrWhiteSpace(PassportId);
    
    public bool IsValidPartTwo 
    {
      get
      {
        if (!IsValid)
        {
          return false;
        }
        if(BirthYear.Length != 4 ||
           !int.TryParse(BirthYear, out int parsedBirth) ||
           parsedBirth < 1920 ||
           parsedBirth > 2002)
        {
          return false;
        }
        if(IssueYear.Length != 4 ||
           !int.TryParse(IssueYear, out int parsedIssue) ||
           parsedIssue < 2010 ||
           parsedIssue > 2020)
        {
          return false;
        }
        
        if(ExpirationYear.Length != 4 ||
           !int.TryParse(ExpirationYear, out int parsedExp) ||
           parsedExp < 2020 ||
           parsedExp > 2030)
        {
          return false;
        }

        string heightPostFix = Height.Substring(Height.Length - 2, 2);
        if (heightPostFix == "cm")
        {
          if (!int.TryParse(Height.Substring(0, Height.Length - 2), out int heightParsed) || heightParsed < 150 || heightParsed > 193)
          {
            return false;
          }
        }
        else if (heightPostFix == "in")
        {
          if (!int.TryParse(Height.Substring(0, Height.Length - 2), out int heightParsed) || heightParsed < 59 || heightParsed > 76)
          {
            return false;
          }
        }
        else
        {
          return false;
        }

        

        Regex regex = new Regex("^#(([0-9a-f]{2}){3}|([0-9a-f]){3})$");
        if (!regex.IsMatch(HairColor))
        {
          return false;
        }
        
        // if (HairColor.Substring(0, 1) != "#")
        // {
        //   return false;
        // }
        // bool CharValid(char c) => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f');
        // string hairPart = HairColor.Substring(1, HairColor.Length - 1);
        // if (hairPart.Length != 6 || !hairPart.All(CharValid))
        // {
        //   return false;
        // }

        if (EyeColor != "amb" && EyeColor != "blu" && EyeColor != "brn" && EyeColor != "gry" && EyeColor != "grn" &&
            EyeColor != "hzl" && EyeColor != "oth")
        {
          return false;
        }

        if (PassportId.Length != 9 || !int.TryParse(PassportId, out int parsedPid))
        {
          return false;
        }


        return true;
      }
      
    }
  }
  
  
}