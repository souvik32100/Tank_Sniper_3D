using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SniperHunter;
using UnityEngine.SceneManagement;

public enum Scene2SharkStates
{

    Scene2AttackPlayer,
    Scene2ScopeMove

}
public class SharkMove : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public float time;
    //  [HideInInspector]
    public Vector3 direction;
    // [HideInInspector]

    public Vector3 TarpgetPos;
    //  [HideInInspector]

    public Vector3 initialPos;
    //   [HideInInspector]
    [Range(0, 5)]
    public float RotateSpeed = 1f;
    public Vector3 initialRot;
    public float moveUnit = 50;
    public float Yval;  
    bool Alive;
    bool Scene2AttackPlayer = true;

    [SerializeField]
    [LunaPlaygroundField("Scene2SharkStates", 1, "GamePlay")]
    Scene2SharkStates scene2SharkState;
    private void Start()
    {
        Alive = true;
       // Yval = Random.Range(-8f, 8f);
        direction = transform.forward;
        initialPos = transform.position;
        TarpgetPos = transform.position + ((direction * moveUnit) + new Vector3(0, 1, 0) * Yval) ;
        initialRot = transform.eulerAngles;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(MoveShark());

        }
        else
        {

            if (scene2SharkState == Scene2SharkStates.Scene2AttackPlayer)
            {
                Scene2AttackPlayer = true;
                player.GetComponent<Animator>().enabled = false;

                StartCoroutine(InitAttackPlayerVariant());
            }
            else if (scene2SharkState == Scene2SharkStates.Scene2ScopeMove)
            {
                Scene2AttackPlayer = false;
                StartCoroutine(MoveWithScope());
                // player.GetComponent<PlayerController>()?.ScopeMove(this.transform);


            }
        }
    }

    public void StopMovement()
    {
        StopAllCoroutines();
        transform.DOKill();
        //  transform.doraw
    }

    IEnumerator MoveShark()
    {
        while (Alive)
        {
            transform.eulerAngles = initialRot;
            transform.DOMove(TarpgetPos, time);
            yield return new WaitForSeconds(time);
            transform.DORotate(new Vector3(0, -initialRot.y, 0), .5f);
            transform.DOMove(initialPos, time);
            yield return new WaitForSeconds(time);

            yield return null;
        }

        yield return null;
    }

    public void Die()
    {
        SetDeathMaterial();
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 180), .3f).OnComplete(() => { gameObject.AddComponent<Rigidbody>(); });
        //GameObject deathFx = Instantiate(manager.fxManager.deathEffect, transform.position, Quaternion.identity);
        SoundManager.sharedInstance.PlaySFX(SoundManager.sharedInstance.killSFX[0]);
        StartCoroutine(WaitForDie());
    }
    IEnumerator WaitForDie()
    {
        yield return new WaitForSeconds(2f);
        transform.gameObject.SetActive(false);
    }
    private void SetDeathMaterial()
    {
        Material hitMat = new Material(Shader.Find("Standard"));
        hitMat.color = Color.red;
        hitMat.EnableKeyword("_EMISSION");
        hitMat.SetColor("_EmissionColor", Color.red);
        transform.GetComponentInChildren<SkinnedMeshRenderer>().material = hitMat;
    }
    private IEnumerator InitAttackPlayerVariant()
    {
        yield return new WaitForSeconds(.5f);

        while (true)
        {
            transform.DOLookAt(player.position + Vector3.right * 5, .5f);
            transform.DOMove(player.position + Vector3.forward * 20, 1.4f);
            yield return new WaitForSeconds(1.4f);
            StartCoroutine(player.GetComponent<PlayerController>().weaponUI.Alert(loop: 1));
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(.2f);

            transform.DOMove((initialPos), 1.4f);
            yield return new WaitForSeconds(1.4f);

        }
    }
    private IEnumerator MoveWithScope()
    {
        float speed = 100;

        while (true)
        {
            transform.position += Vector3.right * RotateSpeed * (speed) * Time.deltaTime;

            yield return new WaitForSeconds(.1f);
            player.transform.position += Vector3.right * RotateSpeed * (speed - .5f) * Time.deltaTime;

        }
        yield return null;

    }
}

