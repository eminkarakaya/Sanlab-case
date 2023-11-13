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
    private const string extractionAlert = "Objeyi Çıkarmak İçin Önce Kırmızı Objeleri Çıkarmalısınız.",stickAlert = "Kırmızı nesneler objeyi yerleştirmeye engel oluyor.";
    public UnityEvent<float> AnimationEvent;
    [Header("Task")]
    [SerializeField] private int taskIndex;
    [SerializeField] private TaskSide taskSide;
    [SerializeField] private TransformData transformData;
    [SerializeField] private float stickDistance;
    private Vector3 offset;
    private Camera cam;
    private float zCoordinate;
    private Order task;
    private bool isStick; // obje gidecegi yere yeterince yaklaşırsa true olur, uzaklaşırsa false.
    private Outline outline;
    [SerializeField] private Transform target;
    private Vector3 startDragPos;
    private bool isSelected; // onMouseDown da true onMouseUp da false olur.  sadece hareket edecek objenin update' si çalışır. dıgerlerınınki boşa çalışmaz.
    private int detectTargetIndex;
    private void Start() {
        cam = Camera.main;
        outline = GetComponent<Outline>();
        task = TaskManager.Instance.GetTask(taskSide,taskIndex);
        
        if(transformData.target != null)
        {
            target = transformData.target;; 
        }
    }
    private void OnMouseDown() {

        if(TaskManager.Instance.isFinish) return;
        // animasyon oynarken hareket ettiremeyiz.
        if(GameManager.Instance.isInAnimation) 
        {
            AlertManager.Instance.ShowAlert("Animasyonun Bitmesini Bekleyin.");
            return;
        }
        

        // eger monte edilmişse ve kaldırmak istiyosak engelleyen obje var mı (örnegin civatalar takıldıysa civataları çıkarmadan içindekı nesnelerı çıkaramayız.)
        // varsa o objeleri işaretler ve hata mesajı verir.
        if(isStick)
        {
            if(CheckIfObstacles())
            {
                FailAlert(extractionAlert);
                return;
            }
            transformData.detectTargets[detectTargetIndex].isFull = false;
        }

        // objenın gıdecegi yer eger dolu degılse şeffaf silüeti çıkar.---
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
        // ---
        isSelected= true;
        task.IsDone = false;
        transform.parent  = null;

        // hareket pozisyonları ayarlanıyor.
        startDragPos = transform.position;
        zCoordinate = cam.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
    }
    private void OnMouseUp() {
        if(!isSelected) return;

        // eger monte edcegimiz objeyı engelleyen objeler varsa o objeleri işaretler ve hata mesajı verir. 
        // (ornegın civataları takılmış yerin içine obje yerlestirmek istersek civataları işaretler ve hata mesajı verir.)
        if(isStick)
        {
            if(CheckIfObstacles() )
            {
                FailAlert(stickAlert);
                
                transform.position = startDragPos;
                if(transformData.target == null)
                {
                    foreach (var item in transformData.detectTargets)
                    {
                        
                        item.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                else
                    transformData.target.GetComponent<MeshRenderer>().enabled = false;
                return;
            }
        
        }
        isSelected = false;
        Stick();
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
        if(!isSelected)
        {
            return;
        }
        
        for (int i = 0; i < transformData.detectTargets.Count; i++)
        {
            // yapışma mesafesindeyse
            if(Vector3.Distance(transformData.detectTargets[i].transform.position , transform.position)<stickDistance)
            {
                if(transformData.detectTargets[i].isFull)
                    continue;
                if(transformData.target == null)
                {
                    target = transformData.detectTargets[i].transform; 
                }

                isStick = true;
                detectTargetIndex = i;
                outline.OutlineColor = Color.green;
                break;
            }
            // yapışma mesafesınde degılse
            else
            {
                if(transformData.target == null)
                {
                    target = null;
                }
                
                isStick = false;
                outline.OutlineColor = Color.white;
            }
        }
    }
    private void FailAlert(string alertText)
    {
        AlertManager.Instance.ShowAlert(alertText);
        outline.enabled = false;
        SoundManager.Instance.FailSoundEffect();
    }
    public void Stick()
    {
        if(isStick)
        {
            task.IsDone = true;
            transformData.detectTargets[detectTargetIndex].isFull = true;
            if(CheckIfObstacles())
            {
                FailAlert(stickAlert);
                isStick = false;
                return;
            }
            // animasyon ---
            GameManager.Instance.isInAnimation = true;
            SoundManager.Instance.SuccessSoundEffect();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(transformData.detectTargets[detectTargetIndex].GetAnimPos(),.5f).OnComplete(()=>AnimationEvent?.Invoke(1f)));
            sequence.Append(transform.DOMove(target.transform.position,.5f).OnComplete(()=>
            {
                GameManager.Instance.isInAnimation = false;
                TaskManager.Instance.CheckFinish();
            }));
            // ---


            MakeParent();
        }
        else
        {
            transformData.detectTargets[detectTargetIndex].isFull = false;
            task.IsDone = false;
        }
    }
    /// <summary>
    /// objeyi monte ederken veya çıkarırken engelleyen objeler varsa o objelerin outline'larını kırmızı yapar.
    /// </summary>
    /// <returns></returns>
    public bool CheckIfObstacles()
    {
        List<Order> tasks =  TaskManager.Instance.GetTasks(taskSide,taskIndex);
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
    public void Restart()
    {
        isStick = false;
        foreach (var item in transformData.detectTargets)
        {
            item.isFull = false;
        }
    }
}
