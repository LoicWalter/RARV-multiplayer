using System;

public class FailedToSerializedException : Exception
{
  public FailedToSerializedException() { }

  public FailedToSerializedException(string message)
      : base(message) { }

  public FailedToSerializedException(string message, Exception inner)
      : base(message, inner) { }
}