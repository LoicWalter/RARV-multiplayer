using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class GameOverRankLine : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _text;
  public void SetRankUI(string name, int rank)
  {
    _text.text = rank + ".  " + name + "\n";
  }
}
