using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public interface I_Interactable
{
    void Interact();
    // void Interact(LeifPlayerController lPC);
}

public class InteractableCamera : MonoBehaviour
{
    [DoNotSerialize] public LayerMask interactableLayerMask;
    //todo raycast: events: mouseOver, mouseExit

    public UnityEvent onAimingOn, onAimingOff;
    private RaycastHit hit;
    private bool wasAimingAtLastFrame;

    private void Update()
    {
        DoRay();
    }

    private void DoRay()
    {
        var ray = new Ray(transform.position, transform.forward);
        if (!Physics.Raycast(ray, out hit, 1000, interactableLayerMask)) return;

        if (hit.transform.gameObject.TryGetComponent(out I_Interactable iInteractable))
        {
            // trigger the event if:
            // we are aiming at I_Interactable, and
            // we were not aiming at a interactable last frame
            if (!wasAimingAtLastFrame) onAimingOn?.Invoke();
            wasAimingAtLastFrame = true;
        }
        else if (wasAimingAtLastFrame)
        {
            // not hitting I_Interactable this frame == aiming off
            onAimingOff?.Invoke();
            wasAimingAtLastFrame = false;
        }
    }

    public bool TryDoRay(out I_Interactable interactable)
    {
        interactable = null;
        var ray = new Ray(transform.position, transform.forward);
        if (!Physics.Raycast(ray, out hit, 1000, interactableLayerMask)) return false;
        if (!hit.transform.gameObject.TryGetComponent(out I_Interactable iInteractable)) return false;
        interactable = iInteractable;
        return true;
    }
}

public class Interactable : MonoBehaviour
{
    public UnityEvent testAimingOn, testAimingOff;

    public float pickUpRadius = 5f;
    public bool useTriggerBox, useRayCast, useKey;
    public Camera playerCamera;
    public TriggerBoxData triggerBoxData = new();
    public LayerMask interactableLayerMask;
    public KeyCode key = KeyCode.E;

    private InteractableCamera _interactableCamera;

    private TriggerBox _triggerBox;
    private RaycastHit hit;

    private Transform playerCameraTransform;

    private bool playerInRange;

    private void Awake()
    {
        ValidateInteractableCamera();
        ValidateLayers();
    }

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            playerCameraTransform = playerCamera.transform;
            Debug.LogWarning($"playerCamera not set - using default: Camera.main: {playerCamera.transform.name}");
        }

        if (pickUpRadius > 0)
        {
            var sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = pickUpRadius;
        }
    }

    private void Update()
    {
        // range
        // onEnterTriggerBox
        // onExitTriggerBox
        // onRaycast (in range, press key, aim at)

        // we shoot a ray from the camera
        // it returns null or interactable
        // interactable here is TriggerBox

        // if:
        // player is in range, and
        // useKey is enabled, and
        // useRaycast is enabled, and
        // user presses <key>, and
        // interactableCamera is looking at <I_Interactable>
        // then:
        // Interact() with interactable (TriggerBox)

        if (!playerInRange) return;
        if (playerCameraTransform) // if player is in range, draw debugRay
            Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward);
        if (!useKey) return;
        if (!useRayCast) return;
        if (!Input.GetKeyDown(key)) return;
        if (!_interactableCamera.TryDoRay(out var interactable)) return;
        interactable.Interact();
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        playerInRange = false;
    }


    private void OnValidate()
    {
        _triggerBox ??= GetComponentInChildren<TriggerBox>();
        triggerBoxData ??= new TriggerBoxData();
        _triggerBox.UpdateTriggerBox(triggerBoxData);
    }

    private void ValidateLayers()
    {
        gameObject.layer = 2;
        foreach (Transform tr in transform)
            tr.gameObject.layer = 2;
        _triggerBox.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    private void ValidateInteractableCamera()
    {
        var cam = Camera.main;
        if (cam == null) throw new Exception("Did not find Camera.main");

        _interactableCamera = FindObjectOfType<InteractableCamera>();
        if (_interactableCamera == null)
        {
            Debug.LogError("Did not find <InteractableCamera>, trying to get from Camera.main!");
            _interactableCamera = cam.gameObject.GetComponent<InteractableCamera>();

            if (_interactableCamera == null)
            {
                Debug.LogError("Did not find <InteractableCamera> on Camera.Main, adding component as new!");
                _interactableCamera = cam.gameObject.AddComponent<InteractableCamera>();

                if (_interactableCamera == null)
                    throw new Exception(
                        "No camera found with <InteractableCamera> script attached\n" +
                        "make sure that there is at least one camera with it!");
            }
        }

        _interactableCamera.interactableLayerMask = interactableLayerMask;
        _interactableCamera.onAimingOn = testAimingOn;
        _interactableCamera.onAimingOff = testAimingOff;
    }

    // private I_Interactable DoRay()
    // {
    //     var cameraTransform = _interactableCamera.transform;
    //     var ray = new Ray(cameraTransform.position, cameraTransform.forward);
    //     if (!Physics.Raycast(ray, out hit, 1000, interactableLayerMask)) return null;
    //     if (hit.transform.gameObject.TryGetComponent(out I_Interactable interactable))
    //         return interactable;
    //     return null;
    // }
}