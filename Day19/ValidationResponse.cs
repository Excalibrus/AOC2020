namespace Day19
{
  public class ValidationResponse
  {
    public int Index { get; set; }

    public bool Success { get; set; }

    public ValidationResponse(int index, bool success)
    {
      Index = index;
      Success = success;
    }

  }
}