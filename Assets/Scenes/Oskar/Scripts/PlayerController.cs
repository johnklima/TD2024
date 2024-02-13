using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    //TBD:
    // Health interactions
    // State changes for animations (walking/running/whatever)
    // Jumping/Crouching movements
    
    private CharacterController ctrl;
    
    public float movementSpeed = 5f;

    public Transform playerCamera;

    private bool _shouldRayCast = true;
  
    
    [Header("Interactable controls")]

    public float interactableDistance = 10f;
    
    public LayerMask interactLayerMask;
    
    public UnityEvent hittingInteractable;
    
    private bool _isCurrentlyHittingInteractable = false;
    
    private IInteractable _targetedItem;
    
    
    private void Awake()
    {
        ctrl = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleRayCasting()
    {
        _shouldRayCast = !_shouldRayCast; // Toggle the value

    }
    public void Move(Vector2 movementDir)
    {
        Vector3 cameraForward = playerCamera.forward;
        Vector3 cameraRight = playerCamera.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the direction relative to the camera's orientation
        Vector3 moveDirection = (cameraForward * movementDir.y + cameraRight * movementDir.x).normalized;

        // Move the player in the calculated direction
        if (ctrl != null)
        {
            ctrl.Move(moveDirection * movementSpeed * Time.deltaTime);
            ctrl.Move(Physics.gravity * Time.deltaTime);
        }
        // Rotate the player to face the direction of movement
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * movementSpeed);
        }
    }

    public void Interact()
    {
        if (_isCurrentlyHittingInteractable && _targetedItem != null)
        {
            Debug.Log("pc interact");
            Debug.Log(_targetedItem);

            _targetedItem.Interact();
        }
        
    }
    
    void Update()
    {
        RaycastHit hit;
        bool currentlyHittingInteractable = false;
        if (_shouldRayCast)
        {
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactableDistance, interactLayerMask))
            {
                Debug.DrawRay(playerCamera.position, (playerCamera.forward.normalized * interactableDistance), Color.red);

                // Check if hit is interactable
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    currentlyHittingInteractable = true;
                    
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                    // Update the targeted item if hitting a new interactable
                    if (_targetedItem != interactable)
                    {
                        _targetedItem = interactable;
                    }
                }
                else
                {
                    _targetedItem = null;
                }
            }
            else
            {
                // Not hitting anything, clearing the variable so you cant interact
                _targetedItem = null;
            }

            // Check if there's been a change in interaction state
            if (currentlyHittingInteractable != _isCurrentlyHittingInteractable)
            {
                _isCurrentlyHittingInteractable = currentlyHittingInteractable;
                // Invoke the event to toggle the crosshair state
                hittingInteractable.Invoke();
            }
        }
    }
}
