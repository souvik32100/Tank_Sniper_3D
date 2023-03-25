using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SniperHunter;
using UnityEngine.SceneManagement;
public class SelectorMovement : MonoBehaviour
{
    public GameObject HeadShot;
    public GameObject BodyShot;
    public GameObject MissiedShot;
    public Transform Player;
    public bool moveUp=true;
    public bool moveDown = false;
    bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
        
    }


    // Update is called once per frame
    void Update()
    {
        if(moveUp==true&&moveDown==false&&stop==false)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 15f , transform.localPosition.z);
            if(transform.localPosition.y>=350)
            {
                moveUp = false;
                moveDown = true;
            }
        }
        if (moveUp == false && moveDown == true&&stop==false)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 15f, transform.localPosition.z);
            if (transform.localPosition.y <= 0)
            {
                moveUp = true;
                moveDown = false;
            }
        }
        if(Input.GetMouseButtonDown(0)&&shoot==true)
        {
            stop = true;

            if(transform.localPosition.y>=160)
            {
                Player.GetComponent<PlayerController>().miss = false;
                if(transform.localPosition.y>=280)
                {
                    StartCoroutine(HeadShotCo());
                }
                else
                {
                    StartCoroutine(BodyShotCo());
                }
            }
            if(transform.localPosition.y <= 160)
            {
                Player.GetComponent<PlayerController>().miss = false;
                StartCoroutine(MissiedShotCo());
            }
            Debug.Log(transform.localPosition.y);
        }

    }
    bool shoot = false;
    IEnumerator HeadShotCo()
    {
        yield return new WaitForSeconds(1.75f);
        HeadShot.SetActive(true);
    }
    IEnumerator BodyShotCo()
    {
        yield return new WaitForSeconds(1.75f);
        BodyShot.SetActive(true);
    }

    IEnumerator MissiedShotCo()
    {
        yield return new WaitForSeconds(1.75f);
        MissiedShot.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Game 1");
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        shoot = true;
    }

}
