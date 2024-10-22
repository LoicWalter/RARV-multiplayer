using UnityEditor;

public class LockInspector
{
    [MenuItem("Edit/Lock Inspector %l")]
    public static void Lock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    [MenuItem("Edit/Lock Inspector %l", isValidateFunction: true)]
    public static bool Valid()
    {
        return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
    }
}
