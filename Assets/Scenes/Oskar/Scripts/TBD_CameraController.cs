using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBD_CameraController : MonoBehaviour
{
    public Transform playerObject;
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;

    private float _rotationY = 0f;
    private float _rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        transform.position = playerObject.position;
    }

    public void RotateCamera(Vector2 mousePos)
    {
        _rotationX += mousePos.x * sensitivityX;
        _rotationY -= mousePos.y * sensitivityY;
        _rotationY = Mathf.Clamp(_rotationY, -45f, 45f); // Clamp to prevent flipping

        // Apply rotation around the Y axis globally, and pitch locally
        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0f);
    }
}
