using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
  [SerializeField] private float bounceForce = 3;
  private AudioSource _audioSource;

  void Start(){
    _audioSource = gameObject.GetComponent<AudioSource>();
  }

  void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Player"))
    {
      var player = other.gameObject.GetComponent<IPlayerMovable>();
      
      //Audio
      _audioSource.Play();

      Vector3 bounceDirection = other.contacts[0].normal;
      player.Rb.AddForce(-bounceDirection * bounceForce, ForceMode.Impulse);
    }
  }
}
