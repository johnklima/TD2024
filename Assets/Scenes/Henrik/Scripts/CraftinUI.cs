using UnityEngine;

public class CraftinUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnDisable()
    {
        CursorLockHandler.LockCursor();
    }

    private void OnEnable()
    {
        CursorLockHandler.UnLockCursor();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
