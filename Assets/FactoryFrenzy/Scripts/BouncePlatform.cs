using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    [SerializeField] private float bounceForce = 4;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("BouncePlatform");
            var player = other.gameObject.GetComponent<IMovable>();
            
            player.Rb.AddForce(Vector3.up * bounceForce, ForceMode.VelocityChange);
        }
    }

}
