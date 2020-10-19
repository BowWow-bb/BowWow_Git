using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Bigbo : MonoBehaviour
{
    public bool isThere;   //존재 유뮤 파악

    Vector3 Pos;
    Move DD;

    AudioSource BigBo;
    public AudioClip BigBoSound;
    // Start is called before the first frame update
    void Start()
    {
        Pos = new Vector3(-42.5f,6.38f,-18.0f);
        isThere = false;

        DD = GameObject.FindWithTag("DDaeng").GetComponent<Move>();

        BigBo = gameObject.AddComponent<AudioSource>();
        BigBo.clip = BigBoSound;
        BigBo.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isThere)    //스킬 습득한 경우
        {
            gameObject.transform.position = Pos;    //스킬 창 활성화
            if (Input.GetKeyDown(KeyCode.W))
            {
                BigBo.Play();
                DD.BigboActive = true;
                isThere = false;
            }
        }
    }
}
