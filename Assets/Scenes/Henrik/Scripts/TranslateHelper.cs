using UnityEngine;

public static class TranslateHelper
{
    public static void LerpTowards(this Transform t1, Transform t2, float t)
    {
        t1.position = Vector3.Lerp(t1.position, t2.position, t);
        t1.rotation = Quaternion.Lerp(t1.rotation, t2.rotation, t);
    }
}