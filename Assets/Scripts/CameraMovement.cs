using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _smoothVelocity = Vector3.zero;
    [SerializeField] private float _smoothTime = 3f;
    private Vector3 _currentRotation;
    [SerializeField] private float _moveSpeed;
    Camera _camera;
    [SerializeField] private float _minZoom,_maxZoom;
    private const string MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
    private const string MOUSE_X = "Mouse X",MOUSE_Y = "Mouse Y";
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _mouseSensitivity = 3.0f;
    private float _rotationY,_rotationX;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceFromTarget;
    [SerializeField] private Vector3 startPixel;

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

        if(Input.GetMouseButtonDown(0))
        {

        }
        else if(Input.GetMouseButton(0))
        {
            
        }
        else if(Input.GetMouseButtonUp(0))
        {

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
