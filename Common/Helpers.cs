using System;
using System.IO;
using System.Linq;
using System.Numerics;
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
  }
}