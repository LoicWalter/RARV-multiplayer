using TMPro;
using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// Defines a text object and its properties.
  /// </summary>
  [CreateAssetMenu(fileName = "Text", menuName = "Custom UI/Text")]
  public class TextSO : ScriptableObject
  {
    [Header("Font")]
    [Tooltip("The font to use for the text.")]
    public TMP_FontAsset Font;
    [Tooltip("The size of the font.")]
    public int FontSize;
  }
}