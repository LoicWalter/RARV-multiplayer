using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformKeeper : MonoBehaviour
{
    private void Start() { }

    private void Update() { }

    // Gère l'entrée du joueur sur la plateforme
    private void OnCollisionEnter(Collision other)
    {
        // Logger.Log("CollisionEnter with " + other.gameObject.name);
        if (other.rigidbody.CompareTag("Player"))
        {
            // Parent le joueur à la plateforme pour qu'il bouge avec elle
            other.rigidbody.transform.SetParent(gameObject.transform);
        }
    }

    // Gère la sortie du joueur de la plateforme
    private void OnTriggerExit(Collider other)
    {
        // Logger.Log("TriggerExit with " + other.gameObject.name);
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            // Retire le joueur de la parenté pour qu'il ne soit plus affecté par le mouvement de la plateforme
            other.attachedRigidbody.transform.SetParent(null);
        }
    }
}
