using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float repulsiveForce = 8f; //сила отталкивания от врага
    public bool isAttacking = true;

    public void ToDestroy()
    {
        isAttacking = false;
        GetComponent<Animator>().SetInteger("State", 2); //Включаем анимацию смерти
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 0.8f);
        GetComponent<Enemy>().isAttacking = false;
        Destroy(gameObject, 1f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && isAttacking)
        {
            print("Hit");
            other.gameObject.GetComponent<Player>().RecountHP(-1);
            
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * repulsiveForce,
                ForceMode2D.Impulse); //добовляем импульс
        }
    }
}