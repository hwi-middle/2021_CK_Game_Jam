using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;

    public Transform body;

    Quaternion camRotation;
    Quaternion bodyRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camRotation = transform.localRotation;
        bodyRotation = body.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float yRotation = Input.GetAxis("Mouse X") * sensitivityX;
        float xRotation = Input.GetAxis("Mouse Y") * sensitivityY;

        //X�� ȸ������ -90 ~ +90���� �����ϴ� Clamp �ʿ���

        camRotation *= Quaternion.Euler(-xRotation, 0f, 0f);
        bodyRotation *= Quaternion.Euler(0f, yRotation, 0f);

        transform.localRotation = camRotation;
        body.localRotation = bodyRotation;
    }
}
