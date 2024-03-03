using UnityEngine;
using UnityEngine.Events;

public class GameOverArea : MonoBehaviour
{
    public UnityEvent OnGameOverEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OnGameOverEnter.Invoke();
    }

    public void TestOnGameOverEnter()
    {
        OnGameOverEnter.Invoke();
    }
}