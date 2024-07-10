using System.Collections.Generic;
using UnityEngine;

public class Hierarchy : MonoBehaviour
{
    public Transform root;
    public List<Relationship> relationships = new List<Relationship>();
    public List<GameObject> v1Roots = new List<GameObject>();

    void Start()
    {
        root = transform.root;
    }

    [ContextMenu("ViewHierarchy")]
    void ViewHierarchy()
    {
        if (root == null) root = transform;
        Transform _duplicate = Instantiate(root);
        gameObject.SetActive(false);
        _duplicate.gameObject.SetActive(true);
        _duplicate.name = root.name + "_HierarchyDuplicate";

        foreach (var relationship in _duplicate.GetComponent<Hierarchy>().relationships)
        {
            foreach (var child in relationship.childs)
            {
                child.SetParent(relationship.parent, true);
            }
        }
    }

    public void SetRoot()
    {
        root = transform.root;

    }

    [System.Serializable]
    public class Relationship
    {
        public Transform parent;
        public bool ignoreCutoutScaling = false;
        public List<Transform> childs = new List<Transform>();
    }
    [System.Serializable]
    public class BooleanObjectInfo
    {
        public Transform obj;
        public OperationType type;
        public bool canSnapped = true;
        public bool disableBoxCollider = false;
        public bool adjustCutoutScaleForOuterWall = false;
    }
    [System.Serializable]
    public enum OperationType
    {
        union,
        difference
    }
}
