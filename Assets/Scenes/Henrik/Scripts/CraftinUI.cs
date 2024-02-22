using UnityEngine;

public class CraftinUI : MonoBehaviour
{
    public GameObject craftingSlot;

    private void Update()
    {
    }

    private void OnEnable()
    {
        craftingSlot.SetActive(true);
        CursorLockHandler.ShowAndUnlockCursor();
    }

    private void OnDisable()
    {
        craftingSlot.SetActive(false);
        CursorLockHandler.HideAndLockCursor();
    }

    public void OnMix()
    {
    }
}