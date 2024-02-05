using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractableCamera : MonoBehaviour
{
    [DoNotSerialize] public LayerMask interactableLayerMask;
    public UnityEvent onAimingOn = new();
    public UnityEvent onAimingOff = new();
    private RaycastHit _hit;
    private bool _wasAimingAtLastFrame;

    private void Update()
    {
        DoRay();
    }

    private void DoRay()
    {
        var ray = new Ray(transform.position, transform.forward);
        if (!Physics.Raycast(ray, out _hit, 1000, interactableLayerMask))
        {
            if (_wasAimingAtLastFrame)
            {
                //* not hitting I_Interactable this frame == aiming off
                onAimingOff?.Invoke();
                _wasAimingAtLastFrame = false;
            }

            return;
        }

        if (_hit.transform.gameObject.TryGetComponent(out I_Interactable iInteractable))
        {
            //* trigger the event if:
            //* we are aiming at I_Interactable, and
            //* we were not aiming at a interactable last frame
            if (!_wasAimingAtLastFrame) onAimingOn?.Invoke();
            _wasAimingAtLastFrame = true;
        }
    }

    public bool TryDoRay(out I_Interactable interactable)
    {
        interactable = null;
        var ray = new Ray(transform.position, transform.forward);
        if (!Physics.Raycast(ray, out _hit, 1000, interactableLayerMask)) return false;
        if (!_hit.transform.gameObject.TryGetComponent(out I_Interactable iInteractable)) return false;
        interactable = iInteractable;
        return true;
    }
}