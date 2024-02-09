using UnityEngine;

public class openCrafting : MonoBehaviour
{
    public GameObject caldron;
    public GameObject craftingGrid;
    public GameObject mix;
    public GameObject cameraGameObject;
    public GameObject Slot1;
    public GameObject Slot2;
    private MouseCamLook mouseCamLook;

    private void Start()
    {
        // start mix button as disabled
        // un-disable mix button only when 2nd material is slotted

        mix.SetActive(false);

        mouseCamLook = cameraGameObject.GetComponent<MouseCamLook>();
    }

    // Update is called once per frame
    private void Update()

    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var state = !mix.activeSelf;
            mix.SetActive(state);

            if (state)
            {
                Cursor.lockState = CursorLockMode.None;
                mouseCamLook.enabled = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                mouseCamLook.enabled = true;
            }
        }
    }
}