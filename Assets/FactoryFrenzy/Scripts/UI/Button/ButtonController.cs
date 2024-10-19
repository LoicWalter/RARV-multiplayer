using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// A button controller.
  /// This class is a bridge between the button and the event system.
  /// </summary>
  [RequireComponent(typeof(CustomButton))]
  public class ButtonController : MonoBehaviour
  {

    [Header("Events")]
    [Tooltip("The event to raise when the button is clicked.")]
    [SerializeField] private GameEvent _onClickEvent;
    [Tooltip("The data to send when the button is clicked.")]
    [SerializeField] private string data;
    [Tooltip("The data type of the data.")]
    [SerializeField] private bool IsFloatValue = false;

    private CustomButton button;

    private void Awake()
    {
      button = GetComponent<CustomButton>();
    }

    private void OnEnable()
    {
      button.OnClickEvent.AddListener(OnClick);
    }

    private void OnDisable()
    {
      button.OnClickEvent.RemoveListener(OnClick);
    }

    public void OnClick()
    {
      if (IsFloatValue && float.TryParse(data, out float floatValue))
      {
        _onClickEvent.Raise(this, floatValue);
        return;
      }
      _onClickEvent.Raise(this, data);
    }
  }
}