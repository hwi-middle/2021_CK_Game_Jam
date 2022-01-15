using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnMobile : MonoBehaviour
{
    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        gameObject.SetActive(false);
#endif
    }
}
