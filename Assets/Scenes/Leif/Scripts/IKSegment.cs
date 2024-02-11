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
        posB = posA + transform.up * length;
    }

    private void UpdateSegment()
    {
        if (parent == null)
        {
            posA = transform.localPosition;
        }
        else
        {
            posA = parent.posB;
            transform.localPosition = posA;
        }

        CalculatePosB();
    }

    public void PointAt(Transform target)
    {
        var pos = target.localPosition;
        Debug.LogWarning("THIS IS WRONG, LOOK-AT USES FORWARD, WE NEED TO USE UP");
        //TODO THIS IS WRONG, LOOK-AT USES FORWARD, WE NEED TO USE UP
        transform.LookAt(pos);


        var rotation = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(new Vector3(rotation.x - 90, rotation.y, 0));
    }

    public void Drag(Transform target)
    {
        PointAt(target);
        transform.localPosition = target.localPosition + transform.up * length;
        if (parent) parent.Drag(transform);
    }

    public void Reach(Transform target)
    {
        Drag(target);
        UpdateSegment();
    }
}