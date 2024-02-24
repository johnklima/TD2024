using UnityEngine;

public class GizmoMe : MonoBehaviour
{
    public float radius = .5f;
    public bool isEnabled;

    private void OnDrawGizmos()
    {
        if (!isEnabled) return;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}