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

  public static bool Save<T>(List<T> obj, string fileName, string pathName) where T : struct
  {
    try
    {
      string savePath = GetFullPath(fileName, ref pathName);
      string json = JsonConvert.SerializeObject(obj);

      if (!Directory.Exists(_streamingAssetsPath + "/" + pathName))
      {
        Directory.CreateDirectory(_streamingAssetsPath + "/" + pathName);
      }

      using (StreamWriter file = new StreamWriter(savePath))
      {
        file.Write(json);
      }

      return true;
    }
    catch (Exception e)
    {
      Logger.Log(e.Message);
      return false;
    }
  }

  public static bool TrySerialization<T>(string json, out List<T> obj) where T : struct
  {
    obj = default;
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
    string[] files = Directory.GetFiles(_streamingAssetsPath + "/" + pathName, "*.json", SearchOption.TopDirectoryOnly);
    foreach (string file in files)
    {
      string fileName = Path.GetFileName(file);
      fileNames.Add(fileName[..^5]);
    }
    return fileNames;
  }
}