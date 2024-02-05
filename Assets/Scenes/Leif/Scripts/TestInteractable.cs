using UnityEngine;
using UnityEngine.Events;

public class TestInteractable : MonoBehaviour
{
    public UnityEvent<Collider> testOnEnter;
    public UnityEvent<Collider> testOnExit;
    public UnityEvent testOnRayInteract;
    public UnityEvent testOnAimingOn;
    public UnityEvent testOnAimingOff;
    private Interactable _interactable;


    private void Start()
    {
        _interactable ??= GetComponentInParent<Interactable>();
        if (!_interactable.useTester) return;
        var interactionEvents = _interactable.interactionEvents;

        interactionEvents.onEnter.RemoveListener(TestOnEnter);
        interactionEvents.onExit.RemoveListener(TestOnExit);
        interactionEvents.onRayInteract.RemoveListener(TestOnRay);

        interactionEvents.onEnter.AddListener(TestOnEnter);
        interactionEvents.onExit.AddListener(TestOnExit);
        interactionEvents.onRayInteract.AddListener(TestOnRay);

        var interactableCamera = _interactable.InteractableCamera;
        interactableCamera.onAimingOn.RemoveListener(TestAimingOn);
        interactableCamera.onAimingOff.RemoveListener(TestAimingOff);

        interactableCamera.onAimingOn.AddListener(TestAimingOn);
        interactableCamera.onAimingOff.AddListener(TestAimingOff);
    }

    public void TestOnEnter(Collider other)
    {
        testOnEnter?.Invoke(other);
        Debug.Log("TestOnEnter: " + other.gameObject.name);
    }

    public void TestOnExit(Collider other)
    {
        testOnExit?.Invoke(other);
        Debug.Log("TestOnExit: " + other.gameObject.name);
    }

    public void TestOnRay()
    {
        testOnRayInteract?.Invoke();
        Debug.Log("TestOnRay");
    }


    public void TestAimingOn()
    {
        testOnAimingOn?.Invoke();
        Debug.Log("TestAimingOn");
    }

    public void TestAimingOff()
    {
        testOnAimingOff?.Invoke();
        Debug.Log("TestAimingOff");
    }
}