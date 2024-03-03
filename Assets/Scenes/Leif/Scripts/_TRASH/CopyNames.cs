using TMPro;
using UnityEngine;

public class CopyNames : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnValidate()
    {
        foreach (Transform t in transform)
        {
            var tt = t.GetComponent<TMP_Text>();
            tt.text = t.name;
            if (t.childCount > 0)
                for (var i = 0; i < t.childCount; i++)
                {
                    var tc = t.GetChild(i);
                    var txt = tc.GetComponent<TMP_Text>();
                    txt.text = tc.name;
                }
        }
    }
}