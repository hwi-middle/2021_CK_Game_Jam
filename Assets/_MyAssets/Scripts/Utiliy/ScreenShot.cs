using System;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ScreenCapture.CaptureScreenshot("ScreenShot" + DateTime.Now.Second + DateTime.Now.Millisecond + ".png");
            Debug.Log("스크린샷저장");
        }
    }
}