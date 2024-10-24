using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(DontDestroyOnLoad))]
public class FactoryFrenzyMapManager : MonoBehaviour
{
  public static FactoryFrenzyMapManager Instance { get; private set; }

  public List<LevelObject> LevelObjects { get; private set; }
  public string SelectedMap { get; private set; }

  public class MapListChangedEventArgs : EventArgs
  {
    public List<string> MapList;
  }

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  private void Start()
  {
    SetSelectedMap(GetMapNames().Count > 0 ? GetMapNames()[0] : string.Empty);
    LoadMap(SelectedMap);
  }

  public void InitializeLobbyFunction()
  {
    FactoryFrenzyLobby.Instance.OnMapNameChanged += FactoryFrenzyLobby_OnMapNameChanged;
  }

  public void DesinitializeLobbyFunction()
  {
    FactoryFrenzyLobby.Instance.OnMapNameChanged -= FactoryFrenzyLobby_OnMapNameChanged;
  }

  private void FactoryFrenzyLobby_OnMapNameChanged(object sender, EventArgs e)
  {
    SetSelectedMap(FactoryFrenzyLobby.Instance.MapName);
    if (NetworkManager.Singleton.IsHost)
    {
      LoadMap(SelectedMap);
    }
  }

  public List<string> GetMapNames()
  {
    return Importer.GetJsonFileNames("Maps");
  }

  public void SetSelectedMap(string mapName)
  {
    SelectedMap = mapName;
  }

  private void LoadMap(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
    {
      return;
    }

    if (!Importer.Load<LevelObject>(fileName, "Maps", out var values))
    {
      Logger.LogError("Failed to load map");
      return;
    }
    LevelObjects = values;
  }
}