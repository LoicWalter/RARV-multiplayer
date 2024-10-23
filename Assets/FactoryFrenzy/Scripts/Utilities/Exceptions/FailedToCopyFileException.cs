using System;

public class FailedToCopyFileException : Exception
{
  public FailedToCopyFileException() { }

  public FailedToCopyFileException(string message)
      : base(message) { }

  public FailedToCopyFileException(string message, Exception inner)
      : base(message, inner) { }
}