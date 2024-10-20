using System.Collections;
using System.Collections.Generic;
using FactoryFrenzy.Enums;
using UnityEngine;

namespace FactoryFrenzy.UI
{
  /// <summary>
  /// Defines the different styles in the theme.
  /// </summary>
  [CreateAssetMenu(fileName = "Theme", menuName = "Custom UI/Theme")]
  public class ThemeSO : ScriptableObject
  {
    [Header("Colors")]
    [Header("Primary Colors")]
    [Tooltip("The primary background color.")]
    public Color PrimaryBackground;
    [Tooltip("The primary text color.")]
    public Color PrimaryText;

    [Header("Secondary Colors")]
    [Tooltip("The secondary background color.")]
    public Color SecondaryBackground;
    [Tooltip("The secondary text color.")]
    public Color SecondaryText;

    [Header("Tertiary Colors")]
    [Tooltip("The tertiary background color.")]
    public Color TertiaryBackground;
    [Tooltip("The tertiary text color.")]
    public Color TertiaryText;

    [Header("Other Colors")]
    [Tooltip("The disabled color.")]
    public Color Disabled;

    /// <summary>
    /// Get the background color for the given style.
    /// </summary>
    /// <param name="style">
    ///   The theme style to get the background color for.
    /// </param>
    /// <returns>
    /// The background color for the given style. Defaults to the primary background color.
    /// </returns>
    public Color GetBackgroundColor(Style style)
    {
      return style switch
      {
        Style.Primary => PrimaryBackground,
        Style.Secondary => SecondaryBackground,
        Style.Tertiary => TertiaryBackground,
        Style.Disabled => Disabled,
        _ => PrimaryBackground,
      };
    }

    /// <summary>
    /// Get the text color for the given style.
    /// </summary>
    /// <param name="style">
    ///   The theme style to get the text color for.
    /// </param>
    /// <returns>
    /// The text color for the given style. Defaults to the primary text color.
    /// </returns>
    public Color GetTextColor(Style style)
    {
      return style switch
      {
        Style.Primary => PrimaryText,
        Style.Secondary => SecondaryText,
        Style.Tertiary => TertiaryText,
        Style.Disabled => Disabled,
        _ => PrimaryText,
      };
    }
  }
}