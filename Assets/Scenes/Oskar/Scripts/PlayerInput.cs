using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent CancelCauldron;

    //public TBD_CameraController cameraController;
    private PlayerController _playerController;
    private InputActions _playerInput;


    private void Awake()
    {
        _playerInput = new InputActions();
        _playerController = GetComponent<PlayerController>();

        _playerInput.Player.Interact.performed += HandleInteract;
        _playerInput.UI.Cancel.performed += HandleCancelUI;
    }

    private void Update()
    {
        //Read these values at every frame rather than reading it at button press.
        //HandleCameraMovement();
        HandleMovement();
    }

    private void OnEnable()
    {
        _playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }

    public void EnablePlayerControls()
    {
        _playerInput.UI.Disable();
        _playerInput.Player.Enable();
        Debug.Log("Player controls enabled.");
    }

    public void EnableUIControls()
    {
        _playerInput.Player.Disable();
        _playerInput.UI.Enable();
        Debug.Log("UI controls enabled.");
    }

    private void HandleCancelUI(InputAction.CallbackContext context)
    {
        CancelCauldron.Invoke();
    }


    private void HandleMovement()
    {
        if (_playerController == null) return;
        //Takes the X and Y from the Vector2 and sends it to the player controller that handles player movement.
        var moveDir = _playerInput.Player.Move.ReadValue<Vector2>();
        _playerController.Move(moveDir);
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        _playerController.Interact();
    }
}