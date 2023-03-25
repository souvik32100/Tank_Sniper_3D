using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        shake1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StopMove()
    {
        transform.DOKill();
    }
    public void StartMove()
    {
        shake1();
    }
    void shake1()
    {
        transform.DOShakeRotation(2, new Vector3(1f, 1.5f, 0), 1, 30,false).OnComplete(() =>
        {
            shake2();
        });
    }
    void shake2()
    {
        transform.DOShakeRotation(2, new Vector3(1f, 1.5f, 0), 1, 30,false).OnComplete(() =>
        {
            shake1();
        });
    }


}
