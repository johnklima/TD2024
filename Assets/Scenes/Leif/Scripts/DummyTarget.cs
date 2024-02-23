using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
// [RequireComponent(typeof(Rigidbody))]
public class DummyTarget : MonoBehaviour
{
    public float radius = .5f;
    public Vector3 center;
    public bool isTrashCan;
    public float scaleSpeed = 1;
    public UnityEvent onGotHit = new();
    public bool gotHit;
    private Rigidbody _rigidbody;
    private SphereCollider _sphereCollider;
    private float lerpAlpa = 1;


    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        // _sphereCollider.isTrigger = true;
        _sphereCollider.radius = radius;
        _sphereCollider.center = center;
        // _rigidbody = gameObject.GetComponent<Rigidbody>();
        // _rigidbody.useGravity = false;
        // _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (gotHit && !isTrashCan)
        {
            if (lerpAlpa > 0)
                lerpAlpa -= Time.deltaTime * scaleSpeed;
            transform.localScale = Vector3.one * (lerpAlpa <= 0 ? 0 : lerpAlpa);
            if (lerpAlpa > 0)
                return;
            // StartCoroutine(ResetGotHit());
            enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Interactable")) return;
        //TODO if wrong potion, deal damage to player, else kill plant
        Destroy(other.gameObject);
        _sphereCollider.enabled = false;
        onGotHit.Invoke();
        gotHit = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gotHit ? Color.red * .25f : Color.blue * .25f;
        Gizmos.DrawSphere(transform.position + center, radius);
    }
}