using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlagColor))]
public class Checkpoint : MonoBehaviour
{
  // [SerializeField] private MeshRenderer _meshRenderer;
  private List<PlayerController> _listPlayerActive = new();
  private AudioSource _audioSource;

  void Start()
  {
    _audioSource = gameObject.GetComponent<AudioSource>();
  }

  // private Material _materialActive;

  // private void Awake()
  // {
  //   _materialActive = new Material(_meshRenderer.material);
  //   _meshRenderer.material = _materialActive;
  // }

  void OnTriggerEnter(Collider other)
  {
    if (other.attachedRigidbody.CompareTag("Player"))
    {
      var playerController = other.attachedRigidbody.GetComponent<PlayerController>();

      if (_listPlayerActive.Contains(playerController)) return;

      //New respawn position
      playerController.RespawnPos = transform.position;
      _listPlayerActive.Add(playerController);

      //Audio
      _audioSource.Play();

      //change color Active checkpoint
      //_materialActive.color = Color.green;
      //_materialActive.SetColor("_EmissionColor", Color.green);
    }
  }
}
