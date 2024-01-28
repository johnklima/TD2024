using System;
using UnityEngine;

public enum PortalType
{
    Entrance,
    Exit
}

public class PortalTrigger : MonoBehaviour
{
    public Action OnEnter, OnExit;
    public Action<PortalType> OnCollision;
    public PortalType PortalType;

    public MeshCollider MeshCollider;

    private void Awake()
    {
        MeshCollider = GetComponent<MeshCollider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out LeifPlayerController lPC)) return;
        OnCollision?.Invoke(PortalType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        OnEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out LeifPlayerController lPC)) return;
        OnExit?.Invoke();
    }
}