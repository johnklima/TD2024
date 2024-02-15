using System;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowingHandler : MonoBehaviour
{
    //TODO reduce count in UI/inventories
    private CannonBall1 _cannonBall;
    private PlayerController _playerController;
    private UIInventoryManager _uIInventoryManager;

    private void Start()
    {
        _uIInventoryManager = FindObjectOfType<UIInventoryManager>();
        if (_uIInventoryManager == null) throw new Exception("Make sure there is a UIInventoryManager in the scene");
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        var hit = DoRay();
        Item selectedItem = null;
        if (Input.GetMouseButtonDown(0))
        {
            // aim mode
            selectedItem = _uIInventoryManager.selectedItem;
            if (Input.GetMouseButton(1))
                // cancel out of aim mode
                return;
        }

        if (Input.GetMouseButtonUp(0) && hit != Vector3.zero)
            // fire when release
            if (selectedItem != null)
            {
                var newThrowable = Instantiate(selectedItem);
                var newCannon = newThrowable.AddComponent<CannonBall1>();
                newThrowable.transform.position = transform.position;
                newCannon.Launch(hit);
            }
    }

    private Vector3 DoRay()
    {
        var ray = new Ray(transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out var _hit, 1000)) return _hit.point;
        return Vector3.zero;
    }
}