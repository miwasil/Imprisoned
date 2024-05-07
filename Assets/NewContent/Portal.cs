using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider me;

    public bool disable = false;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dest;

    private bool a;

    [SerializeField] private Camera player_camera;
    private Camera my_camera;
    [SerializeField] private GameObject dummy;
    void Awake()
    {
        me = GetComponent<BoxCollider>();
        //dummy = GetComponentInChildren<GameObject>();
        //my_camera = GetComponentInChildren<Camera>(); //????
    }

    // Update is called once per frame
    void Update()
    {
        //set_camera_position();
    }

    void set_camera_position()
    {
        //Vector3 tmp_pos = player_camera.transform.position - me.transform.position;
        //my_camera.transform.position = dest.transform.position + tmp_pos;
        //my_camera.transform.rotation = player_camera.transform.rotation;
        //my_camera.GetComponentInChildren<GameObject>().transform.position = my_camera.transform.position;
        Vector3 entr_player_vector = player.transform.position - me.transform.position;
        entr_player_vector =
            Quaternion.Euler(dest.transform.eulerAngles - me.transform.eulerAngles) * entr_player_vector;
        dummy.transform.position = dest.transform.position + entr_player_vector;
        dummy.transform.eulerAngles =
            player.transform.eulerAngles + dest.transform.eulerAngles - me.transform.eulerAngles;
    }

    private void OnTriggerEnter()
    {
        if (!disable)
        {
            dest.GetComponent<Portal>().disable = true;
            
            Vector3 tmp = player.transform.position - me.transform.position;

            //player.transform.position += dest.transform.position - me.transform.position;
            player.transform.position = dest.transform.position;
            player.transform.eulerAngles += dest.transform.eulerAngles - me.transform.eulerAngles;
            //player.transform.rotation *= dest.transform.rotation / me.transform.rotation;
            player.GetComponent<Rigidbody>().velocity = Quaternion.Euler(dest.transform.eulerAngles - me.transform.eulerAngles) * player.GetComponent<Rigidbody>().velocity;
            tmp = Quaternion.Euler(dest.transform.eulerAngles - me.transform.eulerAngles) * tmp;
            player.transform.position += tmp;
            //Debug.Log(player.GetComponent<Rigidbody>().velocity);
        }
    }
    
    private void OnTriggerExit()
    {
        disable = false;
    }
}