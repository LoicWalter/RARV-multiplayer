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
    if (other.rigidbody.CompareTag("Player") && !isBouncing)
    {
      other.rigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

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
