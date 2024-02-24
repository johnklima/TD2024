using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CauldronControllerHenrik : MonoBehaviour, IInteractable
{
    public CinemachineVirtualCamera cauldronCamera;
    public GameObject cauldronUI;

    public UnityEvent onCauldronEnter;
    public UnityEvent onCauldronExit;

    public void Interact()
    {
        Debug.Log("interacting with controller");
        if (cauldronCamera && cauldronUI)
        {
            cauldronCamera.Priority = 20;
            cauldronUI.SetActive(true);

            onCauldronEnter.Invoke();
        }
    }

    public void Interact(LeifPlayerController lPC)
    {
        throw new System.NotImplementedException();
    }

    public void CancelCauldronMode()
    {
        cauldronCamera.Priority = 5;
        cauldronUI.SetActive(false);

        onCauldronExit.Invoke();

    }
}
