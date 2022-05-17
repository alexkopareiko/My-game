using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelpers : MonoBehaviour
{
    // Look through all parent components until meet "tag   "
    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

    // Deactivate all renderers
    public static void DeactivateRenderers(Renderer[] renderers)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
}
