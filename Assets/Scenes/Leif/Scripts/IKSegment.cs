using UnityEngine;

public class IKSegment : MonoBehaviour
{
    public Vector3 posA, posB;
    public IKSegment parent;
    public IKSegment child;
    public float length;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        if (child)
            Gizmos.DrawLine(posA, posB);
        Gizmos.DrawWireSphere(posA, .025f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(posB, .015f);
        var asd = Quaternion.LookRotation(transform.up);
        Gizmos.matrix = Matrix4x4.TRS(transform.position, asd, transform.lossyScale);
        Gizmos.DrawFrustum(transform.position - posA, 10, length, 0, 1);
    }

    public void UpdateSegmentAndChildren()
    {
        UpdateSegment();
        if (child) child.UpdateSegmentAndChildren();
    }

    private void CalculatePosB()
    {
        posB = posA + transform.forward * length;
    }

    private void UpdateSegment()
    {
        if (parent)
        {
            posA = parent.posB;
            transform.position = posA;
        }
        else
        {
            posA = transform.position;
        }

        CalculatePosB();
    }

    public void PointAt(Transform target)
    {
        var rotation = transform.rotation.eulerAngles;
        transform.LookAt(target);
        transform.rotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y, 0));
    }

    public void Drag(Transform target)
    {
        PointAt(target);
        transform.position = target.position - transform.forward * length;
        if (parent) parent.Drag(transform);
    }

    public void Reach(Transform target)
    {
        Drag(target);
        UpdateSegment();
    }
}