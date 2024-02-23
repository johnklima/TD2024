using UnityEngine;
using UnityEngine.Events;

public class TestInteractable : MonoBehaviour
{
    public UnityEvent<Collider> testOnEnter;
    public UnityEvent<Collider> testOnExit;
    public UnityEvent testOnRayInteract;
    public UnityEvent testOnAimingOn;
    public UnityEvent testOnAimingOff;
    public UnityEvent testAimingOff;
    public UnityEvent testAimingOn;
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

        var interactableCamera = _interactable.interactableCamera;
        interactableCamera.onAimingOn.Add(testAimingOn);
        interactableCamera.onAimingOff.Add(testAimingOff);
    }

    public void TestOnEnter(Collider other)
    {
        testOnEnter?.Invoke(other);
        Debug.Log("Test: OnEnter: " + other.gameObject.name);
    }

    public void TestOnExit(Collider other)
    {
        testOnExit?.Invoke(other);
        Debug.Log("Test: OnExit: " + other.gameObject.name);
    }

    public void TestOnRay()
    {
        testOnRayInteract?.Invoke();
        Debug.Log("Test: OnRayInteract");
    }


    public void TestAimingOn()
    {
        testOnAimingOn?.Invoke();
        Debug.Log("Test: AimingOn");
    }

    public void TestAimingOff()
    {
        testOnAimingOff?.Invoke();
        Debug.Log("Test: AimingOff");
    }
}