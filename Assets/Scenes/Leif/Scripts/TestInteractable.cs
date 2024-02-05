using UnityEngine;

public class TestInteractable : MonoBehaviour
{
    public void TestOnEnter(Collider other)
    {
        Debug.Log("TestOnEnter: " + other.gameObject.name);
    }

    public void TestOnExit(Collider other)
    {
        Debug.Log("TestOnExit: " + other.gameObject.name);
    }

    public void TestOnRay()
    {
        Debug.Log("TestOnRay");
    }

    public void TestOnKey()
    {
        Debug.Log("TestOnKey");
    }
}