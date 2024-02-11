using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Limbs
{
    public Limb leftArm;
    public Limb leftLeg;
    public Limb rightArm;
    public Limb rightLeg;
}

public enum LimbType
{
    LeftArm,
    LeftLeg,
    RightArm,
    RightLeg
}

[Serializable]
public class Limb
{
    public bool isDragging, isReaching;
    public Transform root;
    public Transform target;
    public LimbType limbType;
    private IKRigSystem _ikRigSystem;

    public void Validate(Transform rig)
    {
        if (root == null) return;
        //check if we already have targets on rig
        var limbTargets = rig.gameObject.GetComponentsInChildren<LimbTarget>();
        if (limbTargets.Length > 0)
            // check if target type is same as ours
            foreach (var lTarget in limbTargets)
                if (lTarget.limbType == limbType)
                {
                    // types are same, we have found target, exit
                    target = lTarget.transform;
                    return;
                }

        // else, create new target and assign
        var newT = new GameObject
        {
            name = root.name + "_TARGET",
            transform =
            {
                parent = rig,
                position = root.position - root.forward
            }
        };
        target = newT.transform;
        var limbTarget = newT.AddComponent<LimbTarget>();
        limbTarget.limbType = limbType;
    }

    public void IterateChain(Transform rig, Transform start, ref List<IKSegment> segments)
    {
        var maxIter = 10;
        var currIter = 0;
        var current = start;
        IKSegment previousSegment = null;
        while (current != null)
        {
            if (current.childCount == 0) break; // todo include last segment?
            // check if it has segment and add to list
            if (!current.TryGetComponent(out IKSegment currentSegment))
                currentSegment = current.gameObject.AddComponent<IKSegment>();
            if (!segments.Contains(currentSegment))
                segments.Add(currentSegment);
            // if we are on first, set segment.parent to null 

            currentSegment.posA = currentSegment.transform.position; // set A to pos
            if (current == start)
            {
                currentSegment.parent = null;
            }
            else
            {
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

        Validate(rig);
    }
}