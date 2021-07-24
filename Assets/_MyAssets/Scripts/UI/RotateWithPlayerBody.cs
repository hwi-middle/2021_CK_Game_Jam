using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithPlayerBody : MonoBehaviour
{
    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(90, player.transform.rotation.eulerAngles.y, 0);
        //player.transform.rotation;
    }
}
