using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject previewObj;
    public bool isInAnimation;
    public UnityEvent OnRestart;
    private Vector3[] startPositions;
    [SerializeField] private List<Transform> transforms;
    [SerializeField] private Vector3 min,max;
    private void Awake() {
        instance = this;
    }
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
    public void RandomTransforms()
    {
        if(isInAnimation) return;
        foreach (var item in transforms)
        {
            item.transform.position = new Vector3(Random.Range(min.x,max.x),Random.Range(min.y,max.y),Random.Range(min.z,max.z));
        }
        OnRestart?.Invoke();
    }
    public void OldPos()
    {
        if(isInAnimation) return;
        for (int i = 0; i < transforms.Count; i++)
        {
            transforms[i].position = startPositions[i]; 
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
