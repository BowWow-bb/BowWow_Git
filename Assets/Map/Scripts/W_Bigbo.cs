using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Bigbo : MonoBehaviour
{
    public bool isThere;   //스킬 습득 유뮤 파악
    Vector3 origin_pos;
    Vector3 Pos;
    Move DD;

    AudioSource BigBo;
    public AudioClip BigBoSound;
    // Start is called before the first frame update
    void Start()
    {
        origin_pos = gameObject.transform.position; //기존 스킬 창 위치 : 화면 밖
        Pos = new Vector3(-42.5f,6.38f,-18.0f); //스킬 습득 시 창 위치 (화면 내에 보이게)
        isThere = false;

        DD = GameObject.FindWithTag("DDaeng").GetComponent<Move>();

        BigBo = gameObject.AddComponent<AudioSource>(); //땡이 오브젝트 가져오기

        BigBo.clip = BigBoSound;
        BigBo.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isThere)    //스킬 습득한 경우
        {
            gameObject.transform.position = Pos;    //스킬 창 활성화(게임화면 내에 창 띄우기)
            DD.BigboActive = true;   //뼈다귀 스킬 활성화
        }
        else
        {
            DD.BigboActive = false;
            gameObject.transform.position = origin_pos;
        }
    }
}
