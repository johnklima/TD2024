using UnityEngine;

public interface I_Interactable
{
    void Interact();
    // void Interact(LeifPlayerController lPC);
}

public class Interactable : MonoBehaviour
{
    //todo raycast: events: mouseOver, mouseClick, mouseExit
    public float pickUpRadius = 2.5f;
    public bool useTriggerBox, useRayCast;
    public Camera playerCamera;
    public TriggerBoxData triggerBoxData = new();
    private TriggerBox _triggerBox;
    private Transform playerCameraTransform;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            playerCameraTransform = playerCamera.transform;
            Debug.LogWarning($"playerCamera not set - using default: Camera.main: {playerCamera.transform.name}");
        }
    }


    private void Update()
    {
        if (!useRayCast) return;
        if (playerCameraTransform)
            Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward);
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.DrawWireSphere(position, pickUpRadius);
        var pos = position + triggerBoxData.localPos;
        if (useTriggerBox)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(pos, triggerBoxData.size);
        }
    }


    private void OnValidate()
    {
        _triggerBox ??= GetComponentInChildren<TriggerBox>();
        triggerBoxData ??= new TriggerBoxData();

        _triggerBox.UpdateTriggerBox(triggerBoxData);
    }
}