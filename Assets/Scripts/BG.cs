using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    float length, startpose, dist, temp;
    public GameObject cam;
    public float parallaxEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        startpose = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        temp = cam.transform.position.x * (1 - parallaxEffect);
        dist = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startpose + dist, transform.position.y, transform.position.z);

        if (temp > startpose + length)
        {
            startpose += length;
        }
        else if (temp < startpose - length)
        {
            startpose -= length;
        }
    }
}
