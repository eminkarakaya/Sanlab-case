using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;
    private void Awake() {
        instance = this;
    }
    [SerializeField] private Image fadeImage;
    [SerializeField] private float duration;
    [SerializeField] private GameObject alertPrefab;

    [SerializeField] private TMP_Text alertText;
    [SerializeField] private float yOffset;
    [SerializeField] private Transform parentAlert;
   
    public void AlertAnimation()
    {
        Vector3 pos = parentAlert.transform.position;
        var obj = Instantiate(alertPrefab,pos,Quaternion.identity,parentAlert);
        fadeImage = obj.GetComponent<Image>();
        alertText = obj.transform.GetChild(0).GetComponent<TMP_Text>();
        
        obj.transform.DOMove(new Vector3(pos.x,pos.y + yOffset,pos.z)  ,duration);
        alertText.DOFade(0,duration);
        fadeImage.DOFade(0,duration).OnComplete(()=>{
                Destroy(obj,4f);
            });
            Debug.Log(obj,obj);
    }
    public void ShowAlert(string alertContent)
    {
        AlertAnimation();
        alertText.text = alertContent;
    }
}