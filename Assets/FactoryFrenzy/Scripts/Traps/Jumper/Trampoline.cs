using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
  [SerializeField] private float bounceForce = 4.5f;
  [SerializeField] private float bounceCooldown = 0.1f;
  private bool isBouncing = false;

  void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Player") && !isBouncing)
    {
      var player = other.gameObject.GetComponent<IPlayerMovable>();
      player.Rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

      //prevent OnCollisionEnter from being called twice
      isBouncing = true;
      StartCoroutine(ResetBounceFlag());
    }
  }

  private IEnumerator ResetBounceFlag()
  {
    yield return new WaitForSeconds(bounceCooldown);
    isBouncing = false;
  }

}