using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DummyTarget : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    private BoxCollider _boxCollider;

    private bool gotHit;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = size + Vector3.one * .1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gotHit ? Color.red * .25f : Color.green * .25f;
        Gizmos.DrawCube(transform.position, size + Vector3.one * .1f);
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