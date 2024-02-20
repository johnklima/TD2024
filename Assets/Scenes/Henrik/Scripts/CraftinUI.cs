using UnityEngine;

public class CraftinUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnDisable()
    {
        CursorLockHandler.HideAndLockCursor();
    }

    private void OnEnable()
    {
        CursorLockHandler.ShowAndUnlockCursor();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
