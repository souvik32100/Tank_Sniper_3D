using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

namespace SniperHunter
{
    public enum BulleTtype
    {
        sniper = 101,
        machineGun = 102,
        laser = 103,
        missiile = 104,
        torpedo = 105
    }
    public class BulletController : MonoBehaviour
    {
        public SniperHunter.BulleTtype bulletType;

        public GameObject[] bullet;
        public GameObject _cameraOLD;
        public GameObject _camera2New;
        public GameObject _cameraHolder;
        public GameObject _cartHolder;
       public bool bullettime;
        public float rotationSpeed;
        public float cartSpeed = 8f;
       public float chance = 1.0f; //bullet time chance

        public bool ShotMissed;
        public CinemachineDollyCart mydolly;
        private void Start()
        {
            if (_cartHolder != null)
            {
                _cartHolder.SetActive(false);
            }
            if (_cameraHolder != null)
            {
                _cameraHolder.SetActive(false);
            }

            if (ShotMissed)
            {
                return;
            }
            var i = UnityEngine.Random.value;
            if (i < chance)
            {
                bullettime = true;
            }
            else
            {
                bullettime = false;

            }
            if (bullettime && mydolly!=null)
            {
                mydolly.m_Position = 0;
                mydolly.m_Speed = cartSpeed;
                _cartHolder.SetActive(true);
                // bullet time
                
            }
            
            else
            { // old
                _cameraHolder.SetActive(true);
                mydolly.m_Position = 0;
                mydolly.m_Speed = 0;
                SlowMotionCameraEfffect();
            }
        }

        private void Update()
        {
            if (bullet.Length > 0)
            {
                for (int i = 0; i < bullet.Length; i++)
                {
                    bullet[i].transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
                }
            }
            //transform.Translate(Vector3.forward * 100 * Time.deltaTime);
        }
        private void LateUpdate()
        {
            if (bullettime)
            {
                _camera2New.transform.LookAt(this.transform);
            }
        }
        public void SlowMotionCameraEfffect()
        {
            Sequence sequence = DOTween.Sequence();
           // _cameraOLD.transform.localPosition = new Vector3(0, 0, -20f);
            //sequence.Append(_camera.transform.DOLocalMove(new Vector3(0, 0, -20f), .75f));
            sequence.Append(_cameraOLD.transform.DOLocalMove(new Vector3(0, 0, -10f), .75f));

            // sequence.Append(_camera.transform.DOLocalMove(new Vector3(0, 10, 0), .1f));
            //sequence.Append(_camera.transform.DOLocalMove(new Vector3(0, 0, 0), .1f));
            //sequence.Append(_camera.transform.DOLocalMove(new Vector3(3, 3, -5f), .25f));
            //  sequence.Append(_camera.transform.DOLocalMove(new Vector3(3, -3, -20f), .5f));
            //  sequence.Append(_camera.transform.DOLocalMove(new Vector3(10, -16, -10f), .5f));

            //  sequence.Append(_camera.transform.DOLocalMoveX(5, .25f));
            //sequence.Append(_camera.transform.DOLocalMoveX(-5, .25f));
            //
            //Append(_camera.transform.DOLocalMove(new Vector3(0, 0, -20f), 1.0f));
        }

        public void ShakeCam()
        {
            _cameraHolder.transform.GetChild(0).DOShakePosition(1f, new Vector3(1.5f, 1f, 10), 15, 10f, false, true);

            _camera2New.transform.DOShakePosition(1f, new Vector3(1.5f, 1f, 10), 15, 10f, false, true);

            print("====");
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("" + other.transform.name);
            if(other.transform.name=="Enemy1")
            {
                int h = PlayerPrefs.GetInt("Hit");
                h = h + 1;
                PlayerPrefs.SetInt("Hit", h);

                int x=int.Parse(GameObject.Find("Canvas").transform.Find("B").GetComponent<Text>().text);
               
                GameObject.Find("Canvas").transform.Find("" + x).gameObject.SetActive(false);
                x = x - 1;
                
                GameObject.Find("Canvas").transform.Find("B").GetComponent<Text>().text = "" + x;
                if(h%2==0)
                {
                    GameObject.Find("Canvas").transform.Find("HIT").transform.Find("Text").GetComponent<Text>().text = "BODY SHOT!";
                    GameObject.Find("Canvas").transform.Find("HIT").gameObject.SetActive(true);
                }
                else
                {
                    GameObject.Find("Canvas").transform.Find("HIT").transform.Find("Text").GetComponent<Text>().text = "HEAD SHOT!";
                    GameObject.Find("Canvas").transform.Find("HIT").gameObject.SetActive(true);
                }


                other.transform.GetChild(0).gameObject.SetActive(true);
                //other.transform.parent.parent.Find("Canvas").gameObject.SetActive(false);
                //other.transform.parent.parent.GetComponent<SharkMove>().enabled = true;
                //other.transform.parent.parent.GetComponent<SharkMove>().StopMovement();
                //other.transform.parent.parent.GetComponent<SharkMove>().Die();
                //other.transform.parent.parent.GetComponent<SharkMove>().enabled = false;

                
                transform.Find("Bullet Dolly").Find("DollyCart1").Find("CameraHolder").Find("Camera").GetComponent<Camera>().enabled = false;
                GameObject.Find("Player").transform.GetComponent<PlayerController>().StartAgain();
                StartCoroutine(wait());
                //transform.gameObject.SetActive(false);
                transform.GetComponent<Collider>().enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                if (x == 0&&h<3)
                {
                    StartCoroutine(Wait1());
                    //GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(false);
                }
                if (x == 0 && h == 3)
                {

                    StartCoroutine(Wait2());
                    //GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(false);
                }

            }
            if(other.transform.name == "Loose")
            {
                GameObject.Find("Canvas").transform.Find("Miss").gameObject.SetActive(true);
                transform.Find("Bullet Dolly").Find("DollyCart1").Find("CameraHolder").Find("Camera").GetComponent<Camera>().enabled = false;
                GameObject.Find("Player").transform.GetComponent<PlayerController>().StartAgain();
                transform.GetComponent<Collider>().enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                int x = int.Parse(GameObject.Find("Canvas").transform.Find("B").GetComponent<Text>().text);
                GameObject.Find("Canvas").transform.Find("" + x).gameObject.SetActive(false);
                x = x - 1;
                GameObject.Find("Canvas").transform.Find("B").GetComponent<Text>().text = "" + x;
                if (x == 0)
                {
                    StartCoroutine(Wait3());
                    //GameObject.Find("Canvas").transform.Find("Text").gameObject.SetActive(false);
                }

            }
        }
        IEnumerator wait()
        {
            yield return new WaitForSeconds(1.5f);
            GameObject.Find("Enemy1").transform.GetChild(0).gameObject.SetActive(false);
        }
        IEnumerator Wait1()
        {
            yield return new WaitForSeconds(1.5f);
            GameObject.Find("Enemy1").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("Player").transform.GetComponent<PlayerController>().enabled = false;
            GameObject.Find("Player").transform.GetComponent<PlayerController>().ScopedOut();
            GameObject.Find("Player").transform.Find("WeaponCanvas").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("GameFailed").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("Game").gameObject.SetActive(false);
        }
        IEnumerator Wait2()
        {
            yield return new WaitForSeconds(.5f);
            GameObject.Find("Player").transform.GetComponent<PlayerController>().enabled = false;
            GameObject.Find("Player").transform.GetComponent<PlayerController>().ScopedOut();
            GameObject.Find("Player").transform.Find("WeaponCanvas").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("GameComplete").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("Game").gameObject.SetActive(false);
        }
        IEnumerator Wait3()
        {
            yield return new WaitForSeconds(.5f);
            GameObject.Find("Player").transform.GetComponent<PlayerController>().enabled = false;
            GameObject.Find("Player").transform.GetComponent<PlayerController>().ScopedOut();
            GameObject.Find("Player").transform.Find("WeaponCanvas").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("GameFailed").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("Game").gameObject.SetActive(false);
        }

    }
}
