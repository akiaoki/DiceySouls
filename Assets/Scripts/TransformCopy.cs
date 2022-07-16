using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCopy : MonoBehaviour
{
    public Transform target;
    public bool copyPosition;
    public bool copyRotation;

    public Vector3 positionOffset;
    
    void Update()
    {
        if (target == null)
            return;
        
        if (copyPosition)
            transform.position = target.position + positionOffset;

        if (copyRotation)
            transform.rotation = target.rotation;
    }
}
