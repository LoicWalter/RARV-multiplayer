using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoucingSystem : MonoBehaviour
{
    [SerializeField] private float bounceForce = 50;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bounce");
            var player = other.gameObject.GetComponent<IMovable>();
            
            Vector3 direction = player.Rb.velocity.normalized;
            //player.Rb.AddExplosionForce(bounceForce, other.contacts[0].point, 5);
            player.Rb.AddForce(-direction * bounceForce, ForceMode.VelocityChange);
        }
    }
}
