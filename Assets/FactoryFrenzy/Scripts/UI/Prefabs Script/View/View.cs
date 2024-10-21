using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// View component that holds the layout of the UI.
  /// </summary>
  [RequireComponent(typeof(VerticalLayoutGroup))]
  [DisallowMultipleComponent]
  public class View : CustomUIComponent
  {
    [Header("Data")]
    [Tooltip("The data for the view.")]
    public ViewSO viewData;

    [Header("Containers")]
    [Tooltip("The top container.")]
    [SerializeField] private GameObject _containerTop;
    [Tooltip("The center container.")]
    [SerializeField] private GameObject _containerCenter;
    [Tooltip("The bottom container.")]
    [SerializeField] private GameObject _containerBottom;

    private Image _imageTop;
    private Image _imageCenter;
    private Image _imageBottom;

    private VerticalLayoutGroup _verticalLayoutGroup;

    public override void Setup()
    {
      _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
      _imageTop = _containerTop.GetComponent<Image>();
      _imageCenter = _containerCenter.GetComponent<Image>();
      _imageBottom = _containerBottom.GetComponent<Image>();
    }

    public override void Configure()
    {
      _verticalLayoutGroup.padding = viewData.padding;
      _verticalLayoutGroup.spacing = viewData.spacing;

      if (Theme == null) return;
      _imageTop.color = Theme.PrimaryBackground;
      _imageCenter.color = Theme.SecondaryBackground;
      _imageBottom.color = Theme.TertiaryBackground;
    }
  }
}
