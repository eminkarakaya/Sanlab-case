using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    
    public float distance;
    public List<Transform> targets; 

    private void Update() {
        for (int i = 0; i < targets.Count; i++)
        {
            if(Vector3.Distance(targets[i].transform.position , transform.position)<distance)
            {

            }
        }
    }
    public void Stick()
    {

    }
}
