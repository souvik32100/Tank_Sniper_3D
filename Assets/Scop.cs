using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scop : MonoBehaviour
{
    public Transform ScopeImg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Enemy1")
        {
            Debug.Log("Aimed");
            ScopeImg.GetComponent<Image>().color = new Color(256, 0, 0, 256);
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.name=="Enemy1")
        {
            Debug.Log("Aimed");
            ScopeImg.GetComponent<Image>().color = new Color(0, 256, 0, 256);
        }
    }
    
}
