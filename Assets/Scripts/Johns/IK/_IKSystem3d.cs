using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _IKSystem3d : MonoBehaviour
{
    public _Segment3d[] segments;
    public int childcount = 0;
    public Transform target = null;

    public bool isReaching = false;
    public bool isDragging = false;
   
    private _Segment3d lastSegment = null;
    private _Segment3d firstSegment = null;

    
    // Use this for initialization
    void Awake()
    {

        //lets buffer our segements in an array
        childcount = transform.childCount;
        segments = new _Segment3d[childcount];
        int i = 0;
        foreach (Transform child in transform)
        {
            segments[i] = child.GetComponent<_Segment3d>();
            i++;
        }


        firstSegment = segments[0];
        lastSegment = segments[childcount - 1];
    }

    // Update is called once per frame
    void Update()
    {


        if (isDragging)
        {

            lastSegment.drag(target.position);
            
        }
        else if (isReaching)
        {
            //call reach on the last
            lastSegment.reach(target.position);

            //and forward update on the first
            //we needed to maintain that first segment original position
            //which is the position of the IK system itself
            firstSegment.transform.position = transform.position;
            firstSegment.updateSegmentAndChildren();
            
        }
        
    }
    
}

