using UnityEngine;

public class LimbTarget : MonoBehaviour
{
    public LimbType limbType;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.015f);
    }
}