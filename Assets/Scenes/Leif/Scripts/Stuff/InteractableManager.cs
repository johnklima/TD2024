using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private static InteractableManager _instance;
    public LayerMask interactableLayerMask;
    public List<InteractionEvents> interactionEvents = new();
    public Camera playerCamera;
    private Transform _playerCameraTransform;

    public static InteractableManager instance
    {
        get
        {
            if (_instance != null) return _instance;
            var newManager = new GameObject("-- InteractableManager --");
            _instance = newManager.AddComponent<InteractableManager>();
            return _instance;
        }
        private set => _instance = value;
    }


    public static InteractableCamera interactableCamera { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this);
        else _instance = this;
        ValidatePlayerCamera();
    }

    private void Update()
    {
        if (_playerCameraTransform) // if player is in range, draw debugRay
            Debug.DrawRay(_playerCameraTransform.position, _playerCameraTransform.forward);
    }

    public InteractableCamera Register(Interactable interactable)
    {
        instance.interactableLayerMask = interactable.interactableLayerMask;


        instance.interactionEvents.Add(interactable.interactionEvents);
        ValidatePlayerCamera();
        return interactableCamera;
    }

    private void ValidatePlayerCamera()
    {
        //* if playerCamera is not set in inspector manually,
        if (playerCamera == null)
        {
            //* try to get it via Camera.main
            playerCamera = Camera.main;
            if (playerCamera == null)
                //* did not get any camera, throw error!
                throw new Exception("Could not find Camera.main, make sure to either:\n" +
                                    "Set a camera's tag as Main, or\n" +
                                    "Assign a camera to <PlayerCamera> on: " + gameObject.name);
            //* if we get camera, set transform
            _playerCameraTransform = playerCamera.transform;
            Debug.LogWarning($"playerCamera not set on: {gameObject.name}" +
                             $" - using default: Camera.main: {_playerCameraTransform.name}");
        }

        ValidateInteractableCamera();
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
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        interactionEvents.ForEach(x =>
        {
            var on = x.onAimingOn;
            var off = x.onAimingOff;
            if (on.GetPersistentEventCount() > 0 && !interactableCamera.onAimingOn.Contains(on))
                interactableCamera.onAimingOn.Add(on);
            if (off.GetPersistentEventCount() > 0 && !interactableCamera.onAimingOff.Contains(off))
                interactableCamera.onAimingOff.Add(off);
        });
    }
}