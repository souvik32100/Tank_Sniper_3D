using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
/// <summary>
/// using static UnityEditor.PlayerSettings;
/// </summary>

public class PlayerControlar : MonoBehaviour
{
    public Camera mainCamera;
    private float scopedInFOV = 15; 
    private float normalFOV = 60;
    private bool zoomedScope = false;
    public GameObject Crosshare;
    public GameObject Scope;
    private Vector3 lastPos;
    private Vector3 deltaPos;
    private float rotX = 25.3f;
    private float rotY = 19.5f;
    private float speed = 2;
    private RaycastHit hit;
    public GameObject Bullet;
    public Transform hitPos;
    public GameObject exploson;
    public GameObject CancelFair;
    public GameObject GameWinPanel;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Killed", 0);
    }
    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex==3)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if(PlayerPrefs.GetInt("Killed")==4)
        {
            GameWinPanel.SetActive(true);
        }
        InputHandler();
        //if (zoomedScope==true)
        //{
        //    InputHandler();
        //}
        
    }
    public void zoomIn()
    {
        if(zoomedScope==false)
        {
            CancelFair.SetActive(true);
            shooting = true;
            Scope.SetActive(true);
            Crosshare.SetActive(false);
            DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, scopedInFOV, .5f).OnComplete(() =>
            {
                
                zoomedScope = true;
                shooting = false;
            });
            
        }
        if(zoomedScope=true&&shooting==false)
        {
            shoot();
        }
        
    }
    bool shooting = false;
    void shoot()
    {
        shooting = true;
        GameObject b = Instantiate(Bullet, hitPos.position, mainCamera.transform.rotation);
        //b.transform.GetComponent<BulletControl>().target = target;
        //b.transform.DOMove(mainCamera.transform.forward * 50f, .5f);
        b.transform.DOMove( new Vector3(target.x,target.y,target.z+1), 1).OnComplete(() =>
        {
            Instantiate(exploson, b.transform.position, Quaternion.identity);
            shooting = false;
            Destroy(b);
        });
    }
    public void zoomOut()
    {
        CancelFair.SetActive(false);
        Scope.SetActive(false);
        Crosshare.SetActive(true);
        DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, normalFOV, .5f).OnComplete(() =>
        {
            zoomedScope = false;
        });
    }
    float y = 0;
    Vector3 target;
    public Transform weapon;
    private void InputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
            
        }
        else if(Input.GetMouseButton(0))
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 1000))
            {
                target = hit.point;
                
            }
            deltaPos = Input.mousePosition - lastPos;

            rotX -= deltaPos.y * speed * Time.deltaTime;
            rotY -= deltaPos.x * speed * Time.deltaTime * -1;
            if(deltaPos.x>0)
            {
                
                y += deltaPos.x * Time.deltaTime*2f;
                y = Mathf.Clamp(y, -20, 10);
                weapon.localRotation = Quaternion.Euler(0,y,0);
            }
            if (deltaPos.x < 0)
            {
                
                y -= deltaPos.x * Time.deltaTime * 2f*-1;
                y = Mathf.Clamp(y, -20, 10);
                
                weapon.localRotation = Quaternion.Euler(0, y, 0);
            }
           // Debug.Log(rotX +" "+ rotY);
            rotX = Mathf.Clamp(rotX, 5, 32);
            rotY = Mathf.Clamp(rotY, 0, 30);
            
            mainCamera.transform.localRotation = Quaternion.Euler(rotX, rotY, 0);

            //mainCamera.transform.localRotation = Quaternion.Euler(0, rotY, 0);

            lastPos = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            //y = 0;
        }
    }
}
