using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// A text controller.
  /// </summary>
  [RequireComponent(typeof(Text))]
  public class TextController : MonoBehaviour
  {
    private Text _text;

    private void Awake()
    {
      _text = GetComponent<Text>();
    }

    /// <summary>
    /// This method sets the text of the text component.
    /// It can be called using a game event listener.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>
    public void OnValueChanged(Component sender, object data)
    {
      if (data is int intValue)
      {
        _text.SetText(intValue.ToString());
      }
      else if (data is float floatValue)
      {
        _text.SetText(floatValue.ToString());
      }
      else if (data is string stringValue)
      {
        _text.SetText(stringValue);
      }
    }
  }
}