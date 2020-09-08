// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class Destroyable : MonoBehaviour
// {
//     private float repulsiveForce = 8f; //сила отталкивания от врага
//     
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.gameObject.tag == "Player")
//         {
//             other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * repulsiveForce, ForceMode2D.Impulse); //добовляем импульс
//             GetComponent<Animator>().SetInteger("State", 2); //Включаем анимацию смерти
//             GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 0.8f);
//             GetComponent<Enemy>().isAttacking = false;
//             Destroy(gameObject, 1f);
//         }
//     }
// }
