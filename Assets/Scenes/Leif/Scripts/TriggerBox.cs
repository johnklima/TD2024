using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TriggerBoxData
{
    public Vector3 size = Vector3.one;
    public Vector3 localPos = Vector3.zero;
}


[RequireComponent(typeof(BoxCollider))]
public class TriggerBox : MonoBehaviour, I_Interactable
{
    private readonly UnityEvent _onKeyPress = new();
    private BoxCollider _boxCollider;
    private Interactable _interactable;
    private bool _inTrigger;
    private KeyCode _key;
    private UnityEvent<Collider> _onEnter = new();
    private UnityEvent<Collider> _onExit = new();
    private UnityEvent _onRayInteract = new();
    private InteractionEvents interactionEvents;

    private void Awake()
    {
        ValidateComponents();
    }

    private void Update()
    {
        if (_inTrigger && Input.GetKeyDown(_key)) _onKeyPress?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        //todo check for player
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        _inTrigger = true;
        _onEnter?.Invoke(other);
    }


    private void OnTriggerExit(Collider other)
    {
        //todo check for player
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        _inTrigger = false;
        _onExit?.Invoke(other);
    }

    private void OnValidate()
    {
        _boxCollider ??= GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }

    public void Interact()
    {
        _onRayInteract?.Invoke();
    }

    public void UpdateTriggerBox(Interactable interactable)
    {
        _interactable = interactable;
        interactionEvents = interactable.interactionEvents;
        _boxCollider.size = interactable.triggerBoxData.size;
        transform.localPosition = interactable.triggerBoxData.localPos;
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        _boxCollider ??= GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;

        _interactable ??= GetComponentInParent<Interactable>();
        interactionEvents = _interactable.interactionEvents;

        _onEnter = interactionEvents.onEnter;
        _onExit = interactionEvents.onExit;
        _onRayInteract = interactionEvents.onRayInteract;
    }
}