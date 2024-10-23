using UnityEngine;
using System.Diagnostics;

public static class Logger
{
  public static void Log(string message)
  {
    string caller = GetCaller();
    UnityEngine.Debug.Log($"[INFO] {caller}: {message}");
  }

  public static void LogWarning(string message)
  {
    string caller = GetCaller();
    UnityEngine.Debug.LogWarning($"[WARN] {caller}: {message}");
  }

  public static void LogError(string message)
  {
    string caller = GetCaller();
    UnityEngine.Debug.LogError($"[ERROR] {caller}: {message}");
  }

  private static string GetCaller()
  {
    StackTrace stackTrace = new();
    StackFrame frame = stackTrace.GetFrame(2); // 0 is GetCaller, 1 is Log, 2 is the caller of Log
    var method = frame.GetMethod();
    return $"{method.DeclaringType.Name} - {method.Name}";
  }
}