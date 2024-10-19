using TMPro;
using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// Defines an input object and its properties.
  /// </summary>
  [CreateAssetMenu(fileName = "Input", menuName = "Custom UI/Input")]
  public class InputSO : ScriptableObject
  {
    [Header("Font")]
    [Tooltip("The font to use for the text.")]
    public TMP_FontAsset Font;
    [Tooltip("The size of the font.")]
    public int FontSize;
  }
}