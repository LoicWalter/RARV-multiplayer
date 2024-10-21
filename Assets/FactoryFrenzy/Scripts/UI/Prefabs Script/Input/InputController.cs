using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// An input controller.
  /// </summary>
  [RequireComponent(typeof(Input))]
  public class InputController : MonoBehaviour
  {
    [Header("Events")]
    [Tooltip("The event to raise when the text changed.")]
    [SerializeField] private GameEvent _onChangeEvent;

    private Input _input;

    private void Awake()
    {
      _input = GetComponent<Input>();
    }

    /// <summary>
    /// This method sets the text of the input component.
    /// It can be called using a game event listener.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>
    public void OnValueChanged(Component sender, object data)
    {
      if (data is int intValue)
      {
        _input.SetText(intValue.ToString());
      }
      else if (data is float floatValue)
      {
        _input.SetText(floatValue.ToString());
      }
      else if (data is string stringValue)
      {
        _input.SetText(stringValue);
      }
    }

    /// <summary>
    /// This method raises the onChange event when the text changes.
    /// </summary>
    /// <param name="value">
    ///   The new text value.
    /// </param>
    public void OnTextChanged(string value)
    {
      _onChangeEvent.Raise(this, value);
    }
  }
}