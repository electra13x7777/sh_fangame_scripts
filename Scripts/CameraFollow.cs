using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private Vector3 velocity;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        // instance based
        /*
        if (GameManager.Instance.is_player_turn()) 
        {
        
        }
        
        // singleton based
        if (GameManager.is_player_turn()) 
        {
        
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, time);
    }
}
