using System.Collections.Generic;
using System.Linq;

namespace Common
{
  
  public class CircleList<T>: LinkedList<T>
  {
    public CircleList(List<T> numbers): base(numbers)
    {
    }

    public IEnumerable<LinkedListNode<T>> RemoveNextThree(LinkedListNode<T> currentNode)
    {
      foreach(LinkedListNode<T> node in currentNode.NextOfNumber(3).ToList())
      {
        Remove(node);
        yield return node;
      }
    }

    public void InsertAfterNode(LinkedListNode<T> currentNode, params LinkedListNode<T>[] nodes)
    {
      foreach (LinkedListNode<T> node in nodes.Reverse())
      {
        AddAfter(currentNode,node);
      }
    }
  }

  public static class CircleListExtensions
  {
    public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
    {
      return current.Next ?? current.List.First;
    }
    
    public static IEnumerable<LinkedListNode<T>> NextOfNumber<T>(this LinkedListNode<T> currentNode, int nextNumber)
    {
      for (int i = 0; i < nextNumber; i++)
      {
        currentNode = currentNode.NextOrFirst();
        yield return currentNode;
      } 
    }
    
    public static IEnumerable<LinkedListNode<T>> PreviousOfNumber<T>(this LinkedListNode<T> currentNode, int nextNumber)
    {
      for (int i = 0; i < nextNumber; i++)
      {
        currentNode = currentNode.PreviousOrLast();
        yield return currentNode;
      } 
    }

    public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
    {
      return current.Previous ?? current.List.Last;
    }
    
    public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> list)
    {
      for (var node = list.First; node != null; node = node.Next)
      {
        yield return node;
      }
    }
  }
}