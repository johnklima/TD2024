using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IKRigSystem : MonoBehaviour
{
    public Limbs limbs;

    public List<IKSegment> leftArmSegments;
    public List<IKSegment> leftLegSegments;
    public List<IKSegment> rightArmSegments;
    public List<IKSegment> rightLegSegments;


    private void Start()
    {
    }

    private void Update()
    {
        // Left Arm
        var la = limbs.leftArm;
        if (la.isDragging)
        {
            leftArmSegments[^1].Drag(la.target);
        }
        else if (la.isReaching)
        {
            leftArmSegments[^1].Reach(la.target);
            var firstSeg = leftArmSegments[0];
            firstSeg.transform.position = transform.position; //! not correct
            firstSeg.UpdateSegmentAndChildren();
        }

        // Left Leg
        var ll = limbs.leftLeg;
        if (ll.isDragging)
        {
            leftLegSegments[^1].Drag(ll.target);
        }
        else if (ll.isReaching)
        {
            leftLegSegments[^1].Reach(ll.target);
            var firstSeg = leftLegSegments[0];
            firstSeg.transform.position = transform.position; //! not correct
            firstSeg.UpdateSegmentAndChildren();
        }

        // Right Arm
        var ra = limbs.rightArm;
        if (ra.isDragging)
        {
            rightArmSegments[^1].Drag(ra.target);
        }
        else if (ra.isReaching)
        {
            rightArmSegments[^1].Reach(ra.target);
            var firstSeg = rightArmSegments[0];
            firstSeg.transform.position = transform.position; //! not correct
            firstSeg.UpdateSegmentAndChildren();
        }

        // Right Leg
        var rl = limbs.rightLeg;
        if (rl.isDragging)
        {
            rightLegSegments[^1].Drag(rl.target);
        }
        else if (rl.isReaching)
        {
            rightLegSegments[^1].Reach(rl.target);
            var firstSeg = rightLegSegments[0];
            firstSeg.transform.position = transform.position; //! not correct
            firstSeg.UpdateSegmentAndChildren();
        }
    }

    private void OnDrawGizmos()
    {
        // var laTarget = limbs.leftArm.target;
        // var llTarget = limbs.leftLeg.target;
        // var raTarget = limbs.rightArm.target;
        // var rlTarget = limbs.rightLeg.target;
        // if (laTarget != null)
        //     Gizmos.DrawSphere(laTarget.position, 0.015f);
        // if (llTarget != null)
        //     Gizmos.DrawSphere(llTarget.position, 0.015f);
        // if (raTarget != null)
        //     Gizmos.DrawSphere(raTarget.position, 0.015f);
        // if (rlTarget != null)
        //     Gizmos.DrawSphere(rlTarget.position, 0.015f);
    }

    private void OnValidate()
    {
        ResetLists();
        var la = limbs.leftArm;
        if (la.root != null) la.IterateChain(transform, la.root, ref leftArmSegments);
        else if (la.root == null && la.target != null) StartCoroutine(DestroyTarget(la));


        var ll = limbs.leftLeg;
        if (ll.root != null) ll.IterateChain(transform, ll.root, ref leftLegSegments);
        else if (ll.root == null && ll.target != null) StartCoroutine(DestroyTarget(ll));


        var ra = limbs.rightArm;
        if (ra.root != null) ra.IterateChain(transform, ra.root, ref rightArmSegments);
        else if (ra.root == null && ra.target != null) StartCoroutine(DestroyTarget(ra));


        var rl = limbs.rightLeg;
        if (rl.root != null) rl.IterateChain(transform, rl.root, ref rightLegSegments);
        else if (rl.root == null && rl.target != null) StartCoroutine(DestroyTarget(rl));
    }

    private IEnumerator DestroyTarget(Limb limb)
    {
        yield return new WaitForEndOfFrame();
        var l = limb.target;
        limb.target = null;
        DestroyImmediate(l);
    }

    private void ResetLists()
    {
        leftArmSegments.Clear();
        leftLegSegments.Clear();
        rightArmSegments.Clear();
        rightLegSegments.Clear();
    }

    private void IterateChain(Transform start, ref List<IKSegment> segments)
    {
        var maxIter = 10;
        var currIter = 0;
        var current = start;
        IKSegment previousSegment = null;
        while (current != null)
        {
            if (current.childCount == 0) break;
            // check if it has segment and add to list
            if (!current.TryGetComponent(out IKSegment currentSegment))
                currentSegment = current.AddComponent<IKSegment>();
            if (!segments.Contains(currentSegment))
                segments.Add(currentSegment);
            // if we are on first, set segment.parent to null 

            if (current == start)
            {
                currentSegment.parent = null;
                currentSegment.posA = currentSegment.transform.position; // set A to pos
            }
            else
            {
                currentSegment.posA = currentSegment.transform.position; // set A to pos
                previousSegment.posB = currentSegment.posA; // set parent.B to our pos
                previousSegment.child = currentSegment;
                currentSegment.parent = previousSegment;
                previousSegment.length = Vector3.Distance(currentSegment.posA, previousSegment.posA);
            }

            previousSegment = currentSegment;


            current = current.GetChild(0);
            currIter++;
            if (currIter > maxIter)
            {
                Debug.Log("reached end without end");
                break;
            }
        }


        // if (!start.TryGetComponent(out IKSegment ikSegment))
        //     ikSegment = start.AddComponent<IKSegment>();
        // if (!segments.Contains(ikSegment))
        //     segments.Add(ikSegment);
        // ikSegment.posA = start.position;
        // ikSegment.posB = child.position;
        //
        // if (start == end)
        //     return start;
        //
        // return IterateChain(child, end, ref segments);
    }
}