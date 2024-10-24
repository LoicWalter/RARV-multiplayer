using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public struct SerializableVector3
{
  public float x;
  public float y;
  public float z;

  /// <summary>
  /// Creates a new serializable vector3.
  /// It is possible to pass a vector3 to this constructor, instead of the individual values.
  /// </summary>
  /// <param name="x"></param>
  /// <param name="y"></param>
  /// <param name="z"></param>
  public SerializableVector3(float x, float y, float z)
  {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  /// <summary>
  /// Converts a serializable vector3 to a vector3.
  /// </summary>
  /// <param name="sVector"></param>
  public static implicit operator Vector3(SerializableVector3 sVector)
  {
    return new Vector3(sVector.x, sVector.y, sVector.z);
  }

  /// <summary>
  /// Converts a vector3 to a serializable vector3.
  /// </summary>
  /// <param name="vector"></param>
  public static implicit operator SerializableVector3(Vector3 vector)
  {
    return new SerializableVector3(vector.x, vector.y, vector.z);
  }
}

public struct SerializableQuaternion
{
  public float x;
  public float y;
  public float z;
  public float w;

  /// <summary>
  /// Creates a new serializable quaternion.
  /// It is possible to pass a quaternion to this constructor, instead of the individual values.
  /// </summary>
  /// <param name="x"></param>
  /// <param name="y"></param>
  /// <param name="z"></param>
  /// <param name="w"></param>
  public SerializableQuaternion(float x, float y, float z, float w)
  {
    this.x = x;
    this.y = y;
    this.z = z;
    this.w = w;
  }

  /// <summary>
  /// Converts a serializable quaternion to a quaternion.
  /// </summary>
  /// <param name="sQuaternion"></param>
  public static implicit operator Quaternion(SerializableQuaternion sQuaternion)
  {
    return new Quaternion(sQuaternion.x, sQuaternion.y, sQuaternion.z, sQuaternion.w);
  }

  /// <summary>
  /// Converts a quaternion to a serializable quaternion.
  /// </summary>
  /// <param name="quaternion"></param>
  public static implicit operator SerializableQuaternion(Quaternion quaternion)
  {
    return new SerializableQuaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
  }
}

public struct LevelObject
{
  public string prefabName;
  public SerializableVector3 position;
  public SerializableQuaternion rotation;
  public SerializableVector3 scale;

  public SerializableVector3 firstParameter;
  public SerializableVector3 secondParameter;
}

public static class Importer
{
  private static readonly string _streamingAssetsPath = Application.streamingAssetsPath;

  /// <summary>
  /// Removes the first and last character if they are a slash.
  /// </summary>
  /// <param name="pathName">
  ///   The path to remove the slashes from
  /// </param>
  private static void RemoveUnecessaryCharacters(ref string pathName)
  {
    if (pathName.StartsWith("/"))
    {
      pathName = pathName[1..];
    }

    if (pathName.EndsWith("/"))
    {
      pathName = pathName[..^1];
    }
  }

  private static string GetFullPath(string fileName, ref string pathName)
  {
    RemoveUnecessaryCharacters(ref pathName);
    return _streamingAssetsPath + "/" + pathName + "/" + fileName + ".json";
  }

  public static bool Load<T>(string fileName, string pathName, out List<T> values) where T : struct
  {
    values = default;
    try
    {
      string loadPath = GetFullPath(fileName, ref pathName);
      Logger.Log(loadPath);
      if (!File.Exists(loadPath))
      {
        Logger.Log("File does not exist");
        return false;
      }
      using StreamReader file = File.OpenText(loadPath);
      JsonSerializer serializer = new();
      values = (List<T>)serializer.Deserialize(file, typeof(List<T>));
      return true;
    }
    catch (Exception e)
    {
      Logger.Log(e.Message);
      return false;
    }
  }

  public static bool TrySerialization<T>(string json) where T : struct
  {
    return TrySerialization<T>(json, out _);
  }

  public static bool TrySerialization<T>(string json, out List<T> obj) where T : struct
  {
    obj = default;

    if (string.IsNullOrEmpty(json) || json.Length < 2)
    {
      return false;
    }

    try
    {
      obj = JsonConvert.DeserializeObject<List<T>>(json);
      return true;
    }
    catch (Exception e)
    {
      Logger.Log(e.Message);
      return false;
    }
  }

  /// <summary>
  /// Returns a list of file names in the specified path, In the StreamingAssets folder.
  /// </summary>
  /// <param name="pathName">
  ///   The path to get the file names from.
  /// </param>
  /// <returns>
  ///   A list of json file names (without extensions) within the specified path.
  /// </returns>
  public static List<string> GetJsonFileNames(string pathName)
  {
    List<string> fileNames = new();
    RemoveUnecessaryCharacters(ref pathName);
    string folderPath = _streamingAssetsPath + "/" + pathName;
    Logger.Log(folderPath);
    string[] files = Directory.GetFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly);
    foreach (string file in files)
    {
      string fileName = Path.GetFileName(file);
      fileNames.Add(fileName[..^5]);
    }
    return fileNames;
  }

  public static bool AddJSONFilesFromExplorer(out List<string> errorMessages, string path)
  {
    errorMessages = new List<string>();
    string[] paths = SFB.StandaloneFileBrowser.OpenFilePanel("Choose JSON files", path, "json", true);

    foreach (string p in paths)
    {
      try
      {
        TryImportJSONFilesOrThrow<LevelObject>(p, path);
      }
      catch (Exception e)
      {
        Logger.LogError(e.Message);
        errorMessages.Add("Failed to import file: " + p);
      }
    }

    return errorMessages.Count == 0;
  }

  /// <summary>
  /// Tries to import a JSON file, if it fails, it throws an exception.
  /// </summary>
  /// <typeparam name="T">
  ///   The type of the object to import.
  /// </typeparam>
  /// <param name="filePath">
  ///   The path to the file to import.
  /// </param>
  /// <param name="localPath">
  ///   The relative path to save the file to.
  /// </param>
  /// <exception cref="FileAlreadyExistException"></exception>
  /// <exception cref="FailedToCopyFileException"></exception>
  /// <exception cref="FailedToSerializedException"></exception>
  private static void TryImportJSONFilesOrThrow<T>(string filePath, string localPath) where T : struct
  {
    string fileName = Path.GetFileNameWithoutExtension(filePath);
    string fullLocalPath = GetFullPath(fileName, ref localPath);
    Logger.Log(fullLocalPath);
    if (File.Exists(fullLocalPath))
    {
      throw new FileAlreadyExistException("File already exists");
    }

    if (!TrySerialization<T>(File.ReadAllText(filePath)))
    {
      throw new FailedToSerializedException("Failed to serialize file");
    }

    try
    {
      File.Copy(filePath, fullLocalPath);
    }
    catch (Exception)
    {
      throw new FailedToCopyFileException("Failed to copy file");
    }

    Logger.Log("File imported successfully");
  }
}