using System.Collections;
using System.Collections.Generic;
using MimicSpace;
using UnityEngine;

public class ResetMimicSpeed : MonoBehaviour
{
    //private List<Movement> movement_list = new List<Movement>();
    public Mimic mimic1;
    public Mimic mimic2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mimic1.GetComponent<Movement>().ResetMimicSpeed();
            mimic2.GetComponent<Movement>().ResetMimicSpeed();
        }
    }
}
