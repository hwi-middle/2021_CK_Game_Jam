using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckButtonPressed : MonoBehaviour
{
    [HideInInspector] public bool pressed = false;

    public void Press( )
    {
        pressed = true;
    }
}