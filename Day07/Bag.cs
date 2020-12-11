using System;
using System.Collections.Generic;

namespace Day07
{
  public class Bag
  {
    public Guid Id { get; set; }
    public string Color { get; set; }
    public int Number { get; set; }
    public List<Bag> InnerBags { get; set; }

    public Bag()
    {
      Id = Guid.NewGuid();
      InnerBags = new List<Bag>();
    }
  }
}