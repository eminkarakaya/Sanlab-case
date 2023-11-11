using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{ 
    private const string MOUSE_X = "Mouse X",MOUSE_Y = "Mouse Y";
    private const string MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
    [Header("Panning Clamp")]
    [SerializeField] private float xMin;
    [SerializeField] private float xMax,yMin,yMax,zMin,zMax,cameraPanningSensitivity;

    [Space(20)]
    [SerializeField] private float _smoothTime = 3f;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minZoom,_maxZoom;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _mouseSensitivity = 3.0f;
    [SerializeField] private Transform _target;

    private float _rotationY,_rotationX;
    private Vector3 _currentRotation;
    private Camera _camera;
    private Vector3 _smoothVelocity = Vector3.zero;
    [SerializeField] private float _distanceFromTarget;
    private Vector3 startPixel;
    private Vector3 currentPosition;
    private Vector3 deltaPositon;
    private Vector3 lastPositon;

    private void Start() {
        _camera= Camera.main;
    }
    private void Update() {
        // ----------------------------------------
        float zoomAmount = Input.GetAxis(MOUSE_SCROLLWHEEL);
        if(zoomAmount != 0)
        {
            _distanceFromTarget -= zoomAmount*_zoomSpeed;
            _distanceFromTarget = Mathf.Clamp(_distanceFromTarget,_minZoom,_maxZoom);
        }
        if(Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis(MOUSE_X)*_mouseSensitivity;
            float mouseY = Input.GetAxis(MOUSE_Y)*_mouseSensitivity;

            _rotationY += mouseX;
            _rotationX -= mouseY;

            _rotationX = Mathf.Clamp(_rotationX,-40,40);

        }
        Vector3 nextRotation = new Vector3(_rotationX,_rotationY);

        _currentRotation = Vector3.SmoothDamp(_currentRotation,nextRotation,ref _smoothVelocity,_smoothTime);
        transform.localEulerAngles = new Vector3(_rotationX,_rotationY,0);
        transform.position = _target.position - transform.forward * _distanceFromTarget;

        currentPosition = Input.mousePosition;
        deltaPositon = currentPosition-lastPositon;
        lastPositon = currentPosition;
        _target.transform.rotation = _camera.transform.rotation;
        
        if(Input.GetMouseButton(2))
        {

            _target.transform.Translate(-deltaPositon * Time.deltaTime*cameraPanningSensitivity,Space.Self);
            Vector3 pos = _target.transform.position;
            pos.x = Mathf.Clamp(pos.x,xMin,xMax); 
            pos.y = Mathf.Clamp(pos.y,yMin,yMax); 
            pos.z = Mathf.Clamp(pos.z,zMin,zMax); 
            _target.transform.position = pos;
        }
        




























        // ----------------------------------------

        
        // [SerializeField] private bool isMoveStarting;
        // [SerializeField] private Vector3 dragStartPos;
        // [SerializeField] private Vector3 dragCurrPos;
        // [SerializeField] private Vector3 newPos;

        // if(Input.GetMouseButtonDown(0))
        // {
        //     isMoveStarting = true;
        //     Plane plane = new Plane(Vector3.up, Vector3.zero);
        //     Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        //     float entry;
        //     if(plane.Raycast(ray,out entry))
        //     {
        //         dragStartPos = ray.GetPoint(entry);
        //     }
        // }
        // else if(Input.GetMouseButton(0))
        // {
        //     if(!isMoveStarting)
        //     {
        //         isMoveStarting = true;
        //         Plane _plane = new Plane(Vector3.up, Vector3.zero);
        //         Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);
        //         float _entry;
        //         Debug.Log("kekw");
        //         if(_plane.Raycast(_ray,out _entry))
        //         {
        //             dragStartPos = _ray.GetPoint(_entry);
        //         }
        //     }
        //     Plane plane = new Plane(Vector3.up, Vector3.zero);
        //     Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        //     float entry;
        //     if(plane.Raycast(ray,out entry))
        //     {
        //         dragCurrPos = ray.GetPoint(entry);
        //         newPos = _target.transform.position + dragStartPos - dragCurrPos;
        //     }
        //     _target.transform.position = newPos;
        // }
        
    }

}
