using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItems : MonoBehaviour
{
    ItemHolder itemHolder;

    // Start is called before the first frame update
    void Start()
    {
        itemHolder = ItemHolder.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(itemHolder.TryGetUSB())
            {
                Destroy(gameObject);
                Debug.Log("æ∆¿Ã≈€ »πµÊ!");
            }
        }
    }
}
