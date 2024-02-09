using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class RootTransforms
{
    public Segment leftLeg;
    public Segment rightLeg;
    public Segment leftArm;
    public Segment rightArm;
}

[Serializable]
public class Segment
{
    public Transform start;
    public Transform end;
}

public class IKRigSystem : MonoBehaviour
{
    public RootTransforms rootTransforms;
    public List<IKSegment> leftArmSegments;
    public List<IKSegment> leftLegSegments;
    public List<IKSegment> rightArmSegments;
    public List<IKSegment> rightLegSegments;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnValidate()
    {
        var llr = rootTransforms.leftLeg;
        var rlr = rootTransforms.rightLeg;
        var lar = rootTransforms.leftArm;
        var rar = rootTransforms.rightArm;

        var leftLegEnd = IterateChain(llr.start, llr.end, ref leftArmSegments);
        var rightLegEnd = IterateChain(rlr.start, rlr.end, ref leftLegSegments);
        var leftArmEnd = IterateChain(lar.start, lar.end, ref rightArmSegments);
        var rightArmEnd = IterateChain(rar.start, rar.end, ref rightLegSegments);
    }


    private Transform IterateChain(Transform start, Transform end, ref List<IKSegment> segments)
    {
        if (start == null) return null;
        var child = start.GetChild(0);
        // make a new IKSegment for each bone
        if (!start.TryGetComponent(out IKSegment ikSegment))
            ikSegment = start.AddComponent<IKSegment>();
        if (!segments.Contains(ikSegment))
            segments.Add(ikSegment);
        ikSegment.posA = start.position;
        ikSegment.posB = child.position;

        if (start == end)
            return start;

        return IterateChain(child, end, ref segments);
    }
}