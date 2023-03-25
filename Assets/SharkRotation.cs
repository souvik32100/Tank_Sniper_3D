using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SharkRotation : MonoBehaviour
{
    
    public GameObject CTAPanel;
    public GameObject GameFailed;
    public GameObject GameComplete;
    public Transform[] Sharks;
    bool isLunaGamEndCalled = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Rotation());
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Sharks.Length; i++)
        {
            if (Sharks[i].localPosition.z >= 30)
            {
                
                Sharks[i].localPosition = new Vector3(Sharks[i].localPosition.x, Sharks[i].localPosition.y, -67);
            }
            

        }
    }
   
    IEnumerator Rotation()
    {
        yield return new WaitForSeconds(5f);
        for (int i=0;i<Sharks.Length;i++)
        {
            Sharks[i].DOLocalMoveZ(Sharks[i].localPosition.z + 20f, 3);

        }
        
        StartCoroutine(Rotation());
    }

    public void CTAOpen()
    {
        GameFailed.SetActive(false);
        GameComplete.SetActive(false);
        if (!isLunaGamEndCalled)
        {
            Luna.Unity.LifeCycle.GameEnded();
            isLunaGamEndCalled = true;
        }
        CTAPanel.SetActive(true);
    }
}
