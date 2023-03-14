using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateParentForSelected
{
    [MenuItem("Edit/Create parent for selected %g")]
    public static void CreateParent()
    {
        //get all selected transform
        var transforms = new List<Transform>(UnityEditor.Selection.transforms);
        //nothing selected, don't do anything
        if (transforms.Count<=0)
        {
            return;
        }

        //create new gameobject 
        var parentObject = new GameObject("New parent");
        Undo.RegisterCreatedObjectUndo(parentObject, "Create parents for selected");

        transforms.Sort((a,b)=> { return a.GetSiblingIndex().CompareTo(b.GetSiblingIndex()); } );

        //if there's a shared parents, create the parents underneath it, and not in the root 
        var topMostParent = transforms[0].parent;

        //search for the shared parent
        foreach (var t in transforms)
        {
            if (t.parent!=topMostParent)
            {
                topMostParent = null;
            }
        }

        //move the new parent to the shared one 
        parentObject.transform.SetParent(topMostParent, false);

        //move our children to new parent 
        foreach (var t in transforms)
        {
            Undo.SetTransformParent(t, parentObject.transform, "Create parent for selected");
        }

        UnityEditor.Selection.activeGameObject = parentObject;
    }
}
