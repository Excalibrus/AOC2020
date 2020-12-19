using System.Collections.Generic;

namespace Day18
{
  public static class Library
  {
    public static long CalculatePostfix(List<char> postfix)
    {
      Stack<long> stack = new Stack<long>();
      foreach (char character in postfix)
      {
        if(long.TryParse(character.ToString(), out long num))
        {
          stack.Push(num);
        }
        else if(stack.Count > 1)
        {
          if (character == '+') stack.Push(stack.Pop() + stack.Pop());
          else if (character == '-') stack.Push(stack.Pop() - stack.Pop());
          else if (character == '*') stack.Push(stack.Pop() * stack.Pop());
          else if (character == '/') stack.Push(stack.Pop() / stack.Pop());
        }
      }

      return stack.Pop();
    }
    
    public static List<char> ConvertInfixToPostFix(List<char> inFix, int part)
    {
      List<char> postFix = new List<char>();
      char arrival;
      Stack<char> opreratorsStack = new Stack<char>();
      foreach (char ruleObject in inFix)
      {
        if (int.TryParse(ruleObject.ToString(), out int num))
        {
          postFix.Add(ruleObject);
        }
        else if (ruleObject == '(')
        {
          opreratorsStack.Push(ruleObject);
        }
        else if (ruleObject == ')') 
        {
          arrival = opreratorsStack.Pop();
          while (arrival != '(')
          {
            postFix.Add(arrival);
            if (opreratorsStack.Count == 0)
            {
              break;
            }
            arrival = opreratorsStack.Pop();
          }
        }
        else
        {
          if (opreratorsStack.Count != 0 && Predecessor(opreratorsStack.Peek(), ruleObject, part))
          {
            arrival = opreratorsStack.Peek();
            while (Predecessor(arrival, ruleObject, part))
            {
              postFix.Add(opreratorsStack.Pop());

              if (opreratorsStack.Count == 0)
                break;

              arrival = opreratorsStack.Peek();
            }
            opreratorsStack.Push(ruleObject);
          }
          else
            opreratorsStack.Push(ruleObject);
        }
      }
      while (opreratorsStack.Count > 0)
      {
        arrival = opreratorsStack.Pop();
        postFix.Add(arrival);
      }
      return postFix;
    }
    
    private static bool Predecessor(char firstObject, char secondObject, int part)
    {
      if (part == 1)
      {
        return (firstObject == '*' || firstObject == '/' || firstObject == '+' || firstObject == '-') &&
               (secondObject == '*' || secondObject == '/' || secondObject == '+' || secondObject == '-');  
      }
      else
      {
        int firstOperatorPrecendence = 0;
        int secondOperatorPrecendence = 0;

        if (firstObject == '*' || firstObject == '/' || firstObject == '+' || firstObject == '-')
        {
          if (firstObject == '+' || firstObject == '-')
          {
            firstOperatorPrecendence = 2;
          }
          else if (firstObject == '*' || firstObject == '/')
          {
            firstOperatorPrecendence = 1;
          }
        }

        if (secondObject == '*' || secondObject == '/' || secondObject == '+' || secondObject == '-')
        {
          if (secondObject == '+' || secondObject == '-')
          {
            secondOperatorPrecendence = 2;
          }
          else if (secondObject == '*' || secondObject == '/')
          {
            secondOperatorPrecendence = 1;
          }
        }
      
        return firstOperatorPrecendence >= secondOperatorPrecendence;
      }
      
      
    }
  }
}