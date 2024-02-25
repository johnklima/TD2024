using UnityEngine;

public class CameraDisabler : MonoBehaviour
{
    private void Start()
    {
        var asd = FindObjectOfType<MainMenu>();
        if (asd != null) // we have been loaded additively, disable this cam
        {
            Debug.Log("Loaded additively, disabling player cam");
            if (!TryGetComponent(out Camera cam))
            {
                gameObject.SetActive(false);
                return;
            }

            cam.enabled = false;
        }
    }

    private void Update()
    {
    }
}