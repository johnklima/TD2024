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
    public List<IKSegment> segments;
    private IKRigSystem _ikRigSystem;
    public string name => limbType.ToString();

    public void ValidateLimbTarget(Transform rig)
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
        CreateTarget(rig);
    }

    private void CreateTarget(Transform rig)
    {
        var newT = new GameObject
        {
            name = name + "_TARGET",
            transform =
            {
                parent = rig,
                position = root.position - rig.forward
            }
        };
        target = newT.transform;
        var limbTarget = newT.AddComponent<LimbTarget>();
        limbTarget.limbType = limbType;
    }

    public void IterateChain(Transform rig)
    {
        var maxIter = 10;
        var currIter = 0;
        var currentTransform = root;
        IKSegment previousSegment = null;
        while (currentTransform != null)
        {
            // check if it has segment and add to list
            if (!currentTransform.TryGetComponent(out IKSegment currentSegment))
                currentSegment = currentTransform.gameObject.AddComponent<IKSegment>();
            if (!segments.Contains(currentSegment))
                segments.Add(currentSegment);
            // if we are on first, set segment.parent to null 

            currentSegment.posA = currentSegment.transform.position; // set A to pos
            // if we are on root, parent == null
            if (currentTransform == root)
            {
                currentSegment.parent = null;
            }
            else
            {
                // if we are not on root
                previousSegment.posB = currentSegment.posA; // set parent.B to our posA
                previousSegment.child = currentSegment; // set previous segments child to this
                currentSegment.parent = previousSegment; // set current segment's parent to previous
                // calculate previous segments length 
                previousSegment.length = Vector3.Distance(currentSegment.posA, previousSegment.posA);
            }

            // set previousSegment to current
            previousSegment = currentSegment;
            // if we have 0 children, exit loop
            if (currentTransform.childCount == 0) break; //! including last segment
            // find next child.
            // if we have more than 1 child, we are at foot and want toe (index 1)
            currentTransform = currentTransform.GetChild(currentTransform.childCount > 1 ? 1 : 0);

            currIter++;
            if (currIter > maxIter)
            {
                Debug.Log("reached end without end");
                break;
            }
        }

        ValidateLimbTarget(rig);
    }

    public void ClearList()
    {
        segments.Clear();
    }
}