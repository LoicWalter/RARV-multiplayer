using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineSnapper : MonoBehaviour
{
    [SerializeField]
    private Transform _startPoint; // Premier objet (départ de la ligne)

    [SerializeField]
    private Transform _endPoint; // Deuxième objet (fin de la ligne)

    private LineRenderer _lineRenderer;

    private void OnValidate()
    {
        InitializeLineRenderer();
        UpdateLineRenderer();
    }

    private void Awake()
    {
        InitializeLineRenderer();
    }

    private void Update()
    {
        UpdateLineRenderer();
    }

    private void InitializeLineRenderer()
    {
        // Récupère le Line Renderer attaché à l'objet
        _lineRenderer = GetComponent<LineRenderer>();

        // Définit le nombre de positions (2 pour une ligne simple)
        _lineRenderer.positionCount = 2;
    }

    private void UpdateLineRenderer()
    {
        // Met à jour les positions du Line Renderer à chaque frame
        if (_startPoint != null && _endPoint != null)
        {
            _lineRenderer.SetPosition(0, _startPoint.position); // Position de départ (startPoint)
            _lineRenderer.SetPosition(1, _endPoint.position); // Position de fin (endPoint)
        }
    }
}
