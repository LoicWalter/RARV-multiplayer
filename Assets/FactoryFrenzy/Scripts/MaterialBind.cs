using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBind : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameObjectToBindWith;

    // Start is called before the first frame update
    void Start()
    {
        BindMaterial();
    }

    // Permet au code de s'exécuter même lorsque des modifications sont faites dans l'éditeur
    [ExecuteInEditMode]
    void OnValidate()
    {
        BindMaterial();
    }

    // Fonction pour bind le matériel
    void BindMaterial()
    {
        // Vérifie si _gameObjectToBindWith n'est pas null et a un Renderer
        if (_gameObjectToBindWith != null && _gameObjectToBindWith.GetComponent<Renderer>() != null)
        {
            Renderer targetRenderer = _gameObjectToBindWith.GetComponent<Renderer>();

            // Vérifie si l'objet actuel a également un Renderer
            if (this.GetComponent<Renderer>() != null)
            {
                Renderer currentRenderer = this.GetComponent<Renderer>();

                // Crée un nouvel instance du matériau pour ne pas affecter le matériau partagé
                currentRenderer.material = new Material(targetRenderer.sharedMaterial);
            }
            else
            {
                Debug.LogWarning("Pas de Renderer sur l'objet actuel");
            }
        }
        else
        {
            Debug.LogWarning("Game Object To Bind With est null ou n'a pas de Renderer");
        }
    }
}
