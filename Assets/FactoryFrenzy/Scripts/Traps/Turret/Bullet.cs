using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  public static Bullet Instance { get; private set; }

  [Header("Settings")]
  [SerializeField] private bool _shouldGrow = true;
  [SerializeField] private float _timeToGrow = 2f;
  [SerializeField] private float _endSizeMultiplier = 5f;

  private Vector3 _currentScale;
  private float _timer = 0;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
  }

  void Start()
  {
    _currentScale = transform.localScale;
  }

  void Update()
  {
    if (_shouldGrow)
    {
      Grow();
    }
  }

  private void Grow()
  {
    _timer += Time.deltaTime;
    float t = _timer / _timeToGrow;

    if (t >= 1)
    {
      _shouldGrow = false;
      t = 1;
    }

    transform.localScale = Vector3.Lerp(_currentScale, _currentScale * _endSizeMultiplier, t);

    if (Instance == this)
    {
      Logger.Log("Bullet is growing: " + transform.localScale);
    }
  }
}
