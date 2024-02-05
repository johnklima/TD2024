using System;
using UnityEngine;
using UnityEngine.Events;

public interface I_Interactable
{
    void Interact();
    // void Interact(LeifPlayerController lPC);
}

[Serializable]
public struct InteractionEvents
{
    public UnityEvent<Collider> onEnter;
    public UnityEvent<Collider> onExit;
    public UnityEvent onRayInteract;
    public UnityEvent onAimingOn;
    public UnityEvent onAimingOff;
}

public class Interactable : MonoBehaviour
{
    public InteractionEvents interactionEvents;


    public bool useTester = true;
    public LayerMask interactableLayerMask;
    public KeyCode key = KeyCode.E;
    public float pickUpRadius = 5f;
    public Camera playerCamera;
    public UnityEvent onAimingOn = new();
    public UnityEvent onAimingOff = new();
    public TriggerBoxData triggerBoxData = new();
    private TriggerBox _triggerBox;
    private RaycastHit hit;
    private Transform playerCameraTransform;
    private bool playerInRange;

    public InteractableCamera InteractableCamera { get; private set; }

    private void Awake()
    {
        ValidatePlayerCamera();
        ValidateInteractableCamera();
        ValidateLayers();
    }

    private void Start()
    {
        //* if pickUpRadius is greater than 0 (indicating that we want to use it)
        //* add a sphereCollider and set its values
        if (pickUpRadius > 0)
        {
            var sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = pickUpRadius;
        }
    }

    private void Update()
    {
        //* we shoot a ray from the camera
        //* it returns null or interactable
        //* interactable here is TriggerBox

        //* if:
        //* player is in range, and
        //* useKey is enabled, and
        //* useRaycast is enabled, and
        //* user presses <key>, and
        //* interactableCamera is looking at <I_Interactable>
        //* then:
        //* Interact() with interactable (TriggerBox)

        if (!playerInRange) return;
        if (playerCameraTransform) // if player is in range, draw debugRay
            Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward);
        if (!Input.GetKeyDown(key)) return;
        if (!InteractableCamera.TryDoRay(out var interactable)) return;
        interactable.Interact();
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.DrawWireSphere(position, pickUpRadius);
        var pos = position + triggerBoxData.localPos;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, triggerBoxData.size);
    }

    private void OnTriggerEnter(Collider other)
    {
        //todo check for player
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //todo check for player
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        playerInRange = false;
    }


    private void OnValidate()
    {
        _triggerBox ??= GetComponentInChildren<TriggerBox>();
        triggerBoxData ??= new TriggerBoxData();
        _triggerBox.UpdateTriggerBox(this);
    }

    private void ValidatePlayerCamera()
    {
        //* if playerCamera is not set in inspector manually,
        if (playerCamera == null)
        {
            //* try to get it via Camera.main
            playerCamera = Camera.main;
            if (playerCamera != null)
            {
                //* if we get camera, set transform
                playerCameraTransform = playerCamera.transform;
                Debug.LogWarning($"playerCamera not set on: {gameObject.name}" +
                                 $" - using default: Camera.main: {playerCameraTransform.name}");
            }
            else
            {
                //* did not get any camera, throw error!
                throw new Exception("Could not find Camera.main, make sure to either:\n" +
                                    "Set a camera's tag as Main, or\n" +
                                    "Assign a camera to <PlayerCamera> on: " + gameObject.name);
            }
        }
    }

    private void ValidateLayers()
    {
        //* set all children (not grand-children) to ignore raycast
        gameObject.layer = 2;
        foreach (Transform tr in transform)
            tr.gameObject.layer = 2;
        //* set triggerBox to interactable
        _triggerBox.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    private void ValidateInteractableCamera()
    {
        //* try to get main camera
        var cam = playerCamera;
        //* if there is none, throw error
        if (cam == null) throw new Exception("Did not find <playerCamera>");

        //* try to get InteractableCamera
        InteractableCamera = FindObjectOfType<InteractableCamera>();
        if (InteractableCamera == null)
        {
            //* if we cant find it, log error and try to get it off main camera
            Debug.LogError("Did not find <InteractableCamera>, trying to get from Camera.main!");
            InteractableCamera = cam.gameObject.GetComponent<InteractableCamera>();

            if (InteractableCamera == null)
            {
                //* if we cant get it of main camera, add it to main camera.
                Debug.LogError("Did not find <InteractableCamera> on Camera.Main, adding component as new!");
                InteractableCamera = cam.gameObject.AddComponent<InteractableCamera>();

                if (InteractableCamera == null)
                    //! if we for whatever reason STILL do not have a reference, something is wrong!
                    throw new Exception(
                        "No camera found with <InteractableCamera> script attached\n" +
                        "make sure that there is at least one camera with it!");
            }
        }

        // set interactableLayerMask on the interactable camera
        InteractableCamera.interactableLayerMask = interactableLayerMask;

        InteractableCamera.onAimingOn = onAimingOn;
        InteractableCamera.onAimingOff = onAimingOff;
    }
}