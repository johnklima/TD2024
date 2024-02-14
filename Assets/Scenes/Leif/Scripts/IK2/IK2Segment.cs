using UnityEngine;

public class IK2Segment : MonoBehaviour
{
    public float length;
    public IK2Segment parent, child;

    private IK2Chain _ikThread;

    public Vector3 pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    public Vector3 end => pos + transform.forward * length;

    public Quaternion rotation
    {
        get => transform.rotation;
        set => transform.rotation = value;
    }

    public int index { get; set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos, .5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, end);
        Gizmos.DrawWireSphere(end, .25f);
    }

    public void Wake(IK2Chain ikThread, Vector3 position, float length, IK2Segment parent)
    {
        this.parent = parent;
        if (parent != null)
            this.parent.child = this;
        _ikThread = ikThread;
        pos = position;
        this.length = length;
    }

    public void UpdateSegmentAndChildren()
    {
        UpdateSegment();
        //update its children
        if (child)
            child.UpdateSegmentAndChildren();
    }

    public void UpdateSegment()
    {
        if (parent)
            pos = parent.end;
    }

    public void PointAt(Vector3 target)
    {
        transform.LookAt(target);
        var rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        rotation = Quaternion.Euler(rot);
    }

    public void Drag(Vector3 target)
    {
        PointAt(target);
        pos = target - transform.forward * length;
        transform.localScale = Vector3.one * (Mathf.Sin(Time.time * index) + 1);

        if (parent)
            parent.Drag(transform.position);
    }

    public void Reach(Vector3 target)
    {
        Drag(target);
        UpdateSegment();
    }
}