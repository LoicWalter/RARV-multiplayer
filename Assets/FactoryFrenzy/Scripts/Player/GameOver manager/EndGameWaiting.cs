using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Rendering;

public class EndGameWaiting : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.transform.root.CompareTag("Player")){

            PlayerController playerController = other.transform.root.GetComponent<PlayerController>();
            
            if(playerController != null){
                playerController.playerInput.enabled = false;
                playerController.enabled = false;
                playerController.freeCamera.SetActive(true);

                FactoryFrenzyGameManager.Instance.AddPlayerRank(playerController);

                StartCoroutine(EnableFreeCamera(playerController.freeCamera));
            }
        }
    }

    private IEnumerator EnableFreeCamera(GameObject freeCamera)
    {
        FreeCamera camera = freeCamera.GetComponent<FreeCamera>();

        camera.enabled = false;
        yield return new WaitForSeconds(2);
        camera.enabled = true;
    }
}