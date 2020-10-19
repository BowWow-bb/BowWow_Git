using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageBoss : MonoBehaviour
{
    Move DD;
    void Start()
    {
        GameObject Title = GameObject.FindWithTag("title");
        Destroy(Title, 3.0f);

        DD = GameObject.Find("DDaeng").GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DD.HP <= 0)//땡이가 죽은 경우
            SceneManager.LoadScene("Die");

        GameObject boss = GameObject.FindWithTag("Bigtol");
        if (!boss)  //boss 존재하지 않는 경우
            SceneManager.LoadScene("Clear");
    }
}
