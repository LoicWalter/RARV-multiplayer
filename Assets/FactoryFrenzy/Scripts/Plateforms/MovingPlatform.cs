using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
  [SerializeField]
  private GameObject _movingPlatform; // Plateforme mobile

  [SerializeField]
  private Transform _pointA; // Point de départ

  [SerializeField]
  private Transform _pointB; // Point d'arrivée

  [SerializeField]
  private float speed = 3f; // Vitesse de la plateforme
  private Vector3 targetPosition; // Position cible actuelle
  private bool goingToB = true; // Savoir si la plateforme va vers B

  private void Start()
  {
    Init();
  }

  public void SetPoints(Vector3 pointA, Vector3 pointB)
  {
    _pointA.position = pointA;
    _pointB.position = pointB;
    Init();
  }

  private void Init()
  {
    _movingPlatform.transform.position = (_pointA.position + _pointB.position) / 2f;
    targetPosition = _pointB.position;
    RotatePlatform();
  }

  private void Update()
  {
    MovePlatform();
  }

  private void OnValidate()
  {
    _movingPlatform.transform.position = (_pointA.position + _pointB.position) / 2f;
  }

  private void MovePlatform()
  {
    // Déplace la plateforme vers la position cible
    _movingPlatform.transform.position = Vector3.MoveTowards(
        _movingPlatform.transform.position,
        targetPosition,
        speed * Time.deltaTime
    );

    // Vérifie si la plateforme est arrivée à destination
    if (Vector3.Distance(_movingPlatform.transform.position, targetPosition) < 0.1f)
    {
      // Change la direction de la plateforme
      if (goingToB)
      {
        targetPosition = _pointB.position;
      }
      else
      {
        targetPosition = _pointA.position;
      }
      goingToB = !goingToB; // Inverse la direction
    }
  }

  private void RotatePlatform()
  {
    // Calcul de la direction de mouvement
    Vector3 direction = targetPosition - _movingPlatform.transform.position;

    // Calcul de la rotation pour être perpendiculaire au mouvement
    // Supposons que l'axe Y est l'axe de rotation principal
    Quaternion targetRotation = Quaternion.LookRotation(direction);

    // Ajuste la rotation à 90° par rapport à l'axe de déplacement
    _movingPlatform.transform.rotation = Quaternion.Euler(
        0,
        targetRotation.eulerAngles.y + 90f,
        0
    );
  }

  // Gère l'entrée du joueur sur la plateforme
  private void OnCollisionEnter(Collision other)
  {
    Logger.Log("CollisionEnter with " + other.gameObject.name);
    if (other.rigidbody.CompareTag("Player"))
    {
      // Parent le joueur à la plateforme pour qu'il bouge avec elle
      other.rigidbody.transform.SetParent(_movingPlatform.transform);
    }
  }

  // Gère la sortie du joueur de la plateforme
  private void OnTriggerExit(Collider other)
  {
    Logger.Log("TriggerExit with " + other.gameObject.name);
    if (other.attachedRigidbody.CompareTag("Player"))
    {
      // Retire le joueur de la parenté pour qu'il ne soit plus affecté par le mouvement de la plateforme
      other.attachedRigidbody.transform.SetParent(null);
    }
  }
}
