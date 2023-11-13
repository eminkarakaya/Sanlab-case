using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTransform : MonoBehaviour
{
    public bool isFull;
    public Vector3 sideMultiplier;
    public Vector3 GetAnimPos()
    {        
        return transform.position + (transform.right * sideMultiplier.x) + (transform.up * sideMultiplier.y) + (transform.forward * sideMultiplier.z); 
    }
}
