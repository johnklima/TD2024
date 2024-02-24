using System;
using UnityEngine;
using UnityEngine.Events;

public class DamageArea : MonoBehaviour
{
    public int damageAmount = 1;
    public UnityEvent onDamagingPlayer;
    public float triggerSize = 2;
    public Vector3 offset;
    public TriggerShape triggerShape;
    public DummyTarget dummyTarget;
    private Collider _collider;
    private Vector3 sizeV3 => new(triggerSize, triggerSize, triggerSize);

    private void Start()
    {
        dummyTarget = GetComponent<DummyTarget>();
        dummyTarget.onGotDestroyed.AddListener(DisableDamageArea);
        switch (triggerShape)
        {
            case TriggerShape.Box:
            {
                var bCol = gameObject.AddComponent<BoxCollider>();
                bCol.size = sizeV3;
                bCol.isTrigger = true;
                bCol.center = offset;
                _collider = bCol;
                break;
            }
            case TriggerShape.Sphere:
            {
                var sCol = gameObject.AddComponent<SphereCollider>();
                sCol.radius = triggerSize / 2;
                sCol.isTrigger = true;
                sCol.center = offset;
                _collider = sCol;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDrawGizmos()
    {
        if (!isActiveAndEnabled) return;
        var pos = transform.position + offset;
        Gizmos.color = Color.red;
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
        if (!other.CompareTag("Player")) return;
        var playerHealth = other.GetComponent<PlayerHealthSystem>();
        if (playerHealth == null) throw new Exception("PlayerHealth component not found on the player object.");
        Debug.Log("player took damage");
        playerHealth.TakeDamage(damageAmount);
        onDamagingPlayer.Invoke();
    }

    private void DisableDamageArea()
    {
        if (_collider != null) _collider.enabled = false;
        enabled = false;
    }

    public void TestOnDamage()
    {
        Debug.Log("Test on DamageArea successful");
    }
}