using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class AirPatrol : MonoBehaviour
{
    public Transform point1; //начальная и конечная точка пути
    public Transform point2;
    public float speed = 1f;
    private Transform target;
    private SpriteRenderer sr;
    private float waitTime = 2f; //время ожидания перед повторным движением
    private bool canGo = true; //способность мухи двигаться
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        target = point1;
        //gameObject.transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (canGo)
        {
            transform.position = Vector3.MoveTowards(transform.position, point1.position,
                speed * Time.deltaTime); //движение между точками
        }

        if (gameObject.transform.position == point1.position)
        {
            target = point1;
            point1 = point2;
            point2 = target;

            canGo = false;
            StartCoroutine(Waiting());

            if (sr.flipX)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        canGo = true;
    }
}
