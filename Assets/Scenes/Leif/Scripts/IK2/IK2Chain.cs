using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class IK2Chain : MonoBehaviour
{
    public int numberOfJoints = 3;
    public IK2Segment[] segments;
    public Transform target;

    [FormerlySerializedAs("_iK2System")] [DoNotSerialize]
    public IK2System iK2System;

    private float totalLength;

    private void Start()
    {
    }


    public void UpdateChain(IK2System iK2System)
    {
        if (segments == null) return;
        this.iK2System = iK2System;
        var fS = segments[0];
        var lS = segments[^1];
        if (iK2System.iK2Settings.isDragging)
        {
            lS.Drag(target.position);
        }
        else if (iK2System.iK2Settings.isReaching)
        {
            lS.Reach(target.position); // iterate forward
            fS.pos = transform.position; // lock first seg
            fS.UpdateSegmentAndChildren(); // iterate back
        }
    }

    private void InitializeChain(IK2System iK2System)
    {
        this.iK2System = iK2System;
        numberOfJoints = iK2System.iK2Settings.segmentsPrChain;
        totalLength = iK2System.iK2Settings.chainLength;
    }

    public void Wake(IK2System iK2System, Transform target)
    {
        if (iK2System.snek)
            numberOfJoints = transform.childCount;

        InitializeChain(iK2System);
        this.target = target;
        segments = new IK2Segment[numberOfJoints];
        var segLen = totalLength / numberOfJoints;
        IK2Segment prevSeg = null;
        for (var i = 0; i < numberOfJoints; i++)
        {
            var newGo = transform.GetChild(i).gameObject;
            if (!iK2System.snek)
                newGo = IK2System.GenerateNewGo("IKSegment_" + i, transform);
            var newSegment = newGo.AddComponent<IK2Segment>();
            newSegment.index = i;
            var pos = Vector3.forward * segLen * i;
            newSegment.Wake(this, pos, segLen, prevSeg);
            prevSeg = segments[i] = newSegment;
        }
    }
}