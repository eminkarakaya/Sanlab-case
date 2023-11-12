using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public enum TaskSide
{
    Up,
    Down
}
[Serializable]
public class TransformData
{
    public List<TargetTransform> detectTargets;
    public Transform target;
}
public class MagnetController : MonoBehaviour ,IPointerEnterHandler , IPointerExitHandler
{

    Camera cam;
    public UnityEvent OnMouseUpEvent;
    public UnityEvent<float> AnimationEvent;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float zCoordinate;
    Order task;
    public TaskSide taskSide;
    public int taskIndex;
    public TransformData transformData;
    public bool isStick;
    Outline outline;
    public float distance,saydamDistance;
    public Transform target;
    private bool isSelected;
    private bool isInAnimation;
    private Vector3 startDragPos;
    public bool IsSelected{get => isSelected; set{
        isSelected = value;
    }}
    private int detectTargetIndex;
    private void Start() {
        cam = Camera.main;
        outline = GetComponent<Outline>();
        outline = GetComponent<Outline>();
        task = TaskManager.instance.GetTask(taskSide,taskIndex);
        OnMouseUpEvent.AddListener(Stick);
        
        if(transformData.target != null)
        {
            target = transformData.target;; 
        }
    }
    private void OnMouseDown() {
        if(isInAnimation) return;
        if(isStick)
        {
            if(CheckPrevTasks() )
            {
                Debug.Log("checkprevtask");
                return;
            }
        }
        if(transformData.target == null)
        {
            foreach (var item in transformData.detectTargets)
            {
                if(!item.isFull)
                    item.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            transformData.target.GetComponent<MeshRenderer>().enabled = true;
        }
        startDragPos = transform.position;
        transform.parent  = null;
        zCoordinate = cam.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
        IsSelected= true;
        task.IsDone = false;
    }
    private void OnMouseUp() {
        if(!isSelected) return;

        if(isStick)
        {
            if(CheckPrevTasks() )
            {

            Debug.Log("checkprevtask");
            transform.position = startDragPos;
            return;
            }
        
        }
        IsSelected = false;
        OnMouseUpEvent?.Invoke();
        if(transformData.target == null)
        {
            foreach (var item in transformData.detectTargets)
            {
                
                item.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
            transformData.target.GetComponent<MeshRenderer>().enabled = false;
    }
    
    private void OnMouseDrag() {
        if(!isSelected) return;
        transform.position = GetMouseWorldPos() + offset;
    }
    
    private void Update() {

        // secılı degılse bosa calismasin
        if(!IsSelected)
        {
            return;
        }
        
        for (int i = 0; i < transformData.detectTargets.Count; i++)
        {
            // for (int j = 0; j < bizimTransformlar.Count; j++)
            // {
                // if(transformData.target == null)
                //     if(target != null) break;
            if(Vector3.Distance(transformData.detectTargets[i].transform.position , transform.position)<distance)
            {
                if(transformData.detectTargets[i].isFull)
                    continue;
                if(transformData.target == null)
                {
                    target = transformData.detectTargets[i].transform; 
                }

                isStick = true;
                detectTargetIndex = i;
                // transformData.detectTargets[i].isFull = true;
                outline.OutlineColor = Color.green;
                break;
            }
            else
            {
                if(transformData.target == null)
                {
                    target = null;
                }
                // transformData.detectTargets[detectTargetIndex].isFull = false;
                
                // transformData.detectTargets[i].isFull = false;
                isStick = false;
                outline.OutlineColor = Color.white;
            }
            // }
        }
    }
    public void Stick()
    {
        if(isStick)
        {
            task.IsDone = true;
            transformData.detectTargets[detectTargetIndex].isFull = true;
            if(TaskManager.instance.IsFinish())
            {
                Debug.Log("Finish");
            }
            if(CheckPrevTasks())
            {
                isStick = false;
                return;
            }
            isInAnimation = true;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(transformData.detectTargets[detectTargetIndex].GetAnimPos(),1).OnComplete(()=>AnimationEvent?.Invoke(1)));
            sequence.Append(transform.DOMove(target.transform.position,1).OnComplete(()=>isInAnimation = false));
            
            
            // transform.position = target.transform.position;
            MakeParent();
        }
        else
        {
            transformData.detectTargets[detectTargetIndex].isFull = false;
            task.IsDone = false;
        }
    }
    public bool CheckPrevTasks1()
    {
        List<Order> tasks =  TaskManager.instance.GetTasks(taskSide,taskIndex);
        if(tasks.Count > 0)
        {    
            return true;
        }
        return false;
    }
    public bool CheckPrevTasks()
    {
        List<Order> tasks =  TaskManager.instance.GetTasks(taskSide,taskIndex);
        if(tasks.Count > 0)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                OutlineAlert(2,tasks[i].taskObject.GetComponent<Outline>());
            }
            return true;
        }
        return false;
    }
    public void MakeParent()
    {
        transform.parent = target.transform.parent;
    }
    public void OutlineAlert(float dur , Outline outline)
    {
        outline.enabled = true;
        Color color = outline.OutlineColor; 
        outline.OutlineColor = Color.red;
        StartCoroutine (OutlineAlertCoroutine(dur,color,outline));
    }
    IEnumerator OutlineAlertCoroutine(float dur,Color targetColor,Outline outline)
    {
        yield return new WaitForSeconds(dur);
        outline.OutlineColor = targetColor;
        outline.enabled = false;
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoordinate;
        return cam.ScreenToWorldPoint(mousePoint);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
    
}
