using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemBone : MonoBehaviour
{
    // Start is called before the first frame update
    float t;    //타이머
    Q_Bone q_Bone;

    AudioSource Item_pick;
    AudioSource Item_drop;

    public AudioClip Item_drop_Sound;
    public AudioClip Item_Sound;

    bool isDDaeng = false;  //땡이와 충돌 유무 판단

    void Start()
    {
        q_Bone = GameObject.Find("Q_Bone").GetComponent<Q_Bone>();  //뼈다귀 스킬 슬롯 오브젝트 찾기

        t = 0;
        Item_pick = gameObject.AddComponent<AudioSource>();
        Item_pick.clip = Item_Sound;
        Item_pick.volume = 0.6f;
        Item_pick.loop = false;

        Item_drop = gameObject.AddComponent<AudioSource>();
        Item_drop.clip = Item_drop_Sound;
        Item_drop.volume = 0.6f;
        Item_drop.loop = false;

        Item_drop.Play();   //아이템 생성될때 효과음 재생(스몰톨 죽는 소리)
    }

    // Update is called once per frame
    void Update()
    {
        //일정시간 경과 후 아이템 소멸
        if (t >= 100.0f)
            Destroy(gameObject);

        if(isDDaeng&&Input.GetKeyDown(KeyCode.Z))   //땡이와 충돌하면서 Z키를 누른 경우
        {
            getBone();
            Item_pick.Play();
            Debug.Log("땡이가 먹음");
            Destroy(gameObject, 0.2f);
        }

        t += 0.1f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //땡이와 충돌한 경우
        {
            isDDaeng = true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //땡이와 충돌한 경우
        {
            isDDaeng = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //땡이와 충돌한 경우
        {
            isDDaeng = false;
        }
    }

    void getBone()  //스킬 습득 여부 판단
    {
        if(!q_Bone.isThere) //스킬 미습득한 상태
            q_Bone.isThere = true;  //스킬 습득
    }
}
