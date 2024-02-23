using UnityEngine;

public class GizmoMe : MonoBehaviour
{
    public float radius = .5f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}