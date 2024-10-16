using System.Collections.Generic;
using UnityEngine;

public interface ITriggerTurret
{
  List<GameObject> PlayersInRange { get; set; }

  void PlayerEnterRange(GameObject player);
  void PlayerExitRange(GameObject player);
}