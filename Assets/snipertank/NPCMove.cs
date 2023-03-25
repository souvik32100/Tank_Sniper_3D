using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCMove : MonoBehaviour
{
    public Transform Waypoints;
    public bool move;
    public float speed;
    public Transform tankBody;
    int killed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    int i = 0;
    
    // Update is called once per frame
    void Update()
    {
        
        if (move)
        {
            transform.DOLookAt(Waypoints.GetChild(i).position, .5f).OnComplete(()=>
            {
                transform.position = Vector3.MoveTowards(transform.position, Waypoints.GetChild(i).position, speed * Time.deltaTime);
                if (transform.position == Waypoints.GetChild(i).position)
                {
                    i++;
                    if (i == Waypoints.childCount)
                    {
                        i = 0;
                    }
                }
            });
            
            


        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            //Destroy(other.gameObject);
            move = false;
            killed = PlayerPrefs.GetInt("Killed");
            killed++;
            PlayerPrefs.SetInt("Killed", killed);
            //transform.GetComponent<Rigidbody>().useGravity = true;
            for (int i=0;i<tankBody.childCount;i++)
            {
                tankBody.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                tankBody.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
                Destroy(transform.gameObject, 1f);
            }
        }
    }

}
