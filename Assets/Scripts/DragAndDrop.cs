using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    Outline outline;
    Camera cam;
    
    [SerializeField] private Vector3 offset;
    [SerializeField] private float zCoordinate;

    private void Start() {
        cam = Camera.main;
        outline = GetComponent<Outline>();
    }
    private void OnMouseDown() {
        zCoordinate = cam.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
    }
    private void OnMouseDrag() {
        transform.position = GetMouseWorldPos() + offset;
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
