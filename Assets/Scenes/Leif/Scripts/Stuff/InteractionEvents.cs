using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct InteractionEvents
{
    [Tooltip("Triggered when the player enters the triggerBox")]
    public UnityEvent<Collider> onEnter;

    [Tooltip("Triggered when the player exit the triggerBox")]
    public UnityEvent<Collider> onExit;

    [Tooltip("Triggered when the player is in pickUpRadius, is aimingOn triggerBox and presses <key>")]
    public UnityEvent onRayInteract;

    [Tooltip("Triggered the first frame 'Aiming On' an interactable item")]
    public UnityEvent onAimingOn;

    [Tooltip("Triggered the first frame 'Aiming Off' an interactable item")]
    public UnityEvent onAimingOff;
}