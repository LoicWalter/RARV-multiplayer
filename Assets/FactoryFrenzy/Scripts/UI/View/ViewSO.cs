using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// Defines a view object and its properties.
  /// </summary>
  [CreateAssetMenu(fileName = "View", menuName = "Custom UI/View")]
  public class ViewSO : ScriptableObject
  {
    [Header("Layout")]
    [Tooltip("The padding of the view.")]
    public RectOffset padding;
    [Tooltip("The spacing between elements in the view.")]
    public float spacing;
  }
}