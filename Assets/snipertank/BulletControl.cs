﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float speed;
    public Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        //transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.MoveTowards(transform.position,target, speed * Time.deltaTime) ;
    }
}
