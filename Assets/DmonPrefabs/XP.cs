using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    public Rigidbody playerRb;
    public Transform playerTr;
    public Transform gravityPoint;

    public float force = 7; // постоянное ускорение

    private bool CanGravity=false;

    private void Start()
    {
        gravityPoint = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CanGravity = true;
        }
    }
    private void Update()
    {
        if (CanGravity)
        {
            playerRb.velocity=(  gravityPoint.position- transform.position).normalized*Time.deltaTime*force;
        }
    }

}
