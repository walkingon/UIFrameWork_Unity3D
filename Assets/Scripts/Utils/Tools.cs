using UnityEngine;

public class Tools
{
    public static void normalizedParent(Transform child, Transform parent)
    {
        child.SetParent(parent);
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localRotation = Quaternion.identity;
    }
}