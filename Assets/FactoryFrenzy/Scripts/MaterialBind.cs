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
    private void OnValidate()
    {
        BindMaterial();
    }

    // Fonction pour bind le matériel
    void BindMaterial()
    {
        if (_gameObjectToBindWith == null)
        {
            Logger.LogWarning($"Game Object To Bind With est null {gameObject.name}");
            return;
        }

        // Vérifie si _gameObjectToBindWith n'est pas null et a un Renderer
        if (_gameObjectToBindWith.TryGetComponent(out Renderer targetRenderer))
        {
            // Vérifie si l'objet actuel a également un Renderer
            if (gameObject.TryGetComponent(out Renderer currentRenderer))
            {
                // Crée un nouvel instance du matériau pour ne pas affecter le matériau partagé
                currentRenderer.material = new Material(targetRenderer.sharedMaterial);
            }
            else
            {
                Logger.LogWarning("Pas de Renderer sur l'objet actuel");
            }
        }
        else
        {
            Logger.LogWarning("Game Object To Bind With n'a pas de Renderer");
        }
    }
}
