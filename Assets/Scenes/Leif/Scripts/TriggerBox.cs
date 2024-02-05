using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TriggerBoxData
{
    public Vector3 size = Vector3.one;
    public Vector3 localPos = Vector3.zero;
    public UnityEvent<Collider> onEnter;
    public UnityEvent<Collider> onExit;
    public UnityEvent onRayInteract;
}


[RequireComponent(typeof(BoxCollider))]
public class TriggerBox : MonoBehaviour, I_Interactable
{
    private BoxCollider _boxCollider;
    private bool _doRun = true;
    private Interactable _interactable;
    private bool _inTrigger;
    private KeyCode _key;
    private UnityEvent<Collider> _onEnter;
    private UnityEvent<Collider> _onExit;
    private UnityEvent _onKeyPress;
    private UnityEvent _onRayInteract;
    private TriggerBoxData _triggerBoxData;

    private void Awake()
    {
        ValidateComponents();
        _doRun = _interactable.useTriggerBox;
    }

    private void Update()
    {
        if (_inTrigger && Input.GetKeyDown(_key)) _onKeyPress?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_doRun) return;
        _inTrigger = true;
        _onEnter?.Invoke(other);
    }


    private void OnTriggerExit(Collider other)
    {
        if (!_doRun) return;
        _inTrigger = false;
        _onExit?.Invoke(other);
    }

    private void OnValidate()
    {
        if (!_doRun) return;
        _boxCollider ??= GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }

    public void Interact()
    {
        _onRayInteract?.Invoke();
    }

    public void UpdateTriggerBox(TriggerBoxData triggerBoxData)
    {
        _triggerBoxData = triggerBoxData;
        _boxCollider.size = _triggerBoxData.size;
        transform.localPosition = _triggerBoxData.localPos;
    }

    private void ValidateComponents()
    {
        _boxCollider ??= GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;

        _interactable ??= GetComponentInParent<Interactable>();
        _triggerBoxData = _interactable.triggerBoxData;

        _onEnter = _triggerBoxData.onEnter;
        _onExit = _triggerBoxData.onExit;
        _onRayInteract = _triggerBoxData.onRayInteract;
    }
}