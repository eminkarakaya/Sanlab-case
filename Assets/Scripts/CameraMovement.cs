using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{ 
    private const string MOUSE_X = "Mouse X",MOUSE_Y = "Mouse Y";
    private const string MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
    [Header("Panning Clamp")]
    [SerializeField] private float xMin;
    [SerializeField] private float xMax,yMin,yMax,zMin,zMax,cameraPanningSensitivity;

    [Space(20)]
    [SerializeField] private float cameraAnimationDuration;
    [SerializeField] private Transform cameraFinishPoint;
    [SerializeField] private float smoothTime = 3f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minZoom,maxZoom;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float mouseSensitivity = 3.0f;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromTarget;

    private float rotationY,rotationX;
    private Vector3 currentRotation;
    private Camera _camera;
    private Vector3 smoothVelocity = Vector3.zero;
    private Vector3 currentPosition;
    private Vector3 deltaPositon;
    private Vector3 lastPositon;


    private void Start() {
        _camera= Camera.main;
    }

    private void Update() {
        if(TaskManager.instance.isFinish) 
            return;
        float zoomAmount = Input.GetAxis(MOUSE_SCROLLWHEEL);
        if(zoomAmount != 0)
        {
            distanceFromTarget -= zoomAmount*zoomSpeed;
            distanceFromTarget = Mathf.Clamp(distanceFromTarget,minZoom,maxZoom);
        }
        if(Input.GetMouseButton(1)) // rotate
        {
            float mouseX = Input.GetAxis(MOUSE_X)*mouseSensitivity;
            float mouseY = Input.GetAxis(MOUSE_Y)*mouseSensitivity;

            rotationY += mouseX;
            rotationX -= mouseY;

            rotationX = Mathf.Clamp(rotationX,-40,40);

        }
        Vector3 nextRotation = new Vector3(rotationX,rotationY);

        currentRotation = Vector3.SmoothDamp(currentRotation,nextRotation,ref smoothVelocity,smoothTime);
        transform.localEulerAngles = new Vector3(rotationX,rotationY,0);
        transform.position = target.position - transform.forward * distanceFromTarget;

        currentPosition = Input.mousePosition;
        deltaPositon = currentPosition-lastPositon;
        lastPositon = currentPosition;
        target.transform.rotation = _camera.transform.rotation;
        
        if(Input.GetMouseButton(2)) // movement
        {

            target.transform.Translate(-deltaPositon * Time.deltaTime*cameraPanningSensitivity,Space.Self);
            Vector3 pos = target.transform.position;
            pos.x = Mathf.Clamp(pos.x,xMin,xMax); 
            pos.y = Mathf.Clamp(pos.y,yMin,yMax); 
            pos.z = Mathf.Clamp(pos.z,zMin,zMax); 
            target.transform.position = pos;
        }
    }

     
    [ContextMenu("CamAnimationNoParam")]
    public void CamAnimationNoParam(){
        transform.DOMove(cameraFinishPoint.position,cameraAnimationDuration);
        transform.DORotate(cameraFinishPoint.rotation.eulerAngles,cameraAnimationDuration).OnComplete(()=>StartCoroutine(RotateAroundFinishAnim(2)));
        GetComponent<CameraMovement>().enabled = false;
    }
    public void CamAnimation(float dur ){
        target.transform.position = Vector3.zero;
        transform.DOMove(cameraFinishPoint.position,dur);
        transform.DORotate(cameraFinishPoint.rotation.eulerAngles,dur).OnComplete(()=>StartCoroutine(RotateAroundFinishAnim(dur)));
        GetComponent<CameraMovement>().enabled = false;
    }
    IEnumerator RotateAroundFinishAnim(float dur)
    {
        var passedTime = 0f;
        while(passedTime<dur)
        {
            passedTime += Time.deltaTime;
            transform.RotateAround(target.transform.position, Vector3.up , 200 * Time.deltaTime);
            yield return null;
        }
    }
}
