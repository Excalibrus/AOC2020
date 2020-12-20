namespace Day19
{
  public class ValidationResponse
  {
    public int Index { get; set; }

    public bool CharacterFound { get; set; }

    public bool Success { get; set; }

    public Path Path { get; set; }

    public ValidationResponse(int index, bool characterFound, bool success, Path path)
    {
      Index = index;
      CharacterFound = characterFound;
      Success = success;
      Path = path;
    }

  }
}