using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    scene_0,
    scene_1
};

namespace SniperHunter
{
    public class WeaponUI : MonoBehaviour
    {
        public UnityAction ScopedInaction;
        public UnityAction cancelFireAction;

        public GameObject weaponUI;

        public SceneType sceneType;

        [Header("Weapon UI Components")]
        public GameObject text;
        public Image imgScope;
        public Image imgCrossHair;
        public Image imgBlurBG;
        public Image imgHand;
        public Image damageMarker;
        public Button btnScope;
        public Button btnShoot;
        public Button btnCancelFire;
        public Slider zoomSlider;
        public Image imgInvisible;
        public float zoomSliderValue;
        public Image imgAlertSplash;
        public Image imgAlertSign;

        [Header("Scope Sprite")]
        public Sprite defaultScope;
        public Sprite inactiveScope;

        public Image txtTapToShoot;
        // public TextMeshProUGUI txtTapToZoom;

        [SerializeField] float timer = 0;
        Sequence sequence;
        public bool scopedZoom;

        public bool isOnSlider = false;
        public GameObject btnShootTapToShoot;
        public GameObject btnShootImage;
        public GameObject panelCTA_UI;

        private void Start()
        {
            //btnCancelFire.onClick.AddListener(() => CancelFireCallBack());
            ///txtTapToZoom.text = "Tap To Zoom";
            AnimateBtnScope();

            if (SceneManager.GetActiveScene().buildIndex == 0)
                sceneType = SceneType.scene_0;
            else if(SceneManager.GetActiveScene().buildIndex == 1)
                sceneType = SceneType.scene_1;
        }
        private void Update()
        {
            //Debug.Log(transform.rotation);
            transform.eulerAngles = new Vector3(0,0,0);
        }
        private void AnimateHand()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(imgHand.rectTransform.DOAnchorPosX(-50f, .5f).OnComplete(() =>
            {
                Sequence sequence1 = DOTween.Sequence();
                sequence1.Append(imgHand.rectTransform.DOScale(Vector3.one * .85f, .5f)).
                    Append(imgHand.rectTransform.DOScale(Vector3.one * 1.15f, .25f)).SetLoops(-1).SetDelay(.25f);

                Sequence sequence2 = DOTween.Sequence();
                sequence2.Append(imgHand.rectTransform.DORotate(Vector3.right * 35f, .5f)).
                    Append(imgHand.rectTransform.DORotate(Vector3.zero, .25f)).SetLoops(-1).SetDelay(.25f).OnComplete(() =>
                    {

                        imgHand.rectTransform.DOAnchorPosX(500f, .5f).SetDelay(1.5f);
                    }
                        );
            }));
            sequence.SetLoops(-1);
        }

        internal void SetDamageMarker(bool damaged, float rotation = -115f)
        {
            float[] rots = new float[3] { -115, -25, -250 };
            damageMarker.rectTransform.localRotation = Quaternion.Euler(0, 0, rots[UnityEngine.Random.Range(0, rots.Length)]);
            damageMarker.gameObject.SetActive(damaged);
        }

        public void AnimateBtnScope()
        {
            sequence = DOTween.Sequence();
            sequence.Append(btnScope.GetComponent<Image>().rectTransform.DOScale(new Vector3(.9f, .9f, .9f), .5f)).Append(
                btnScope.GetComponent<Image>().rectTransform.DOScale(Vector2.one, .5f)).SetLoops(-1);
        }

        public void CancelFireCallBack()
        {
            ///SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().btnTap);
            cancelFireAction.Invoke();
            btnScope.gameObject.SetActive(true);
            AnimateBtnScope();
            imgCrossHair.gameObject.SetActive(true);


            isOnSlider = false;
        }
        public void ActivateEndCart()
        {
            btnShootTapToShoot.gameObject.SetActive(false);
            btnShootImage.gameObject.SetActive(false);
            panelCTA_UI.gameObject.SetActive(true);
            imgHand.gameObject.SetActive(false);
        }

        public void ActivateScopedZoom(bool active)
        {
            btnShoot.gameObject.SetActive(active);
            zoomSlider.gameObject.SetActive(active);
            imgInvisible.gameObject.SetActive(active);
            zoomSlider.value = 33f;
        }


        public void WeaponUISetActive(bool task) => weaponUI.SetActive(task);
        public void ScopedInCallBack()
        {
            //txtTapToZoom.gameObject.SetActive(false);
            if (sceneType == SceneType.scene_0)
            {
                imgHand.gameObject.SetActive(false);
            }
            else if (sceneType == SceneType.scene_1)
            {
                imgHand.gameObject.SetActive(true);
            }
            txtTapToShoot.GetComponent<RectTransform>().DOScale(Vector3.one * .85f, .6f).SetLoops(-1, LoopType.Yoyo);

            sequence.Kill();
            timer = 0;
            if(SoundManager.SharedManager()!=null)
            SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().ScopeOn);
            ScopedInaction.Invoke();
            imgScope.gameObject.SetActive(true);
            text.SetActive(true);
            btnShoot.gameObject.SetActive(true);
            //btnShoot.GetComponent<RectTransform>().DOScale(Vector3.one * .85f, .6f).SetLoops(-1, LoopType.Yoyo);

            btnScope.gameObject.SetActive(false);
            imgCrossHair.gameObject.SetActive(false);

            imgBlurBG.gameObject.SetActive(true);
            imgBlurBG.DOColor(new Color(imgBlurBG.color.a, imgBlurBG.color.g, imgBlurBG.color.b, 1), .5f);
            btnCancelFire.GetComponent<RectTransform>().DOAnchorPosX(0, .5f);
        }

        internal void ScopedOut(bool isFire)
        {
            SoundManager.SharedManager().PlaySFX(SoundManager.SharedManager().ScopeOff);
            btnCancelFire.GetComponent<RectTransform>().DOAnchorPosX(-140, .25f);
            btnShoot.gameObject.SetActive(false);
            if (isFire)
                btnScopeSetactive(false);
            else
                btnScopeSetactive(true);
            //btnShoot.GetComponent<RectTransform>().DOAnchorPosX(140, .25f);
            imgBlurBG.DOColor(new Color(imgBlurBG.color.a, imgBlurBG.color.g, imgBlurBG.color.b, 0), .25f).OnComplete(() =>
            {
                imgBlurBG.gameObject.SetActive(false);
            });
            imgScope.gameObject.SetActive(false);
            text.SetActive(false);
            zoomSlider.gameObject.SetActive(false);
        }

        public void DeactiveScopedBtn() => btnScope.image.sprite = inactiveScope;
        public void NormalScopedBtn() => btnScope.image.sprite = defaultScope;

        public void OnPointerUpAnim()
        {
            btnCancelFire.transform.DOScale(Vector3.one * 1.3f, .1f).OnComplete(() =>
            {
                btnCancelFire.transform.DOScale(Vector3.one, .1f);
            });
            if (btnShoot.gameObject.activeInHierarchy)
            {
                btnShoot.transform.DOScale(Vector3.one, .1f);
            }
            CancelFireCallBack();
        }
        public IEnumerator Alert(int loop = 1)
        {
            imgAlertSign.gameObject.SetActive(true);
            imgAlertSplash.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            imgAlertSign.gameObject.SetActive(false);
            imgAlertSplash.gameObject.SetActive(false);
            // DOTween.Sequence().Append(imgAlertSign.DOFade(0, .5f));
            //  DOTween.Sequence().Append(imgAlertSplash.DOFade(0, .5f));//.Append(imgAlertSplash.DOFade(1, .5f)).SetLoops(2);
        }
        public void OnPointerDownSlider() => isOnSlider = true;
        public void OnPointerUpSlider() => isOnSlider = false;

        internal void btnScopeSetactive(bool task) => btnScope.gameObject.SetActive(task);
    }
}