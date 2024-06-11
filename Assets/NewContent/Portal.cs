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

    private FirstPersonLook player_rotation_setter; //UWAGA czeba ustawic
    private Camera player_camera;
    private Camera my_camera;

	private List<GameObject> children = new List<GameObject>();

    private RenderTexture view_texture;

    private MeshRenderer camera_target;

    [SerializeField]private GameObject heroo;
    
    //travellers
    private List<GameObject> objects_to_watch = new List<GameObject>();
    private List<double> objects_to_watch_dprod = new List<double>();
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
        player_camera = heroo.GetComponentInChildren<Camera>();
        my_screan=transform.Find("PortalScreen").gameObject;
        my_camera=transform.Find("Camera").GetComponent<Camera>();
        dest_screen = dest.transform.Find("PortalScreen").gameObject;
        view_texture = new RenderTexture(Screen.width, Screen.height, 0);
        my_camera.targetTexture = view_texture;
        camera_target = dest_screen.GetComponent<MeshRenderer>();
        camera_target.material.SetTexture("_MainTex",view_texture);
        my_camera.enabled = false;
        
        player_rotation_setter = player_camera.GetComponent<FirstPersonLook>();
    }

    public void Renderr()
    {
        my_screan.SetActive(false);
        set_camera_position();
        set_near_clip_plane();
        my_camera.Render();
        my_screan.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        //set_camera_position();
        //teleport_stuff();
    }

    public void set_camera_position()
    {
        //Vector3 tmp_pos = player_camera.transform.position - me.transform.position;
        //my_camera.transform.position = dest_screen.transform.position + tmp_pos;
        //my_camera.transform.rotation = player_camera.transform.rotation;
        //my_camera.GetComponentInChildren<GameObject>().transform.position = my_camera.transform.position;
        
        Vector3 cam_vector = player_camera.transform.position - dest_screen.transform.position;
        cam_vector = Quaternion.Euler(my_screan.transform.eulerAngles - dest_screen.transform.eulerAngles) * cam_vector;
        my_camera.transform.position = my_screan.transform.position + cam_vector;
        my_camera.transform.eulerAngles = player_camera.transform.eulerAngles + my_screan.transform.eulerAngles - dest_screen.transform.eulerAngles;
        
        // Vector3 entr_player_vector = player.transform.position - me.transform.position;
        // entr_player_vector =
        //     Quaternion.Euler(dest_screen.transform.eulerAngles - me.transform.eulerAngles) * entr_player_vector;
        // dummy.transform.position = dest_screen.transform.position + entr_player_vector;
        // dummy.transform.eulerAngles =
        //     player.transform.eulerAngles + dest_screen.transform.eulerAngles - me.transform.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            objects_to_watch.Add(other.gameObject);
            Vector3 player_portal_vector = other.gameObject.transform.position - transform.position;
            objects_to_watch_dprod.Add(Vector3.Dot(transform.forward, player_portal_vector));
        }
    }

    private void teleport_stuff()
    {
        for (int i = 0; i < objects_to_watch.Count; i++)
        {
            player = objects_to_watch[i];
            Vector3 player_portal_vector = player.transform.position - transform.position;
            float dot_now = Vector3.Dot(transform.forward, player_portal_vector);
            //Debug.Log(dot_now);
            //Debug.Log(objects_to_watch_dprod[i]);
            if (Math.Sign(dot_now) != Math.Sign(objects_to_watch_dprod[i]))
            {
                objects_to_watch.Remove(player);
                
                if (player.tag == "Player")
                {
                    Vector3 tmp = player.transform.position - my_screan.transform.position;
                    player.transform.position = dest.transform.position;
                    player.transform.eulerAngles = dest.transform.eulerAngles - my_screan.transform.eulerAngles;
                    
                    // Vector3 player_rot = dest.transform.eulerAngles - my_screan.transform.eulerAngles;
                    // player_rotation_setter.SetCameraRotation(new Vector2(player_rot.x, player_rot.y));
                    
                    player.GetComponent<Rigidbody>().velocity = Quaternion.Euler(dest.transform.eulerAngles - my_screan.transform.eulerAngles) * player.GetComponent<Rigidbody>().velocity;
                    tmp = Quaternion.Euler(dest.transform.eulerAngles - my_screan.transform.eulerAngles) * tmp;
                    player.transform.position += tmp;   
                }
                else
                {
                    Vector3 tmp = player.transform.position - my_screan.transform.position;
                    player.transform.position = dest.transform.position;
                    player.transform.eulerAngles += dest.transform.eulerAngles - my_screan.transform.eulerAngles;
                    player.GetComponent<Rigidbody>().velocity =
                        Quaternion.Euler(dest.transform.eulerAngles - my_screan.transform.eulerAngles) *
                        player.GetComponent<Rigidbody>().velocity;
                    tmp = Quaternion.Euler(dest.transform.eulerAngles - my_screan.transform.eulerAngles) * tmp;
                    player.transform.position += tmp;
                    //Debug.Log(player.name);
                }
            }

            //objects_to_watch_dprod[i] = dot_now;
        }

        // dest.GetComponent<Portal>().disable = true;
        // player = other.gameObject;

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
    private void OnTriggerExit(Collider other)
    {
        int tmp = objects_to_watch.IndexOf(other.gameObject);
        objects_to_watch.Remove(other.gameObject);
        if (tmp != -1)
        {
            objects_to_watch_dprod.RemoveAt(tmp);
        }
    }

    void set_near_clip_plane()
    {
        // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
        Transform clipPlane = transform;
        //Vector3 tmp = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - my_camera.transform.position));
        Vector3 camSpacePos = my_camera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = my_camera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + (float)transform.localScale.z + 0.02f;
        Vector4 CPCS = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);
        my_camera.projectionMatrix = player_camera.CalculateObliqueMatrix(CPCS);
    }

    void FixedUpdate()
    {
        teleport_stuff();
    }
}