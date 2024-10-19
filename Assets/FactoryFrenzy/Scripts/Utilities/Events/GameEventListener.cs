using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }

/// <summary>
/// A game event listener.
/// This class listens for a game event and invokes a response when the event is raised.
/// </summary>
public class GameEventListener : MonoBehaviour
{

  [Tooltip("Event to register with.")]
  public GameEvent gameEvent;

  [Tooltip("Response to invoke when Event with GameData is raised.")]
  public CustomGameEvent response;

  private void OnEnable()
  {
    gameEvent.RegisterListener(this);
  }

  private void OnDisable()
  {
    gameEvent.UnregisterListener(this);
  }

  public void OnEventRaised(Component sender, object data)
  {
    response.Invoke(sender, data);
  }

}
