using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject previewObj;
    public bool isInAnimation;
    public UnityEvent OnRestart;
    private Vector3[] startPositions; // restart yapmak için objelerin ilk pozisyonlarını tutar.
    [SerializeField] private List<Transform> transforms; // 
    [SerializeField] private Vector3 min,max;
    
    private void Start() {
        AssignStartPositions();
    }
    private void AssignStartPositions()
    {
        startPositions = new Vector3[transforms.Count];
        for (int i = 0; i < transforms.Count; i++)
        {
            startPositions[i] = transforms[i].position;
        }
    }
    public void RestartRandomPositions()
    {
        if(isInAnimation) return;
        foreach (var item in transforms)
        {
            item.SetParent(null);
            item.transform.position = new Vector3(Random.Range(min.x,max.x),Random.Range(min.y,max.y),Random.Range(min.z,max.z));
            item.GetComponent<MagnetController>().Restart();
        }
        OnRestart?.Invoke();
    }
    public void RestartOldPosition()
    {
        if(isInAnimation) return;
        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].SetParent(null);
            transforms[i].position = startPositions[i]; 
            transforms[i].GetComponent<MagnetController>().Restart();
        }
        OnRestart?.Invoke();
    }
    public void OpenPreview()
    {
        previewObj.SetActive(true);
        foreach (var item in transforms)
        {
            item.GetComponent<MeshRenderer>().enabled=false;
            item.GetComponent<Collider>().enabled=false;
        }
    }
    public void ClosePreview()
    {

        foreach (var item in transforms)
        {
            item.GetComponent<MeshRenderer>().enabled=true;
            item.GetComponent<Collider>().enabled=true;
        }
        previewObj.SetActive(false);
    }
   
}
