using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pressed = false;
    private int pointerId = -1;
    private Vector2 prevPointerPosition = Vector2.zero;
    [HideInInspector] public Vector2 dragDiffPerFrame = Vector2.zero;
    private readonly Vector2 zeroVector = Vector2.zero;

    void Start()
    {
    }

    void Update()
    {
        if (pressed)
        {
            if (pointerId >= Input.touches.Length || pointerId < 0) return;
            dragDiffPerFrame = Input.touches[pointerId].position - prevPointerPosition;
            prevPointerPosition = Input.touches[pointerId].position;
        }
        else
        {
            dragDiffPerFrame = zeroVector;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        prevPointerPosition = eventData.position;
        pointerId = eventData.pointerId;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
}