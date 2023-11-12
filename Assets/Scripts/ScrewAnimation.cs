using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScrewAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 rotVector;
    [SerializeField] private Space space;
    public void ScrewSpinAnimation(float dur)
    {
        StartCoroutine(TurnAnim(dur));
    }
    private IEnumerator TurnAnim(float dur)
    {
        var passedTime = 0f;
        while(passedTime<dur)
        {
            passedTime += Time.deltaTime;
            Debug.Log(transform.rotation + " transform.rotation ");
            transform.Rotate(rotVector,space);
            yield return null;
        }
    }
}
