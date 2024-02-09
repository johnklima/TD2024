using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //TBD:
    // Health interactions
    // State changes for animations (walking/running/whatever)
    // Jumping/Crouching movements
    // Collission/Gravity Handles
    private CharacterController ctrl;

    
    public float movementSpeed = 5f;

    public Transform playerCamera;

    public float interactableDistance = 10f;


    public LayerMask interactLayerMask;
    private bool _isHittingInteractable = false;

    private IInteractable _targetedItem;

    public InventoryController inventory;

    private void Awake()
    {
        ctrl = GetComponent<CharacterController>();
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
        if (_isHittingInteractable && _targetedItem != null)
        {
            _targetedItem.Interact();
        }
        
    }

    void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactableDistance, interactLayerMask))
        {
            Debug.DrawRay(playerCamera.position, (playerCamera.forward.normalized * interactableDistance), Color.red);
            
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Interactable"))
            {
                _isHittingInteractable = false;
                _targetedItem = null;
            }
            else
            {
                _targetedItem = hit.collider.GetComponent<IInteractable>();
                _isHittingInteractable = true;
            }
            
            
        }
    }
}
