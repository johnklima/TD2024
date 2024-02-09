using UnityEngine;

public class IKSegment : MonoBehaviour
{
    public Vector3 posA, posB;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(posA, .025f);
        Gizmos.DrawWireSphere(posB, .025f);
        Gizmos.DrawLine(posA, posB);
    }
}