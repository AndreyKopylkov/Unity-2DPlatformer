using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed; //параметр скорости
    public float jumpHeight; //высота прыжка
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y); //перемещаем персонажа по горизонатали
        if (Input.GetKeyDown(KeyCode.Space)) //прыжок при нажатии пробела
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); //добовляем импульс
    }

    void Flip() //поворачиваем объект
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}
