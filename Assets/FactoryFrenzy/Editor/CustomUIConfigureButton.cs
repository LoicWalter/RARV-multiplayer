using UnityEditor;
using UnityEngine;
using FactoryFrenzy.UI;

[CustomEditor(typeof(CustomUIComponent), editorForChildClasses: true), CanEditMultipleObjects]
public class CustomUIConfigureButton : Editor
{
  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();
    CustomUIComponent _customUIComponent = (CustomUIComponent)target;

    if (GUILayout.Button("Configure"))
    {
      _customUIComponent.Init();
    }
  }
}