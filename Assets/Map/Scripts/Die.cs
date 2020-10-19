using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void onStart()
    {
        SceneManager.LoadScene("Stage1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
