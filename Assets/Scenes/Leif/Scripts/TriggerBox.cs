using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerBox : MonoBehaviour, IInteractable
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
        //todo check for player correctly
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        _inTrigger = true;
        _onEnter?.Invoke(other);
    }


    private void OnTriggerExit(Collider other)
    {
        //todo check for player correctly
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
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        _boxCollider ??= GetComponent<BoxCollider>();
        _interactable ??= GetComponentInParent<Interactable>();

        _boxCollider.isTrigger = true;
        transform.localPosition = _interactable.triggerBoxData.localPos;
        _boxCollider.size = _interactable.triggerBoxData.size;
        interactionEvents = _interactable.interactionEvents;

        _onEnter = interactionEvents.onEnter;
        _onExit = interactionEvents.onExit;
        _onRayInteract = interactionEvents.onRayInteract;
    }
}