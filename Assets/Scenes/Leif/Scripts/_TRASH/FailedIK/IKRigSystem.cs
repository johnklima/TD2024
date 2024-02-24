using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKRigSystem : MonoBehaviour
{
    public List<Limb> limbs;

    private void Update()
    {
        foreach (var limb in limbs)
        {
            var firstSeg = limb.segments[0];
            var lastSeg = limb.segments[^1];
            if (limb.isDragging)
            {
                lastSeg.Drag(limb.target);
            }
            else if (limb.isReaching)
            {
                lastSeg.Reach(limb.target);
                firstSeg.transform.position = firstSeg.posA;
                firstSeg.UpdateSegmentAndChildren();
            }
        }
    }


    private void OnValidate()
    {
        ResetLists();
        if (limbs.Count == 0)
            StartCoroutine(CleanUp());
        else if (limbs.Count > 0)
            foreach (var limb in limbs)
                if (limb.root != null) limb.IterateChain(transform);
                else if (limb.root == null && limb.target != null) StartCoroutine(DestroyLimbTarget(limb));
    }

    private IEnumerator CleanUp()
    {
        yield return new WaitForEndOfFrame();

        var segments = transform.gameObject.GetComponentsInChildren<IKSegment>();
        var targets = transform.gameObject.GetComponentsInChildren<LimbTarget>();

        foreach (var segment in segments)
            DestroyImmediate(segment);
        foreach (var target in targets)
            DestroyImmediate(target.gameObject);
    }

    private IEnumerator DestroyLimbTarget(Limb limb)
    {
        yield return new WaitForEndOfFrame();
        var l = limb.target;
        foreach (var seg in limb.segments) DestroyImmediate(seg);
        limb.target = null;
        DestroyImmediate(l);
    }

    private void ResetLists()
    {
        foreach (var limb in limbs) limb.ClearList();
    }
}