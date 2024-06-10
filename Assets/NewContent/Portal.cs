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
    private GameObject player;
    //private GameObject player;
    [SerializeField] private GameObject dest;
    private GameObject dest_screen;
    private GameObject my_screan;
    private GameObject a;


    [SerializeField] private Camera player_camera;
    private Camera my_camera;

	private List<GameObject> children = new List<GameObject>();


    void Awake()
    {
        // me = GetComponent<BoxCollider>();
        // //dummy = GetComponentInChildren<GameObject>();
        // my_camera = GetComponentInChildren<Camera>(); //????
        // my_screan = GetComponentInChildren<GameObject>();
        // Debug.Log(my_screan.GetType());
        
  //       int portal_count = transform.childCount;
  //       Debug.Log(portal_count);
  //       for (int i = 0; i < portal_count; i++)
  //       {
  //           a = transform.GetChild(i).gameObject;
  //           Debug.Log(a.GetType());
  //           //Debug.Log(a.name);
  //           //children.Add(a);
		// }
        my_screan=transform.Find("PortalScreen").gameObject;
        my_camera=transform.Find("Camera").GetComponent<Camera>();
        dest_screen = dest.transform.Find("PortalScreen").gameObject;
    }

    public void Renderr()
    {
        dest_screen.SetActive(false);
        set_camera_position();
        my_camera.Render();
        dest_screen.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        //set_camera_position();
    }

    public void set_camera_position()
    {
        //Vector3 tmp_pos = player_camera.transform.position - me.transform.position;
        //my_camera.transform.position = dest_screen.transform.position + tmp_pos;
        //my_camera.transform.rotation = player_camera.transform.rotation;
        //my_camera.GetComponentInChildren<GameObject>().transform.position = my_camera.transform.position;
        
        Vector3 cam_vector = player_camera.transform.position - my_screan.transform.position;
        cam_vector = Quaternion.Euler(dest_screen.transform.eulerAngles - my_screan.transform.eulerAngles) * cam_vector;
        my_camera.transform.position = dest_screen.transform.position + cam_vector;
        my_camera.transform.eulerAngles = player_camera.transform.eulerAngles + dest_screen.transform.eulerAngles - my_screan.transform.eulerAngles;
        
        // Vector3 entr_player_vector = player.transform.position - me.transform.position;
        // entr_player_vector =
        //     Quaternion.Euler(dest_screen.transform.eulerAngles - me.transform.eulerAngles) * entr_player_vector;
        // dummy.transform.position = dest_screen.transform.position + entr_player_vector;
        // dummy.transform.eulerAngles =
        //     player.transform.eulerAngles + dest_screen.transform.eulerAngles - me.transform.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!disable)
        {
            dest.GetComponent<Portal>().disable = true;
            //other.transform.position += new Vector3(0, 10, 0);
            player = other.gameObject;
            Vector3 tmp = player.transform.position - my_screan.transform.position;
            //player.transform.position += dest.transform.position - me.transform.position;
            player.transform.position = dest.transform.position;
            player.transform.eulerAngles += dest.transform.eulerAngles - my_screan.transform.eulerAngles;
            //player.transform.rotation *= dest.transform.rotation / me.transform.rotation;
            player.GetComponent<Rigidbody>().velocity = Quaternion.Euler(dest.transform.eulerAngles - my_screan.transform.eulerAngles) * player.GetComponent<Rigidbody>().velocity;
            tmp = Quaternion.Euler(dest.transform.eulerAngles - my_screan.transform.eulerAngles) * tmp;
            player.transform.position += tmp;
<<<<<<< HEAD
            //Debug.Log(player.GetComponent<Rigidbody>().velocity);
=======
            Debug.Log(player.name);
            
>>>>>>> a447125efe3f3dea3308c1f77e647c724230cd2b
            //player_camera.transform.eulerAngles += dest.transform.eulerAngles - me.transform.eulerAngles;
        }
    }

    // private void OnDisable()
    // {
    //     Debug.Log("hoy");
    //     set_camera_position();
    // }
    //
    // private void OnRender()
    // {
    //     Debug.Log("hoy");
    //     set_camera_position();
    // }
    
    private void OnTriggerExit()
    {
        disable = false;
    }
}