using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1 : MonoBehaviour
{
    // Start is called before the first frame update
    Move DD;
    void Start()
    {
        DD = GameObject.Find("DDaeng").GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(DD.HP <= 0)//땡이가 죽은 경우
            SceneManager.LoadScene("Die");

        GameObject st1_tol = GameObject.FindWithTag("Smalltol_stage1");
        if(st1_tol.transform == null)
            SceneManager.LoadScene("Stage2");

    }
}
