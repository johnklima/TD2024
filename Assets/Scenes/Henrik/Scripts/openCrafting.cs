using UnityEngine;

public class openCrafting : MonoBehaviour
{
    public GameObject caldron;
    public GameObject craftingGrid;
    public GameObject mix;
    public GameObject camera;
    public GameObject Slot1;
    public GameObject Slot2;
    private MouseCamLook mouseCamLook;

    private void Start()
    {
        // start mix button as disabled
        // un-disable mix button only when 2nd material is slotted
        caldron.SetActive(false);
        craftingGrid.SetActive(false);
        mix.SetActive(false);
        Slot1.SetActive(false);
        Slot2.SetActive(false);
        mouseCamLook = camera.GetComponent<MouseCamLook>();
    }

    // Update is called once per frame
    private void Update()

    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var state = !mix.activeSelf;
            caldron.SetActive(state);
            craftingGrid.SetActive(state);
            mix.SetActive(state);
            Slot1.SetActive(state);
            Slot2.SetActive(state);
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