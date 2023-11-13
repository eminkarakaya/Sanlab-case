using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTransform : MonoBehaviour
{
    public bool isFull; // civatanın girecegi yuva dolu mu. 
    public Vector3 animationOffset; // animasyon hangi taraftan ve ne mesafeden başlayacak
    public Vector3 GetAnimPos()
    {        
        return transform.position + (transform.right * animationOffset.x) + (transform.up * animationOffset.y) + (transform.forward * animationOffset.z); 
    }
}
