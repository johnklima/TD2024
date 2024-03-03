using System;
using UnityEngine;
using UnityEngine.Events;

// leif edit: changed class name to reflect file name
public class HealingArea : MonoBehaviour
{
    public TestingHealArea testingHealArea;

    public int healAmount = 1; //leif edit: added amount
    public bool isOneShot;
    public float triggerSize = 2;
    public Vector3 offset;
    public TriggerShape triggerShape;
    public UnityEvent OnHealing;
    private bool hasShot;
    private Vector3 sizeV3 => new(triggerSize, triggerSize, triggerSize);


    private void Start()
    {
        var pos = transform.GetChild(0).localPosition + offset;

        switch (triggerShape)
        {
            case TriggerShape.Box:
            {
                var col = gameObject.AddComponent<BoxCollider>();
                col.size = sizeV3;
                col.isTrigger = true;
                col.center = pos;
                break;
            }
            case TriggerShape.Sphere:
            {
                var col = gameObject.AddComponent<SphereCollider>();
                col.radius = triggerSize / 2;
                col.isTrigger = true;
                col.center = pos;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDrawGizmos()
    {
        if (!isActiveAndEnabled) return;
        var pos = transform.GetChild(0).position + offset;

        switch (triggerShape)
        {
            case TriggerShape.Box:
                Gizmos.DrawWireCube(pos, sizeV3);
                break;
            case TriggerShape.Sphere:
                Gizmos.DrawWireSphere(pos, triggerSize / 2);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOneShot && hasShot) return;
        // Check if the object that entered the trigger is the player
        if (!other.CompareTag("Player")) return; // leif edit: inverted if
        // Get the PlayerHealth component from the player object
        //  leif edit: updated to use the correct one
        //PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        var playerHealth = other.GetComponent<PlayerHealthSystem>();
        // Check if the playerHealth is not null
        if (playerHealth == null) throw new Exception("PlayerHealth component not found on the player object.");
        if (!playerHealth.CanHeal()) // if we cant heal,
        {
            if (testingHealArea.isTesting)
                testingHealArea.MeshRenderer.material = testingHealArea.fail; // set color to red
            Debug.Log("player cant heal");
            return; // exit
        }

        hasShot = true;
        Debug.Log("player can heal");
        // Call the Heal method on the playerHealth
        playerHealth.Heal(healAmount); //leif edit: added healAmount
        if (testingHealArea.isTesting) testingHealArea.MeshRenderer.material = testingHealArea.healing; // green
        OnHealing.Invoke(); //leif edit: added event
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return; // leif edit: inverted if
        if (testingHealArea.isTesting) testingHealArea.MeshRenderer.material = testingHealArea.standby;
    }

    public void TestOnHealing()
    {
        Debug.Log("Test on HealingArea successful");
    }

    [Serializable]
    public class TestingHealArea
    {
        public Material standby;
        public Material healing;
        public Material fail;
        public MeshRenderer MeshRenderer;
        public bool isTesting;
    }
}

public enum TriggerShape
{
    Box,
    Sphere
}