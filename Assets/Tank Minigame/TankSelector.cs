using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class TankSelector : MonoBehaviour
{
    public GameObject Tank1;
    public GameObject Tank2;
    public GameObject Weapon1;
    public GameObject Weapon2;

    public GameObject TankPanel;
    public GameObject WeaponPanel;
    public GameObject BattlePanel;

    public GameObject Tank1D;
    public GameObject Tank2D;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Tank", 0);
        PlayerPrefs.SetInt("Weapon", 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TankBtn(int t)
    {
        Tank1.transform.position = new Vector3(-2.5f, Tank1.transform.position.y, Tank1.transform.position.y);
        Tank2.transform.position = new Vector3(-2.5f, Tank2.transform.position.y, Tank2.transform.position.y);
        Tank1.SetActive(false);
        Tank2.SetActive(false);
        Weapon1.SetActive(true);
        Weapon2.SetActive(true);
        TankPanel.SetActive(false);
        WeaponPanel.SetActive(true);
        PlayerPrefs.SetInt("Tank", t);
    }

    public void WeaponBtn(int w)
    {
        Weapon1.transform.position = new Vector3(2.5f, .4f, Weapon1.transform.position.y);
        Weapon2.transform.position = new Vector3(2.5f, .4f, Weapon2.transform.position.y);
        Weapon1.SetActive(false);
        Weapon2.SetActive(false);
        WeaponPanel.SetActive(false);
        PlayerPrefs.SetInt("Weapon", w);
        //ShowTank();
        BattlePanel.SetActive(true);
        StartCoroutine(showingTank());
    }

    public void ShowTank()
    {
        if(PlayerPrefs.GetInt("Tank")==1)
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

    public void Battle()
    {
        SceneManager.LoadScene(1);
    }

    public IEnumerator showingTank()
    {
        if (PlayerPrefs.GetInt("Tank") == 1)
        {
            Tank1.SetActive(true);
            Tank1.transform.DOMoveX(.35f, 2f);
        }
        if (PlayerPrefs.GetInt("Tank") == 2)
        {
            Tank2.SetActive(true);
            Tank2.transform.DOMoveX(.35f, 2f);
        }
        if (PlayerPrefs.GetInt("Weapon") == 1)
        {
            Weapon1.SetActive(true);
            Weapon1.transform.DOMoveX(.35f, 2f);
        }
        if (PlayerPrefs.GetInt("Weapon") == 2)
        {
            Weapon2.SetActive(true);
            Weapon2.transform.DOMoveX(.35f, 2f);
        }


        yield return new WaitForSeconds(1.85f);
        Tank1.SetActive(false);
        Tank2.SetActive(false);
        Weapon1.SetActive(false);
        Weapon2.SetActive(false);
        ShowTank();
    }
}
