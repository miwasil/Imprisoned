using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prerender : MonoBehaviour
{
    private GameObject[] portals_to_render;

    private List<Portal> portal_scriptt=new List<Portal>();
    // Start is called before the first frame update
    void Awake()
    {
        portals_to_render = GameObject.FindGameObjectsWithTag("Portal");
        for (int i = 0; i < portals_to_render.Length; i++)
        {
            portal_scriptt.Add(portals_to_render[i].GetComponent<Portal>());
        }
        Debug.Log(portal_scriptt.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPreRender()
    {
        
    }

    void OnPreCull()
    {
        for (int i = 0; i < portals_to_render.Length; i++)
        {
            portal_scriptt[i].Renderr();
        }
    }
}
