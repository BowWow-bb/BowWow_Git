using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2 : MonoBehaviour
{
    Move DD;
    void Start()
    {
        DD = GameObject.Find("DDaeng").GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DD.HP <= 0)//땡이가 죽은 경우
            SceneManager.LoadScene("Die");

        GameObject st2_tol = GameObject.FindWithTag("smalltall");
        if (st2_tol.transform == null)
            SceneManager.LoadScene("StageBoss");
    }
}
