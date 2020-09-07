using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barnecle : MonoBehaviour
{
    public float speed = 4f; 
    private bool isWait = true;
    private bool isHidden = true;
    public float waitTime = 2f; //время ожидания
    private float distance = 0.8f; //дистанция выползания
    private Vector3 point;
    private Enemy _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = gameObject.GetComponent<Enemy>();
        point = new Vector3(transform.position.x, transform.position.y + distance, transform.position.z);
        _enemy.isAttacking = false;

        // point.transform.position = new Vector3(transform.position.x, transform.position.y + distance,
        //     transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isWait == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, point,
                speed * Time.deltaTime);
        }

        if (transform.position == point)
        {
            if (isHidden)
            {
                point = new Vector3(transform.position.x, transform.position.y - distance,
                    transform.position.z);
                isHidden = false;
            }
            else
            {
                point = new Vector3(transform.position.x, transform.position.y + distance,
                    transform.position.z);
                isHidden = true;
                isWait = true;
                _enemy.isAttacking = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        isWait = false;
        _enemy.isAttacking = true;
    }
}
