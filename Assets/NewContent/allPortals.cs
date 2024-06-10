using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allPortals : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> all_portals = new List<GameObject>();
    private int portal_count;
    
    private GameObject a;
    
    private List<Camera> all_cameras = new List<Camera>();
    void Start()
    {
        
        portal_count = transform.childCount;
        for (int i = 0; i < portal_count; i++)
        {
            a = transform.GetChild(i).gameObject;
            //Debug.Log(a.GetType());
            Debug.Log(a.name);
            all_portals.Add(a);
            //all_portals.Add(transform.GetChild(i).gameObject);
            //print(a.GetComponentInChildren<Camera>().name);
            all_cameras.Add((a.GetComponentInChildren<Camera>()));
            Debug.Log(all_portals[i].GetComponent<Portal>());

        }
    }

    void Update()
    {
        for (int i = 0; i < portal_count; i++)
        {
            all_portals[i].GetComponent<Portal>().Renderr();
        }
 
    }
    
    // Update is called once per frame
    // void Update()
    // {
    //     // for (int i = 0; i < portal_count; i++)
    //     {
    //         //print(i);
    //         all_portals[i].SetActive(false);
    //         //all_cameras[i].Render();
    //         //all_portals[i].SetActive(true);
    //     }
    //     for (int i = 0; i < portal_count; i++)
    //     {
    //         //print(i);
    //         //all_portals[i].SetActive(false);
    //         all_cameras[i].Render();
    //         all_portals[i].SetActive(true);
    //     }
    // }
}
