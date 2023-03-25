using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Desert()
    {
        SceneManager.LoadScene(1);
    }
    public void WinterF()
    {
        SceneManager.LoadScene(2);
    }
    public void WinterV()
    {
        SceneManager.LoadScene(3);
    }

}
