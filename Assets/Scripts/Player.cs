using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed; //параметр скорости
    public float jumpHeight; //высота прыжка
    public Transform groundCheck;
    private bool isGrounded;
    private CapsuleCollider2D _box;
    private Animator anim;
    private static readonly int State = Animator.StringToHash("State");

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _box = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //прыжок при нажатии пробела
        //     rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); //добовляем импульс

        Vector3 max = _box.bounds.max;
        Vector3 min = _box.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(max.x, min.y - .2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        isGrounded = false;
        if (hit != null)
            isGrounded = true;
        
        //Переключение анимаций
        if(Input.GetAxis("Horizontal") == 0 && isGrounded)
            anim.SetInteger(State, 1); //покой
        else
        {
            Flip(); 
            if (isGrounded)
                anim.SetInteger(State, 2); //ходьба
        }
        
        if (isGrounded == false)
            anim.SetInteger(State, 3); //прыжок

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //прыжок при нажатии пробела
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); //добовляем импульс
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y); //перемещаем персонажа по горизонатали
    }

    void Flip() //поворачиваем объект
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    // private void CheckGround() //проверка стоит герой на земле или нет
    // {
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
    //     isGrounded = colliders.Length > 1;
    // }
}
