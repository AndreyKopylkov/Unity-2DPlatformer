using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float repulsiveForce = 8f; //сила отталкивания от врага
    public bool isAttacking = true; 
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && isAttacking)
        {
            print("Hit");
            other.gameObject.GetComponent<Player>().RecountHP(-1);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * repulsiveForce, ForceMode2D.Impulse); //добовляем импульс
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}