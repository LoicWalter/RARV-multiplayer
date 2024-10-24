using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
  [SerializeField] private float bounceForce = 3;
  private AudioSource _audioSource;

  void Start()
  {
    _audioSource = gameObject.GetComponent<AudioSource>();
  }

  void OnCollisionEnter(Collision collision)
  {
    // Vérifier si l'objet entrant en collision a un Rigidbody
    Rigidbody rb = collision.rigidbody;
    if (rb != null)
    {
      //Audio
      _audioSource.Play();

      Vector3 bounceDirection = collision.GetContact(0).normal.normalized;
      rb.AddForce(-bounceDirection * bounceForce, ForceMode.Impulse);
    }
  }
}
