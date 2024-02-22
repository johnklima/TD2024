using System;
using UnityEngine;
using UnityEngine.Events;

public class Dnafmg : MonoBehaviour, IInteractable
{
    //Definitely Not A Fishing Mini-Game (DNAFMG)
    public float AmpModY = -.01f;

    public float speed = 2.5f;

    public Transform lineTop, lineBottom;
    public float lineWidth = 2.5f;
    public Material lineRendererMat;

    public Transform armatureRoot;
    public PoleSegment[] segments;

    public UnityEvent onInteract;
    private bool _flip;


    private LineRenderer _lineRenderer;
    private float _sinModY;

    private void Start()
    {
        SetupLineRenderer();
        SetupPole();
    }

    // Update is called once per frame
    private void Update()
    {
        Animate();
        WobblePole();
        AnimateLineRenderer();
    }

    private void OnValidate()
    {
        SetupLineRenderer();
    }

    public void Interact()
    {
        onInteract.Invoke();
    }

    public void Interact(LeifPlayerController lPC)
    {
        onInteract.Invoke();
    }

    private void WobblePole()
    {
        for (var i = 0; i < segments.Length; i++)
        {
            var pos = segments[i].pos;
            pos.y = segments[i].start.y + Mathf.Sin(i / 2f * AmpModY) * _sinModY;
            segments[i].pos = pos;
            if (segments[i].parent != null)
            {
                segments[i].parent.transform.LookAt(segments[i].transform);
                segments[i].parent.transform
                    .RotateAround(segments[i].parent.pos, segments[i].parent.transform.right, 90);
            }
        }
    }

    private void SetupPole()
    {
        var pos = armatureRoot.position;
        var res = armatureRoot.childCount;
        // for child, get start+end, parent, child, set pot
        PoleSegment prevSeg = null;
        segments = new PoleSegment[res];
        for (var i = 0; i < res; i++)
        {
            var current = armatureRoot.GetChild(i);
            var currentSeg = current.gameObject.AddComponent<PoleSegment>();

            segments[i] = currentSeg;
            currentSeg.start = i == 0 ? pos : current.position;
            currentSeg.parent = prevSeg;
            if (i != 0)
            {
                prevSeg.end = currentSeg.start;
                prevSeg.child = currentSeg;
            }

            prevSeg = currentSeg;
        }
    }

    private void SetupLineRenderer()
    {
        if (lineRendererMat == null) throw new Exception("No material set: disabling 'DNAFMG'");
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
        _lineRenderer.sharedMaterial = lineRendererMat;
        _lineRenderer.positionCount = 2;
        AnimateLineRenderer();
    }

    private void AnimateLineRenderer()
    {
        _lineRenderer.SetPosition(0, lineTop.position);
        _lineRenderer.SetPosition(1, lineBottom.position);
    }

    private void Animate()
    {
        if (_flip)
            _sinModY -= Time.deltaTime * speed;
        else
            _sinModY += Time.deltaTime * speed;
        if (_sinModY is < 0 or > 1) _flip = !_flip;
    }

    public class PoleSegment : MonoBehaviour
    {
        public Vector3 start, end;
        public PoleSegment parent;
        public PoleSegment child;

        public Vector3 pos
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}