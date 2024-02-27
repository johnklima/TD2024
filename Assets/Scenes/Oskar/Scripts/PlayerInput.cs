using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static bool playerHasControl = true;
    public UnityEvent onEscapePressed;

    public Vector2 moveDir;
    private PauseMenu _pauseMenu;
    private PlayerController _playerController;
    private InputActions _playerInput;


    private void Awake()
    {
        _playerInput = new InputActions();
        _playerController = GetComponent<PlayerController>();

        _pauseMenu = FindObjectOfType<PauseMenu>();
        if (_pauseMenu == null) throw new Exception("Make sure there is a <PauseMenu> in the scene!");

        _playerInput.Player.Interact.performed += HandleInteract;
        // _playerInput.UI.Cancel.performed += HandleCancelUI;
        _playerInput.Player.Cancel.performed += HandleCancelUI;
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
        CursorLockHandler.ShowAndUnlockCursor();
    }


    public void SetPlayerInputState(bool enable)
    {
        if (enable) _playerInput.Player.Enable();
        else _playerInput.Player.Disable();
    }

    public void DisablePlayerInputForDuration(float duration)
    {
        _playerInput.Player.Disable();
        StartCoroutine(ReactivatePlayerInputDelayed(duration));
    }

    private IEnumerator ReactivatePlayerInputDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerInput.Player.Enable();
    }

    public void EnablePlayerControls()
    {
        _playerInput.UI.Disable();
        _playerInput.Player.Enable();
        playerHasControl = true;
        Debug.Log("Player controls enabled.");
    }

    public void EnableUIControls()
    {
        _playerInput.Player.Disable();
        _playerInput.UI.Enable();
        playerHasControl = false;
        Debug.Log("UI controls enabled.");
    }

    private void HandleCancelUI(InputAction.CallbackContext context)
    {
        // CancelCauldron.Invoke();
        // cauldron has own button now
        _pauseMenu.TogglePauseMenu();
        onEscapePressed.Invoke();
        // Escape pressed, show pause menu
    }


    private void HandleMovement()
    {
        if (_playerController == null) return;
        //Takes the X and Y from the Vector2 and sends it to the player controller that handles player movement.
        moveDir = _playerInput.Player.Move.ReadValue<Vector2>();
        _playerController.Move(moveDir);
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        _playerController.Interact();
    }
}