using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
// [RequireComponent(typeof(Rigidbody))]
public class DummyTarget : MonoBehaviour
{
    public Potion requiredPotion;

    [Header("hit box settings")] public float radius = .5f;

    public Vector3 center;

    [Header("event settings")] public float scaleSpeed = 1;

    public int damageAmount = 1;
    public UnityEvent onGotHit = new();
    public UnityEvent onGotDestroyed = new();

    [HideInInspector] public bool gotHit;

    private CurseOMeter _curseOMeter;
    private float _lerpAlpha = 1;
    private PlayerHealthSystem _playerHealthSystem;
    private Rigidbody _rigidbody;
    private SphereCollider _sphereCollider;

    private void Start()
    {
        _playerHealthSystem = FindObjectOfType<PlayerHealthSystem>();
        if (_playerHealthSystem == null) throw new Exception("Make sure there is a <PlayerHealthSystem> in the scene");


        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = radius;
        _sphereCollider.center = center;


        if (_curseOMeter == null) _curseOMeter = GetComponentInChildren<CurseOMeter>();
        _curseOMeter.DummyTarget = this;
        onGotDestroyed.AddListener(() => { _curseOMeter.PlantDestroyed(); });

        // _sphereCollider.isTrigger = true;
        // _rigidbody = gameObject.GetComponent<Rigidbody>();
        // _rigidbody.useGravity = false;
        // _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (gotHit)
        {
            if (_lerpAlpha > 0)
                _lerpAlpha -= Time.deltaTime * scaleSpeed;
            transform.localScale = Vector3.one * (_lerpAlpha <= 0 ? 0 : _lerpAlpha);
            if (_lerpAlpha > 0)
                return;
            enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var collisionObject = other.gameObject;
        // ignore everything except potion
        if (!collisionObject.CompareTag("Interactable")) return;

        // destroy whatever hits us
        Destroy(collisionObject);
        onGotHit.Invoke();


        // check if its a potion and that its the correct potion
        var isPotionItem = collisionObject.TryGetComponent(out PotionObjectItem potionItem);
        Debug.Log("isPotionItem" + isPotionItem);
        Debug.Log("requiredPotion" + requiredPotion);
        Debug.Log("potionItem.itemData2.potion" + potionItem.itemData2.potion);

        if (isPotionItem && requiredPotion == potionItem.itemData2.potion)
        {
            // correct potion
            onGotDestroyed.Invoke();
            _sphereCollider.enabled = false;
            gotHit = true; // turn on "animation"
        }
        else
        {
            // wrong potion
            // damage player
            _playerHealthSystem.TakeDamage(damageAmount);
            Debug.Log("dummy hit by wrong item: " + collisionObject.name);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gotHit ? Color.red * .25f : Color.blue * .25f;
        Gizmos.DrawSphere(transform.position + center, radius);
    }

    private void OnValidate()
    {
        if (_curseOMeter == null) _curseOMeter = GetComponentInChildren<CurseOMeter>();
        _curseOMeter.OnValidated();
    }

    public void TestOnGotDestroyed()
    {
        Debug.Log("TestOnGotDestroyed");
    }

    public void TestOnGotHit()
    {
        Debug.Log("TestOnGotHit");
    }
}