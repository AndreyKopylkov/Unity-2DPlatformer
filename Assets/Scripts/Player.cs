﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed; //параметр скорости
    public float jumpHeight = 13; //высота прыжка
    private Transform tf;
    private bool isGrounded;
    private CapsuleCollider2D _box;
    private Animator anim;
    private static readonly int State = Animator.StringToHash("State");
    private int curHP; //текущее хп
    private int maxHP = 3; //максимальное хп игрока
    private bool isHit = false;
    private float loseTime = 1f; //отсчет для перезапуска сцены
    public Main main;
    private float rayHitDistance = 0.75f;
    public Transform rayHelper;
    private float repulsiveForce = 12f; //сила отталкивания от врага
    private bool isImmortal; //режим бессмертия (после удара или получения урона)
    private float immortalTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _box = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        tf = GetComponent<Transform>();
        curHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //кидаем луч для проверки столкновения снизу игрока
        RayForDestroy();
        
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //прыжок при нажатии пробела
            rb.velocity = Vector3.zero; //приравнивает скорость к нулю
            rb.AddForce(tf.up * jumpHeight, ForceMode2D.Impulse); //добовляем импульс
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y); //перемещаем персонажа по горизонатали
    }

    void Flip() //поворачиваем объект
    {
        if (Input.GetAxis("Horizontal") > 0)
            tf.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            tf.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public void RecountHP(int deltaHP)
    {
        if (deltaHP < 0 && isImmortal == false)
        {
            curHP = curHP + deltaHP;
            StartCoroutine(ImmortalEffect());
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f); //меняет цвет игрока при уроне
            StopCoroutine(OnHit());
            isHit = true;
            StartCoroutine(OnHit());
            print(curHP);
        }
        else
        {
            StartCoroutine(ImmortalEffect());
            curHP = curHP + deltaHP;
            print(curHP);
        }

        if (curHP <= 0)
        {
            print("you are death");
            GetComponent<CapsuleCollider2D>().enabled = false;
            tf.eulerAngles = new Vector3(0, tf.eulerAngles.y, 90);
            Invoke("Lose", loseTime);// вызов проигрыша через loseTime сек
        }

    }

    IEnumerator OnHit()
    {
        if (isHit)
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f,
                GetComponent<SpriteRenderer>().color.b + 0.04f); //меняет цвет игрока при уроне

        if (GetComponent<SpriteRenderer>().color.g == 1)
        { 
            isHit = false; 
            StopCoroutine(OnHit());
        }

        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    }

    void Lose() //вызов метода другого класса
    {
        main.GetComponent<Main>().Lose();
    }

    private void RayForDestroy()
    {
        RaycastHit2D rayHit;
        int layerMask = ~(LayerMask.GetMask("Player")); //маска для всех слоев, кроме игрока
        rayHit = Physics2D.Raycast(rayHelper.position, Vector3.right, rayHitDistance, layerMask); //луч
        Debug.DrawRay(rayHelper.position, transform.TransformDirection(Vector3.right) * rayHitDistance,
            Color.yellow); //проверка пуска луча

        if (rayHit.collider != null) //проверка столкновения луча
        {
            //если луч попал в объект с тегом Enemy
            if (rayHit.collider.CompareTag("Enemy")) //проверяет враг это или нет
            {
                StartCoroutine(ImmortalEffect());
                
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.up * repulsiveForce, ForceMode2D.Impulse); //добовляем импульс
                GameObject rayObject = rayHit.transform.gameObject;
                Enemy target = rayObject.GetComponent<Enemy>();
                target.ToDestroy(); //вызов метода на Enemy
                print("Попадаю во врага!!!");
                
            }
            // //если луч попал куда-то в другой объект
            // else
            //     Debug.Log("Путь к врагу преграждает объект: " + rayHit.collider.name);
        }
    }

    IEnumerator ImmortalEffect()
    {
        
        isImmortal = true;
        Debug.Log("ImmortalEffect true");
        yield return new WaitForSeconds(immortalTime);
        Debug.Log("ImmortalEffect false");
        isImmortal = false;
    }

    // private void CheckGround() //проверка стоит герой на земле или нет
    // {
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
    //     isGrounded = colliders.Length > 1;
    // }
}
