using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
  public static class Helpers
  {
    public static string ToApplicationPath(this string fileName)
    {
      string exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
      Regex appPathMatcher=new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
      string appRoot = appPathMatcher.Match(exePath ?? throw new InvalidOperationException()).Value;
      return Path.Combine(appRoot, fileName);
    }

    public static BigInteger Factorial(this int number)
    {
      BigInteger total = 1;
      for (long i = 1; i <= number; i++)
      {
        total = BigInteger.Multiply(total, i);
      }
      // return (long)Enumerable.Range(1, number).Aggregate(1, (p, item) => (long)BigInteger.Multiply(p, item));
      // return (long)Enumerable.Range(1, number).Aggregate(1, (p, item) => p * item);
      return total;
    }

    public static BigInteger Multiply(this IEnumerable<int> values)
    {
      return values.Aggregate<int, BigInteger>(1, (current, value) => BigInteger.Multiply(value, current));
    }

    public static string ToBinaryString(this long number, int length)
    {
      // return Enumerable.Range(0, (int) Math.Log(number, 2) + 1).Aggregate(string.Empty, (collected, bitshift) => ((number >> bitshift) & 1 )+ collected);
       string bin = Convert.ToString(number, 2);
      string result = string.Empty;
      for (int i = 0; i < length - bin.Length; i++)
      {
        result += "0";
      }

      result += bin;
      return result;
    }

    public static long BinaryToNumber(this string binaryString)
    {
      return Convert.ToInt64(binaryString, 2);
    }
    
    public static long BinaryToNumber(this IEnumerable<char> binaryString)
    {
      return BinaryToNumber(new string(binaryString.ToArray()));
    }

    public static string SubstringBetween(this string text, string leftString, string rightString)
    {
      int leftIndex = text.IndexOf(leftString, StringComparison.InvariantCultureIgnoreCase);
      int rightIndex = text.LastIndexOf(rightString, StringComparison.InvariantCultureIgnoreCase);
      if (leftIndex != -1 && rightIndex != -1 && leftIndex < rightIndex)
      {
        return text.Substring(leftIndex + leftString.Length, rightIndex - (leftIndex + leftString.Length));
      }

      return string.Empty;
    }

    public static string ReverseString(this string text)
    {
      char[] array = text.ToCharArray();
      Array.Reverse(array);
      return new string(array);
    }
  }
}