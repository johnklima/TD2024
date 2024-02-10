using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Limbs
{
    public Limb leftArm;
    public Limb leftLeg;
    public Limb rightArm;
    public Limb rightLeg;
}


[Serializable]
public class Limb
{
    public bool isDragging, isReaching;
    public Transform root;
    public Transform target;
    private IKRigSystem _ikRigSystem;

    public void Validate(Transform rig)
    {
        if (root != null && target == null)
        {
            var newT = new GameObject
            {
                name = root.name + "_TARGET",
                transform =
                {
                    parent = rig,
                    position = root.position + root.forward
                }
            };
            target = newT.transform;
        }
    }
}


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
        if (limbs.leftArm.isDragging)
            leftArmSegments[^1].Drag(limbs.leftArm.target);
        else if (limbs.rightLeg.isReaching)
            rightLegSegments[^1].Reach(limbs.rightLeg.target);

        if (limbs.leftLeg.isDragging)
            leftLegSegments[^1].Drag(limbs.leftLeg.target);
        else if (limbs.rightLeg.isReaching)
            rightLegSegments[^1].Reach(limbs.rightLeg.target);

        if (limbs.rightArm.isDragging)
            rightArmSegments[^1].Drag(limbs.rightArm.target);
        else if (limbs.rightLeg.isReaching)
            rightLegSegments[^1].Reach(limbs.rightLeg.target);

        if (limbs.rightLeg.isDragging)
            rightLegSegments[^1].Drag(limbs.rightLeg.target);
        else if (limbs.rightLeg.isReaching)
            rightLegSegments[^1].Reach(limbs.rightLeg.target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var laTarget = limbs.leftArm.target;
        var llTarget = limbs.leftLeg.target;
        var raTarget = limbs.rightArm.target;
        var rlTarget = limbs.rightLeg.target;
        if (laTarget != null)
            Gizmos.DrawSphere(laTarget.position, 0.015f);
        if (llTarget != null)
            Gizmos.DrawSphere(laTarget.position, 0.015f);
        if (raTarget != null)
            Gizmos.DrawSphere(laTarget.position, 0.015f);
        if (rlTarget != null)
            Gizmos.DrawSphere(laTarget.position, 0.015f);
    }

    private void OnValidate()
    {
        ResetLists();
        var la = limbs.leftArm;

        if (la != null)
        {
            IterateChain(la.root, ref leftArmSegments);
            la.Validate(transform);
        }

        var ll = limbs.leftLeg;
        if (ll != null)
        {
            IterateChain(ll.root, ref leftLegSegments);
            ll.Validate(transform);
        }

        var ra = limbs.rightArm;
        if (ra != null)
        {
            IterateChain(ra.root, ref rightArmSegments);
            ra.Validate(transform);
        }

        var rl = limbs.rightLeg;
        if (rl != null)
        {
            IterateChain(rl.root, ref rightLegSegments);
            rl.Validate(transform);
        }
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
            if (!current.TryGetComponent(out IKSegment ikSegment))
                ikSegment = current.AddComponent<IKSegment>();
            if (!segments.Contains(ikSegment))
                segments.Add(ikSegment);
            // if we are on first, set segment.parent to null 

            if (current == start)
            {
                ikSegment.parent = null;
                ikSegment.posA = ikSegment.transform.position; // set A to pos
            }
            else
            {
                ikSegment.posA = ikSegment.transform.position; // set A to pos
                previousSegment.posB = ikSegment.posA; // set parent.B to our pos
                previousSegment.child = ikSegment;
                ikSegment.parent = previousSegment;
                previousSegment.length = Vector3.Distance(ikSegment.posA, previousSegment.posA);
            }

            previousSegment = ikSegment;


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