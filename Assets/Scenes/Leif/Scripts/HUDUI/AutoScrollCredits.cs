using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoScrollCredits : MonoBehaviour
{
    public float scrollSpeed = 1;
    private Scrollbar _scrollbar;

    private bool doScroll;
    private float scrollAlpha;

    private float timeStamp;

    private void Start()
    {
        _scrollbar = GetComponentInChildren<Scrollbar>();
    }

    private void Update()
    {
        if (doScroll) _scrollbar.value = 1f - scrollAlpha;
    }

    private void OnEnable()
    {
        scrollAlpha = 0;
        StartScroll();
    }

    private void OnDisable()
    {
        StopScroll();
    }

    public void StartScroll()
    {
        if (!isActiveAndEnabled) return;
        doScroll = true;
        StartCoroutine(AutoScroll());
    }

    public void OnValueChanged(Vector2 a)
    {
        if (doScroll) return;
        scrollAlpha = 1f - a.y;
    }

    public void StopScroll()
    {
        doScroll = false;
        StopCoroutine(AutoScroll());
    }

    private IEnumerator AutoScroll()
    {
        var iterationAmount = 0.01f;
        var maxI = 1f / (iterationAmount * scrollSpeed);
        var curI = 0;

        while (doScroll)
        {
            if (scrollAlpha < 1)
                scrollAlpha += iterationAmount * scrollSpeed;

            yield return new WaitForSecondsRealtime(iterationAmount);
            curI++;
            if (curI > maxI)
            {
                doScroll = false;
                StopCoroutine(AutoScroll());
            }
        }
    }
}