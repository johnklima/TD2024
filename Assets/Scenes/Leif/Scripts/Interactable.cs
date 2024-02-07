using System;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    void Interact();
    // void Interact(LeifPlayerController lPC);
}

[Serializable]
public struct InteractionEvents
{
    [Tooltip("Triggered when the player enters the triggerBox")]
    public UnityEvent<Collider> onEnter;

    [Tooltip("Triggered when the player exit the triggerBox")]
    public UnityEvent<Collider> onExit;

    [Tooltip("Triggered when the player is in pickUpRadius, is aimingOn triggerBox and presses <key>")]
    public UnityEvent onRayInteract;

    [Tooltip("Triggered the first frame 'Aiming On' an interactable item")]
    public UnityEvent onAimingOn;

    [Tooltip("Triggered the first frame 'Aiming Off' an interactable item")]
    public UnityEvent onAimingOff;
}

public class Interactable : MonoBehaviour
{
    [Tooltip("Enable to test events added to the TESTER gameObject")]
    public bool useTester = true;

    [Tooltip("The layer to interact with")]
    public LayerMask interactableLayerMask;

    [Tooltip("What key to press to trigger interaction")]
    public KeyCode key = KeyCode.E;

    [Tooltip("How far can away is item pickUpAble")]
    public float pickUpRadius = 5f;

    [Tooltip("Player controller camera, defaults to: Camera.main")]
    public Camera playerCamera;

    [Tooltip("Size and position of the triggerBox")]
    public TriggerBoxData triggerBoxData = new();

    [Tooltip("Events")] public InteractionEvents interactionEvents;

    private RaycastHit _hit;
    private Transform _playerCameraTransform;
    private bool _playerInRange;


    private TriggerBox _triggerBox;

    public InteractableCamera interactableCamera { get; private set; }

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

        if (!_playerInRange) return;
        if (_playerCameraTransform) // if player is in range, draw debugRay
            Debug.DrawRay(_playerCameraTransform.position, _playerCameraTransform.forward);
        if (!Input.GetKeyDown(key)) return;
        if (!interactableCamera.TryDoRay(out var interactable)) return;
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
        //todo check for player correctly
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        //todo check for player correctly
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        _playerInRange = false;
    }


    private void OnValidate()
    {
        _triggerBox ??= GetComponentInChildren<TriggerBox>();
        triggerBoxData ??= new TriggerBoxData();
        if (triggerBoxData != null)
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
                _playerCameraTransform = playerCamera.transform;
                Debug.LogWarning($"playerCamera not set on: {gameObject.name}" +
                                 $" - using default: Camera.main: {_playerCameraTransform.name}");
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
        interactableCamera = FindObjectOfType<InteractableCamera>();
        if (interactableCamera == null)
        {
            //* if we cant find it, log error and try to get it off main camera
            Debug.LogError("Did not find <InteractableCamera>, trying to get from Camera.main!");
            interactableCamera = cam.gameObject.GetComponent<InteractableCamera>();

            if (interactableCamera == null)
            {
                //* if we cant get it of main camera, add it to main camera.
                Debug.LogError("Did not find <InteractableCamera> on Camera.Main, adding component as new!");
                interactableCamera = cam.gameObject.AddComponent<InteractableCamera>();

                if (interactableCamera == null)
                    //! if we for whatever reason STILL do not have a reference, something is wrong!
                    throw new Exception(
                        "No camera found with <InteractableCamera> script attached\n" +
                        "make sure that there is at least one camera with it!");
            }
        }

        //* set interactableLayerMask on the interactable camera
        interactableCamera.interactableLayerMask = interactableLayerMask;
        //* set the events on the camera so it can trigger the events
        interactableCamera.onAimingOn = interactionEvents.onAimingOn;
        interactableCamera.onAimingOff = interactionEvents.onAimingOff;
    }
}