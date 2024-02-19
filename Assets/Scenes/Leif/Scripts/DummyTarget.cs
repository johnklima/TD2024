using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class DummyTarget : MonoBehaviour
{
    public float radius = .5f;
    public Vector3 center;
    private SphereCollider _boxCollider;
    private Rigidbody _rigidbody;
    private bool gotHit;

    private void Start()
    {
        _boxCollider = GetComponent<SphereCollider>();
        _boxCollider.isTrigger = true;
        _boxCollider.radius = radius;
        _boxCollider.center = center;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gotHit ? Color.red * .25f : Color.green * .25f;
        Gizmos.DrawSphere(transform.position + center, radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Got hit");
        if (!other.CompareTag("Interactable")) return;
        Destroy(other.gameObject);
        gotHit = true;
        StartCoroutine(ResetGotHit());
    }

    private IEnumerator ResetGotHit()
    {
        yield return new WaitForSeconds(.1f);
        gotHit = false;
    }
}