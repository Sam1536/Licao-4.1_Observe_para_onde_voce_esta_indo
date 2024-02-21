using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rig;
    private GameObject player;

    public float speed;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        rig.AddForce(lookDirection * speed);  

        if(transform.position.y < -5)
        {
            Destroy(gameObject);
        }

        

    }
}
