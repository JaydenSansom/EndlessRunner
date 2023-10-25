using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnLoad : MonoBehaviour
{
    [SerializeField, Tooltip("this object's renderer")] private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        if(!renderer){
            renderer = GetComponent<Renderer>();
        }
        if(renderer){
            renderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
