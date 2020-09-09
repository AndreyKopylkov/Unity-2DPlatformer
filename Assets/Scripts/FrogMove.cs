using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMove : MonoBehaviour
{
    private bool moveLeft = true;
    private RaycastHit2D groundInfo;
    public Transform groundDetect; //объект, из которого кидается луч
    private float distanceRay = 3.5f; //длина кидаемого луча
    private float waitTime = 1.5f; //время ожидания для прыжка
    private float jumpForce = 4f; //импульс прыжка 
    private bool jumpReady = true; //готовность нового прыжка
    private Vector3 jumpPoint;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetInteger("State", 3); //включаем анимацию idle
    }

    // Update is called once per frame
    void Update()
    {
        groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, distanceRay); //проверка обрыва
        Debug.DrawRay(groundDetect.position, transform.TransformDirection(Vector3.down) * distanceRay,
            Color.red); //проверка пуска луча

        if (groundInfo.collider == false) //изменение направления движения
        {
            if (moveLeft)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                moveLeft = false;
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                moveLeft = true;
            }
        }

        if (jumpReady)
        {
            StartCoroutine(JumpTimer());
        }
        
    }

    IEnumerator JumpTimer()
    {
        jumpReady = false;
        GetComponent<Animator>().SetInteger("State", 3);
        yield return new WaitForSeconds(waitTime);
        GetComponent<Animator>().SetInteger("State", 1);
        jumpPoint = groundDetect.position - gameObject.transform.position; //точка для импульса
        gameObject.GetComponent<Rigidbody2D>().AddForce(jumpPoint * jumpForce,
            ForceMode2D.Impulse); //добовляем импульс
        jumpReady = true;
    }
}
