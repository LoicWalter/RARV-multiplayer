using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EndGameWaiting : MonoBehaviour
{
  [SerializeField] private ParticleSystem _CelebrateEffectLeft;
  [SerializeField] private ParticleSystem _CelebrateEffectRight;
  private AudioSource _audioSource;

  void Awake()
  {
    _audioSource = gameObject.GetComponent<AudioSource>();
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.attachedRigidbody.CompareTag("Player"))
    {

      PlayerController playerController = other.attachedRigidbody.GetComponent<PlayerController>();

      if (playerController != null)
      {
        playerController.playerInput.enabled = false;
        playerController.enabled = false;
        //FreeCamera camera = playerController.ActivateFreeCamera();

        //Active effects
        _CelebrateEffectRight.Play();
        _CelebrateEffectLeft.Play();
        _audioSource.Play();

        PlayerControllerData playerControllerData = new PlayerControllerData
        {
          clientId = playerController.OwnerClientId,
        };

        FactoryFrenzyGameManager.Instance.AddPlayerRank(playerControllerData);

        playerController.EnableFreeCameraIfExist();
      }
    }
  }

  //   private IEnumerator EnableFreeCamera(FreeCamera freeCamera)
  //   {
  //     freeCamera.enabled = false;
  //     yield return new WaitForSeconds(2);
  //     freeCamera.enabled = true;
  //   }
}

