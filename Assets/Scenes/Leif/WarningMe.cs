using UnityEngine;

public class WarningMe : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnValidate()
    {
        string msg(string x, int y = 20)
        {
            return $"<size={y}><color=red>" + x + "</color></size>";
        }

        Debug.Log(msg("<b>Please</b> do not change anything in this scene!"));
        Debug.Log(msg("If you want to modify this scene in any way:"));
        Debug.Log(msg("make a copy of it and put in your folder!"));
        Debug.Log(msg("</color><color=green>Thank you!</color> - <color=white><b>Leif</b>"));
    }
}