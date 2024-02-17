using System;
using UnityEngine;
using UnityEngine.Events;

// leif edit: changed class name to reflect file name
public class HealingArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (!other.CompareTag("Player")) return; // leif edit: inverted if
        // Get the PlayerHealth component from the player object
        //  leif edit: updated to use the correct one
        //PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        var playerHealth = other.GetComponent<PlayerHealthSystem>();

        // Check if the playerHealth is not null
        if (playerHealth != null)
        {
            // Call the Heal method on the playerHealth
            playerHealth.Heal(healAmount); //leif edit: added healAmount
            OnHealing.Invoke(); //leif edit: added event
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on the player object.");
        }
    }
    // leif edit: my textEditor (IDE) formats the code automatically (sorry, not sorry :D )

    #region LEIF EDIT

    public int healAmount = 1; //leif edit: added amount
    public float triggerSize = 2;
    public Vector3 offset;
    public TriggerShape triggerShape;
    public UnityEvent OnHealing;

    private Vector3 sizeV3 => new(triggerSize, triggerSize, triggerSize);
    private BoxCollider _boxCollider;

    private void OnDrawGizmos()
    {
        var pos = transform.position + offset;
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

    public enum TriggerShape
    {
        Box,
        Sphere
    }

    private void Start()
    {
        var pos = transform.position + offset;

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

    public void TestOnHealing()
    {
        Debug.Log("Test on HealingArea successful");
    }

    #endregion
}