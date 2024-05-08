using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject ObjectOnPlayer;
    public GameObject Text;
    
    void Start()
    {
        ObjectOnPlayer.SetActive(false);
        Text.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Text.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);
                ObjectOnPlayer.SetActive(true);
                Text.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Text.SetActive(false);
    }
}
