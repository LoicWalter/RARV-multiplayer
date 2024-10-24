using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flagged : MonoBehaviour
{
    [SerializeField]
    private Material _CheckpointFlagged; // Matériau à appliquer lorsque le joueur touche le drapeau

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est bien le joueur qui entre dans le trigger
        {
            Renderer flagRenderer = this.gameObject.transform.GetChild(0).GetComponent<Renderer>();

            if (flagRenderer != null && _CheckpointFlagged != null)
            {
                // Change le matériau du drapeau au matériau spécifié
                flagRenderer.material = _CheckpointFlagged;
            }
        }
    }
}
