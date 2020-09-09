using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrol : MonoBehaviour
{
    public float speed = 0.1f;
    private bool moveLeft = true;
    private RaycastHit2D groundInfo;
    public Transform groundDetect; //объкт, из которого кидается луч
    private float distanceRay = 1f; //длина кидаемого луча
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime); //передвижение
        groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, distanceRay); //проверка обрыва

        if (groundInfo.collider == false)
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
    }
}
