using DG.Tweening;
using System;
using System.Collections;
//using System.Security.Principal;
using UnityEngine;
//using EZCameraShake;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if !UNITY_EDITOR
using Luna.Unity;
#endif
namespace SniperHunter
{
    public enum EndCart
    {
        old_ui,
        new_ui
    }
    public class PlayerController : MonoBehaviour //ITakeDamage
    {
        public GameObject Tank1D;
        public GameObject Tank2D;

        public GameObject StartText;
        bool isLunaGamEndCalled = false;
        public GameObject ShootBar;
        #region Player Input Data
        [SerializeField]
        private float health = 10;
        public GameObject shootcam;
        public Camera mainCamera;
        public Camera playerCamera;
        public WeaponUI weaponUI;
        public Animator anim;
        private float scopedInFOV = 20;
        private float scopedInZoomedFOV = 33;
        private float normalFOV = 60;

        private Vector3 lastPos;
        private Vector3 deltaPos;
        private float speed = 2;

        private float rotX = 0;
        private float rotY = 0;

        public float minAimTimeToShoot = 1f;
        [SerializeField]
        private float aimTimeCounter = 0;

        private Transform hit;
        private bool isScopedIn = false;

        private bool cancelFire = false;

        ///private MasterManager manager;
#endregion

        public GameObject playerGameobject;
        public GameObject playerHolder;
        public GameObject bulletPrefab;
        public GameObject muzzleFlash;
        public Transform firePoint;
        public bool nvIsOnSettings = true;
        public GameObject caustic;
        int totalBullet;
        GameObject newbullet;
        ///DeferredNightVisionEffect NVeffect;
        [SerializeField] List<Transform> enemyList = new List<Transform>();

        [SerializeField] Button btnInstallNow;
        [Header("Luna ")]
        [SerializeField]
        [LunaPlaygroundField("EndCart", 7, "CTA UI")]
        public EndCart endcart = EndCart.old_ui;
        public void ShowTank()
        {
            if (PlayerPrefs.GetInt("Tank") == 1)
            {
                Tank1D.SetActive(true);
                Tank1D.transform.Find("" + PlayerPrefs.GetInt("Weapon")).gameObject.SetActive(true);
            }
            if (PlayerPrefs.GetInt("Tank") == 2)
            {
                Tank2D.SetActive(true);
                Tank2D.transform.Find("" + PlayerPrefs.GetInt("Weapon")).gameObject.SetActive(true);
            }
        }
        private void Awake()
        {

        }
        
        IEnumerator CoStart()
        {
            yield return new WaitForSeconds(1.0f);
            
            GameObject.Find("Canvas").transform.Find("HIT").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("Miss").gameObject.SetActive(false);
            shootcam.transform.GetComponent<CameraShake>().enabled = true;
            //ShootBar.SetActive(true);
            //manager = MasterManager.Instance;
            weaponUI.ScopedInaction += ScopedIn;
            weaponUI.cancelFireAction += CancelFire;
            weaponUI.zoomSlider.onValueChanged.AddListener(ZoomIn);
            //if (NVeffect != null)
            //{
            //    NVeffect.enabled = false;
            //}
            //LoadPlayer();
            weaponUI.ScopedInCallBack();
            yield return new WaitForSeconds(1.5f);
            StartText.SetActive(false);
            btnInstallNow.GetComponent<RectTransform>().DOScale(Vector3.one * .8f, .6f).SetLoops(-1, LoopType.Yoyo);
        }
        public void StartAgain()
        {
            mainCamera.GetComponent<Camera>().enabled = true;
            shootcam.transform.GetComponent<CameraShake>().StartMove();
            StartCoroutine(CoStart());
        }

        public void zoomin()
        {
            StartCoroutine(CoStart());
        }
        private void Start()
        {
            ShowTank();
            PlayerPrefs.SetInt("Hit", 0);
            int f = PlayerPrefs.GetInt("Failed");
            
            if (weaponUI.sceneType == SceneType.scene_1&&f==0)
            {
                FailedPanel.SetActive(true);
            }
            anim = GetComponent<Animator>();
            //StartCoroutine(CoStart());
            //manager = MasterManager.Instance;
            //weaponUI.ScopedInaction += ScopedIn;
            //weaponUI.cancelFireAction += CancelFire;
            //weaponUI.zoomSlider.onValueChanged.AddListener(ZoomIn);
            ////if (NVeffect != null)
            ////{
            ////    NVeffect.enabled = false;
            ////}
            ////LoadPlayer();
            //weaponUI.ScopedInCallBack();

            //btnInstallNow.GetComponent<RectTransform>().DOScale(Vector3.one * .8f, .6f).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            weaponUI.ScopedInaction -= ScopedIn;
            weaponUI.cancelFireAction -= CancelFire;
        }

        private void CancelFire()
        {
            cancelFire = true;
            ScopedOut();
            isScopedIn = false;
        }

        private void ScopedIn()
        {
            aimTimeCounter = 0;
            isScopedIn = true;
            cancelFire = false;
            playerGameobject.SetActive(false);
            DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, scopedInFOV, .5f);
            playerCamera.gameObject.SetActive(false);
            //if (NVeffect != null && manager.isNightModeEnable)
            //{
            //    if (nvIsOnSettings)
            //    {
            //        NVeffect.enabled = true;
            //    }
            //}
        }

        public void ScopedOut()
        {
            //if (NVeffect != null)
            //{
            //    if (nvIsOnSettings)
            //    {
            //        NVeffect.enabled = false;

            //    }
            //}
            playerGameobject.SetActive(true);
            DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, normalFOV, .5f).OnComplete(() =>
            {
                zoomedScope = false;
            });
        }

        IEnumerator WaitToScopedOut(float duration, bool isFire = false)
        {
            yield return new WaitForSeconds(duration);
            weaponUI.ScopedOut(isFire);
            playerCamera.gameObject.SetActive(true);
            this.transform.eulerAngles = new Vector3(0, 52.3f, 0);
        }
        public GameObject CTAPanel;

        public void Installbtn()
        {
            CTAPanel.SetActive(true);
            
            if (!isLunaGamEndCalled)
            {
                Luna.Unity.LifeCycle.GameEnded();
                isLunaGamEndCalled = true;
            }
        }
        public void download()
        {
#if !UNITY_EDITOR
                Playable.InstallFullGame();
#endif
        }
        public void Shoot()
        {
            StartText.SetActive(false);
//            if (weaponUI.sceneType == SceneType.scene_1)
//            {
//                CTAPanel.SetActive(true);
//                FailedPanel.SetActive(false);
//                if (!isLunaGamEndCalled)
//                {
//                    Luna.Unity.LifeCycle.GameEnded();
//                    isLunaGamEndCalled = true;
//                }
//#if !UNITY_EDITOR
//                Playable.InstallFullGame();
//#endif
//                //Install now.
//                if (endcart == EndCart.old_ui)
//                {
//                    btnInstallNow.gameObject.SetActive(true);
//                    // Application.OpenURL("https://play.google.com/store/apps/details?id=com.funvai.jseasniper");
//                }
//                else if (endcart == EndCart.new_ui)
//                {
//                    weaponUI.ActivateEndCart();
//                }
//                return;
//            }
            cancelFire = false;
            if (cancelFire == false)
            {
                if (isScopedIn)
                {
                    if (cancelFire == false)
                    {
                        shootcam.transform.GetComponent<CameraShake>().StopMove();
                        Fire();
                    }

                    isScopedIn = false;
                    ScopedOut();
                }
            }
            else
            {
                StartCoroutine(WaitToScopedOut(.01f, false));
            }
            weaponUI.NormalScopedBtn();
        }

        public Transform GetNearest()
        {
            float difChecker = 100000;
            Transform t = null;
            for (int i = 0; i < enemyList.Count; i++)
            {
                float diff = Vector3.Distance(transform.position, enemyList[i].position);
                if (diff < difChecker)
                {
                    difChecker = diff;
                    t = enemyList[i];
                }
            }
            return t;
        }
        public Transform MissTarget;
        public bool miss = false;
        private void Fire()
        {
            //GameManager.Instance.totalbullet--;
            //totalBullet = GameManager.Instance.totalbullet;
            ///manager.panelGame.UpdateBulletQuantity();
            if (miss /*Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 1000)*/)
            {
                PlayerPrefs.SetInt("Failed",0);
                SharkMove shark = GetNearest().GetComponent<SharkMove>(); //hit.transform.GetComponentInParent<SharkMove>();
                hit = MissTarget.transform;
                //hit = shark.transform;
                //hit.position = new Vector3(shark.transform.position.x+200, shark.transform.position.y, shark.transform.position.z);
                this.transform.DOLookAt(hit.position, 0.5f).OnComplete(() =>
                {
                    if (shark)
                    {
                        shark.StopMovement();
                        Sequence sequence = DOTween.Sequence();
                        weaponUI.btnScopeSetactive(false);
                        weaponUI.imgCrossHair.gameObject.SetActive(false);
                        GameObject bullet = bulletPrefab;
                        newbullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
                        mainCamera.GetComponent<Camera>().enabled = false;
                        ///CameraShaker.Instance.ShakeOnce(2.5f, 4, .1f, .5f);
                        SoundManager.SharedManager().PlaySFX(SoundManager.sharedInstance.bulletSounds[0]);
                        newbullet.transform.LookAt(hit.position);
                        
                        GameObject gMuzzleFlash = Instantiate(muzzleFlash, firePoint.position, Quaternion.identity);
                        Destroy(gMuzzleFlash, 5f);
                        Time.timeScale = .5f;
                        sequence.PrependInterval(.1f).Append(newbullet.transform.DOMove(hit.position, 1f).SetEase(Ease.Linear).OnComplete(OnCompleteSlowMotion));
                        StartCoroutine(WaitToScopedOut(.01f, true));
                    }
                });
            }
            else
            {
                PlayerPrefs.SetInt("Failed", 1);
                SharkMove shark = GetNearest().GetComponent<SharkMove>(); //hit.transform.GetComponentInParent<SharkMove>();
                hit = MissTarget.transform;
                //hit = shark.transform;
                //hit.position = new Vector3(shark.transform.position.x+200, shark.transform.position.y, shark.transform.position.z);
                if (shark)
                {
                    //Debug.Log("Shark");
                    //shark.StopMovement();
                    Sequence sequence = DOTween.Sequence();
                    weaponUI.btnScopeSetactive(false);
                    weaponUI.imgCrossHair.gameObject.SetActive(false);
                    GameObject bullet = bulletPrefab;
                    newbullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
                    mainCamera.GetComponent<Camera>().enabled = false;
                    ///CameraShaker.Instance.ShakeOnce(2.5f, 4, .1f, .5f);
                    SoundManager.SharedManager().PlaySFX(SoundManager.sharedInstance.bulletSounds[0]);
                    newbullet.transform.LookAt(hit.position);

                    GameObject gMuzzleFlash = Instantiate(muzzleFlash, firePoint.position, Quaternion.identity);
                    Destroy(gMuzzleFlash, 5f);
                    Time.timeScale = .5f;
                    sequence.PrependInterval(.1f).Append(newbullet.transform.DOMove(hit.position, 1f).SetEase(Ease.Linear).OnComplete(OnCompleteSlowMotion));
                    StartCoroutine(WaitToScopedOut(.01f, true));
                }

                //this.transform.DOLookAt(hit.position, 0.5f).OnComplete(() =>
                //{
                //    if (shark)
                //    {
                //        Debug.Log("Shark");
                //        shark.StopMovement();
                //        Sequence sequence = DOTween.Sequence();
                //        weaponUI.btnScopeSetactive(false);
                //        weaponUI.imgCrossHair.gameObject.SetActive(false);
                //        GameObject bullet = bulletPrefab;
                //        newbullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
                //        mainCamera.GetComponent<Camera>().enabled = false;
                //        ///CameraShaker.Instance.ShakeOnce(2.5f, 4, .1f, .5f);
                //        SoundManager.SharedManager().PlaySFX(SoundManager.sharedInstance.bulletSounds[0]);
                //        newbullet.transform.LookAt(hit.position);
                        
                //        GameObject gMuzzleFlash = Instantiate(muzzleFlash, firePoint.position, Quaternion.identity);
                //        Destroy(gMuzzleFlash, 5f);
                //        Time.timeScale = .7f;
                //        sequence.PrependInterval(.1f).Append(newbullet.transform.DOMove(hit.position, 1f).SetEase(Ease.Linear).OnComplete(OnCompleteSlowMotion));
                //        StartCoroutine(WaitToScopedOut(.01f, true));
                //    }
                //});

                



                /////SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().missShot);
                /////GameObject bullet = manager.prefabsList.GetBulletPrefab(productInfo.weapon);
                //GameObject bullet = bulletPrefab;
                //newbullet = Instantiate(bullet, mainCamera.transform.position, mainCamera.transform.rotation);
                ////prone to some error

                //var bullcontroller = newbullet.GetComponent<BulletController>();
                //bullcontroller.ShotMissed = true;
                //newbullet.transform.DOMove(mainCamera.transform.forward * 50f, .5f);
                //Destroy(newbullet, 2.0f);

                //GameObject gMuzzleFlash = Instantiate(muzzleFlash, firePoint.position, Quaternion.identity);
                //Destroy(gMuzzleFlash, 2f);
                //weaponUI.ScopedOut(false);
                /////MasterManager.Instance.OnPlayerMiss();
                //weaponUI.imgCrossHair.gameObject.SetActive(true);
                ////StartCoroutine(CheckMissionStatus(3));
            }
        }

        public void ZoomIn(float value)
        {
            if (isScopedIn)
            {
                //SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().zoomBar);
                mainCamera.fieldOfView = value;
            }
        }

        //private bool SpecialBulletChecker()
        //{
        //    if (productInfo.weapon == ((int)BulletId.torpedo).ToString() ||
        //        productInfo.weapon == ((int)BulletId.missile).ToString()
        //        )
        //    {
        //        firePoint.gameObject.SetActive(false);
        //        return true;
        //    }
        //    return false;
        //}

        private void OnCompleteSlowMotion()
        {
            //caustic.SetActive(true);
            string headShot = "Head Shot";
            string bodyShot = "Body Shot";
            string strPopUp = "";//hit.collider.CompareTag("PlayerShark") ? headShot : bodyShot;
            ///manager.panelGame.ShowTextFirePopUp(strPopUp);
            if (strPopUp == headShot)
            {
                SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().headShot);
            }

            ///GameObject fireEffect = Instantiate(manager.fxManager.fireEffect, hit.point, Quaternion.identity);
            ///Destroy(fireEffect, 2.0f);
            newbullet.transform.GetChild(0).gameObject.SetActive(false);
            newbullet.transform.GetChild(1).gameObject.SetActive(false);
            Time.timeScale = 1f;
            hit.GetComponent<SharkMove>().Die();
            ///int numOfKillSFX = SoundManager.SharedManager().killSFX.Count;
            ///SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().killSFX[UnityEngine.Random.Range(0, numOfKillSFX)]);
            StartCoroutine(SlowMotionCompleteCallBack());
        }
        private void OnFailedSlowMotion()
        {
            

            ///GameObject fireEffect = Instantiate(manager.fxManager.fireEffect, hit.point, Quaternion.identity);
            ///Destroy(fireEffect, 2.0f);
            newbullet.transform.GetChild(0).gameObject.SetActive(false);
            newbullet.transform.GetChild(1).gameObject.SetActive(false);
            Time.timeScale = 1f;
            
            ///int numOfKillSFX = SoundManager.SharedManager().killSFX.Count;
            ///SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().killSFX[UnityEngine.Random.Range(0, numOfKillSFX)]);
            //StartCoroutine(SlowMotionCompleteCallBack());
            //FailedPanel.SetActive(true);

        }
        public GameObject FailedPanel;
        private IEnumerator SlowMotionCompleteCallBack()
        {
            yield return new WaitForSeconds(1.5f);
            //SceneManager.LoadScene("Game 1");
            yield break;
            Destroy(newbullet);
            mainCamera.GetComponent<Camera>().enabled = true;
            weaponUI.imgCrossHair.gameObject.SetActive(true);
            weaponUI.btnScopeSetactive(true);

            firePoint.gameObject.SetActive(true);
        }

        public void FailedCallBack()
        {
            
            

            //SceneManager.LoadScene("Game 1");
            
            Destroy(newbullet);
            mainCamera.GetComponent<Camera>().enabled = true;
            weaponUI.imgCrossHair.gameObject.SetActive(true);
            weaponUI.btnScopeSetactive(true);

            firePoint.gameObject.SetActive(true);
        }

        //private void MissionFailed()
        //{
        //    manager.panelGame.gameObject.SetActive(false);
        //    manager.levelManager.ResetLevel();
        //    manager.prefabsList.LoadPrefab(PrefabName.LevelFailed, manager.uiManager.transform);
        //}

        private void Update()
        {
            InputHandler();

            //if (manager.gameManager.isGameOver && !manager.gameManager.playVictoryCinematics && !manager.gameManager.isLevelFailed)
            //{
            //    StartCoroutine(VictoryCinematics());
            //}
        }

        private bool zoomedScope;
        private void InputHandler()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPos = Input.mousePosition;

                ///weaponUI.DeactiveScopedBtn();
            }
            else if (Input.GetMouseButton(0))
            {
                aimTimeCounter += Time.deltaTime;
                deltaPos = Input.mousePosition - lastPos;
                //text.text = deltaPos.ToString();
                rotX -= deltaPos.y * speed * Time.deltaTime;
                rotY -= deltaPos.x * speed * Time.deltaTime * -1;

                rotX = Mathf.Clamp(rotX, -25, 15);
                rotY = Mathf.Clamp(rotY, -50, 50);
                //mainCamera.transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rotX, 0, 0), .5f);

                //transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, rotY, 0), .5f);

                //lastPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log("Mouse Btn Up");
                if (cancelFire == false)
                {
                    if (isScopedIn)
                    {
                        if (!zoomedScope && aimTimeCounter >= minAimTimeToShoot)
                        {
                            //Fire
                            Shoot();
                            isScopedIn = false;
                        }
                        else
                        {
                            //Manual Zoom
                            if (false /*TagManager.GetPlayerControllerScope() == 0*/)
                            {
                                if (!zoomedScope)
                                {
                                    zoomedScope = true;
                                    weaponUI.ActivateScopedZoom(true);
                                    DOTween.To(() => mainCamera.fieldOfView, x => mainCamera.fieldOfView = x, scopedInZoomedFOV, .5f);
                                }
                                else
                                {
                                    Debug.Log("Manual Zoom Error");
                                }
                            }
                            //Cancel Fire
                            else
                            {
                                ///CancelFire();
                            }
                        }
                    }
                    else
                    {

                    }
                }
                cancelFire = false;
                ///weaponUI.NormalScopedBtn();
            }
        }

        //        public void TakeDamage(float damageAmount)
        //        {

        //            if (GameManager.Instance.isGameOver == false)
        //            {
        //#if !UNITY_EDITOR && UNITY_ANDROID

        //            Vibration.Vibrate(150);
        //#endif
        //#if !UNITY_EDITOR && UNITY_IPHONE && UNITY_IOS

        //            Vibration.VibratePop();
        //#endif

        //                health -= damageAmount;
        //                manager.panelGame.UpdateHealthBar(health);
        //                if (health <= 0)
        //                {
        //                    GameManager.Instance.isLevelFailed = true;
        //                    GameManager.Instance.isGameOver = true;
        //                    manager.panelGame.alertSound = false;
        //                    if (isScopedIn)
        //                    {
        //                        ScopedOut();
        //                    }
        //                    //weaponUI.WeaponUISetActive(false);
        //                    Material hitMat = GetDeathMat();
        //                    playerGameobject.GetComponentInChildren<SkinnedMeshRenderer>().material = hitMat;
        //                    playerGameobject.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 180), 1f);
        //                    GameObject deathFx = Instantiate(manager.fxManager.deathEffect, transform.position, Quaternion.identity);
        //                    Destroy(deathFx, 1.5f);

        //                    ///int numOfKillSFX = SoundManager.SharedManager().killSFX.Count;
        //                    //SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().killSFX[UnityEngine.Random.Range(0, numOfKillSFX)]);

        //                    playerCamera.gameObject.SetActive(false);
        //                    ///StartCoroutine(LoadLevelfailedPanel());
        //                }

        //                weaponUI.SetDamageMarker(true);
        //                CameraShaker.Instance.ShakeOnce(5.5f, 7.5f, .1f, 1f);
        //                SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().oppnentBullethit);
        //                playerGameobject.transform.transform.DOShakePosition(1.0f);
        //                playerGameobject.transform.transform.DOShakeScale(.2f).OnComplete(() =>
        //                {
        //                    weaponUI.SetDamageMarker(false);
        //                    if (health <= 0)
        //                    {
        //                        weaponUI.WeaponUISetActive(false);
        //                    }
        //                });
        //            }
        //        }

        private static Material GetDeathMat()
        {
            Material hitMat = new Material(Shader.Find("Standard"));
            hitMat.color = Color.red;
            hitMat.EnableKeyword("_EMISSION");
            hitMat.SetColor("_EmissionColor", Color.red);
            return hitMat;
        }

        //public IEnumerator VictoryCinematics()
        //{
        //    QuestData.CheckQuestCompletion((int)QuestType.SniperLevelComplete, 1);

        //    manager.gameManager.playVictoryCinematics = true;

        //    playerHolder.transform.GetChild(0).DOMoveY(playerHolder.transform.position.y + 2, 0.5f);

        //    yield return new WaitForSeconds(1f);
        //    SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().victoryGunfireSFX);


        //    playerHolder.transform.GetChild(0).DORotate(new Vector3(-10, 180, 0), 1f, RotateMode.FastBeyond360).SetLoops(1, LoopType.Restart);
        //    GameObject gunfireVFX = Instantiate(manager.gameManager.gunFireVFX, firePoint.position, firePoint.rotation);
        //    gunfireVFX.transform.SetParent(firePoint);

        //    yield return new WaitForSeconds(1.5f);

        //    //crown fall
        //    GameObject crownfallIns = Instantiate(manager.gameManager.crownFallPrefab, new Vector3(playerHolder.transform.position.x, playerHolder.transform.position.y + 14, playerHolder.transform.position.z), manager.gameManager.crownFallPrefab.transform.rotation);
        //    GameObject headSpinIns = null;
        //    crownfallIns.transform.DOMoveY(playerHolder.transform.GetChild(0).position.y + 2.5f, 1).OnComplete(() =>
        //    {
        //        crownfallIns.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        //        crownfallIns.transform.SetParent(firePoint.parent);
        //        //Destroy(crownfallIns);
        //        gunfireVFX.SetActive(false);
        //        // crown fall off
        //        //crownfallIns.transform.DOMove(new Vector3(playerHolder.transform.position.x + 5, playerHolder.transform.position.y + 5, playerHolder.transform.position.z),0.5f);
        //        //crownfallIns.transform.rotation = Quaternion.Euler(new Vector3(crownfallIns.transform.rotation.x, crownfallIns.transform.rotation.y,-25f));
        //        playerHolder.transform.GetChild(0).DOLocalRotateQuaternion(Quaternion.Euler(-5, 180, 0), 0.3f).SetLoops(2, LoopType.Yoyo);

        //        headSpinIns = Instantiate(manager.gameManager.headSpinFX, new Vector3(playerHolder.transform.position.x, playerHolder.transform.position.y + 5, playerHolder.transform.position.z), manager.gameManager.headSpinFX.transform.rotation);
        //        headSpinIns.transform.SetParent(playerHolder.transform);

        //        //GameObject thugGlassPrefabIns = Instantiate(manager.gameManager.thugGlassPrefab, new Vector3(playerHolder.transform.GetChild(0).position.x, playerHolder.transform.GetChild(0).position.y - 3f, playerHolder.transform.GetChild(0).position.z - 6f), playerHolder.transform.GetChild(0).rotation);
        //        //thugGlassPrefabIns.transform.SetParent(firePoint.parent);
        //        manager.gameManager.won = false;

        //    });

        //    yield return new WaitForSeconds(3.0f);
        //    //manager.gameManager.playVictoryCinematics = false;
        //    Destroy(crownfallIns);
        //    Destroy(gunfireVFX);
        //    Destroy(headSpinIns);
        //    playerHolder.transform.GetChild(0).localRotation = Quaternion.identity;

        //    int currentSniperLevel = AppDelegate.sharedManager().GetCurrentSniperLevel();

        //    FPG.FacebookManager.GetInstance().LogSniperCompleteLevelEvent(currentSniperLevel);
        //    FPG.FirebaseManager.GetInstance().LogAnalyticsEvent($"SniperLevel_{currentSniperLevel}");

        //    AppDelegate.sharedManager().SetCurrentSniperLevel(currentSniperLevel + 1);
        //    Debug.Log("IN...COMPLETE");
        //}
    }
}