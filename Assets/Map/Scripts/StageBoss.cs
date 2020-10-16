using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageBoss : MonoBehaviour
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

        GameObject boss = GameObject.FindWithTag("Bigtol");
        if (!boss)
            SceneManager.LoadScene("Clear");
    }
}
