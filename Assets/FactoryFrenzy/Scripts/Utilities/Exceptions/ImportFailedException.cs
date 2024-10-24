using System;

public class ImportFailedException : Exception
{
  public ImportFailedException() { }

  public ImportFailedException(string message)
      : base(message) { }

  public ImportFailedException(string message, Exception inner)
      : base(message, inner) { }
}