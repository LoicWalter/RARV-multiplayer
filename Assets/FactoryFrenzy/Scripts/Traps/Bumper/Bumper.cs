using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
  [SerializeField] private float bumpForce = 3f; // Force de répulsion
  private void OnCollisionEnter(Collision collision)
  {
    // Vérifier si l'objet entrant en collision a un Rigidbody
    Rigidbody rb = collision.rigidbody;
    if (rb != null)
    {
      // Appliquer la force de répulsion
      Vector3 forceDirection = collision.GetContact(0).normal.normalized;
      rb.AddForce(-forceDirection * bumpForce, ForceMode.Impulse);
    }
  }
}
