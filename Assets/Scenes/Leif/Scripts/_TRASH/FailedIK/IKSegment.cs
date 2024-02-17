using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class IKSegment : MonoBehaviour
{
    public Vector3 posA, posB;
    public IKSegment parent;
    public IKSegment child;
    public float length;
    public bool look = true;

    private void Start()
    {
    }

    private void LateUpdate()
    {
        // var trans = transform;
        // var pos = trans.position;
        // posA = pos;
        // posB = pos + trans.up * length;
    }

    private void OnDrawGizmos()
    {
        var pos = transform.position;
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            posA = pos;
            CalculatePosB();
        }
#endif
        if (child)
            Gizmos.DrawLine(posA, posB);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(posA, .025f);
        //   Gizmos.DrawWireSphere(posB, .015f);
        //   var asd = Quaternion.LookRotation(transform.up);
        //   Gizmos.matrix = Matrix4x4.TRS(transform.position, asd, transform.lossyScale);
        //   Gizmos.DrawFrustum(transform.position - posA, 10, length, 0, 1);
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

        // CalculatePosB();
    }

    public void PointAt(Transform target)
    {
        var pos = target.localPosition;
        var trans = transform;
        Debug.LogWarning("THIS IS WRONG, LOOK-AT USES FORWARD, WE NEED TO USE UP");
        //TODO THIS IS WRONG, LOOK-AT USES FORWARD, WE NEED TO USE UP
        transform.LookAt(pos);
        transform.RotateAround(trans.position, trans.right, 90);
        var rotation = trans.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(rotation.x - 90, rotation.y, 0));
    }

    public void Drag(Transform target)
    {
        if (child != null) PointAt(target);
        var pos = transform.InverseTransformPoint(target.position - transform.up * length);
        Debug.Log(pos);

        transform.localPosition = pos;
        if (parent != null && look) parent.Drag(transform);
    }

    public void Reach(Transform target)
    {
        Drag(target);
        UpdateSegment();
    }
}