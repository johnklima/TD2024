using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractableCamera : MonoBehaviour
{
    [DoNotSerialize] public LayerMask interactableLayerMask;
    public List<UnityEvent> onAimingOn = new();
    public List<UnityEvent> onAimingOff = new();

    private RaycastHit _hit;
    private bool _wasAimingAtLastFrame;

    private Dictionary<Interactable, Dictionary<string, UnityEvent>> asd;

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
                foreach (var e in onAimingOff) e?.Invoke();
                _wasAimingAtLastFrame = false;
            }

            return;
        }

        if (_hit.transform.gameObject.TryGetComponent(out IInteractable iInteractable))
        {
            //* trigger the event if:
            //* we are aiming at I_Interactable, and
            //* we were not aiming at a interactable last frame
            if (!_wasAimingAtLastFrame)
                foreach (var e in onAimingOn)
                    e?.Invoke();
            _wasAimingAtLastFrame = true;
        }
    }

    public bool TryDoRay(out IInteractable interactable)
    {
        //* do a single ray, once,
        //* returns bool, outputs IInteractable via out keyword
        interactable = null;
        var ray = new Ray(transform.position, transform.forward);
        if (!Physics.Raycast(ray, out _hit, 1000, interactableLayerMask)) return false;
        if (!_hit.transform.gameObject.TryGetComponent(out IInteractable iInteractable)) return false;
        interactable = iInteractable;
        return true;
    }
}