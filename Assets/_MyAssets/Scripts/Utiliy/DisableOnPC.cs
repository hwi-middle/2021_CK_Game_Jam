using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnPC : MonoBehaviour
{
    void Awake()
    {
#if UNITY_STANDALONE_WIN
        gameObject.SetActive(false);
#endif
    }
}
