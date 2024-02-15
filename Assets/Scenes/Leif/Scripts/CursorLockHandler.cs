using UnityEngine;

public static class CursorLockHandler
{
    public static CursorLockMode GetCursorLockMode()
    {
        return Cursor.lockState;
    }


    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void UnLockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public static void HideCursor()
    {
        Cursor.visible = false;
    }

    public static void HideAndLockCursor()
    {
        LockCursor();
        HideCursor();
    }

    public static void ShowCursor()
    {
        Cursor.visible = true;
    }

    public static void ShowAndUnlockCursor()
    {
        UnLockCursor();
        ShowCursor();
    }
}