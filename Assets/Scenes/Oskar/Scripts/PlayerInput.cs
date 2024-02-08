using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public TBD_CameraController cameraController;
    private PlayerController _playerController;
    private InputActions _playerInput;

    private void Awake()
    {
        _playerInput = new InputActions();
        _playerController = GetComponent<PlayerController>();

        _playerInput.Interactions.Interact.performed += HandleInteract;
    }

    private void Update()
    {
        //Read these values at every frame rather than reading it at button press.
        HandleCameraMovement();
        HandleMovement();
    }

    private void OnEnable()
    {
        _playerInput.Movement.Enable();
        _playerInput.Interactions.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Movement.Disable();
        _playerInput.Interactions.Disable();
    }

    private void HandleCameraMovement()
    {
        if (cameraController == null) return;
        //Takes the mouse delta and sends it to the camera controller that handles rotation
        var mousePos = _playerInput.Movement.LookPos.ReadValue<Vector2>();
        cameraController.RotateCamera(mousePos);
    }

    private void HandleMovement()
    {
        if (_playerController == null) return;
        //Takes the X and Y from the Vector2 and sends it to the player controller that handles player movement.
        var moveDir = _playerInput.Movement.Move.ReadValue<Vector2>();
        _playerController.Move(moveDir);
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        Debug.Log("pressed button");
        _playerController.Interact();
    }
}