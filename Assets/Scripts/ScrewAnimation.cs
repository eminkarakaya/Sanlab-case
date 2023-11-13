using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScrewAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 rotVector;
    [SerializeField] private Space space;
    public void ScrewRotationAnimation(float dur)
    {
        StartCoroutine(TurnAnimation(dur));
    }
    private IEnumerator TurnAnimation(float dur)
    {
        var passedTime = 0f;
        while(passedTime<dur)
        {
            passedTime += Time.deltaTime;
            transform.Rotate(rotVector,space);
            yield return null;
        }
    }
}
