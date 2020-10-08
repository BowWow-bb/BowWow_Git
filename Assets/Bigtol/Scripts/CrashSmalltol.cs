using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSmalltol : MonoBehaviour
{
    public GameObject player;
    Vector3 player_pos;
    float v=100.0f;   //돌진 속도
    // Start is called before the first frame update
    void Start()
    {
        player_pos = player.transform.position; //미니톨 소환시점의 플레이어 위치
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != player_pos)
        {
            if (player_pos.x < transform.position.x)   //플레이어가 빅톨의 왼쪽에 위치
            {
                if(transform.position.x - Time.deltaTime * v == player.transform.position.x)    //현재 플레이어의 위치가 되는 경우
                {
                    //소환된 미니톨이 돌진하다가 플레이어를 만난 상황
                    //플레이어 hp 손상을 주고 사라짐
                    gameObject.SetActive(false); //미니톨 비활성화

                }
                else
                {
                    transform.position = new Vector3(transform.position.x - Time.deltaTime * v, transform.position.y, transform.position.z);
                } 
            }
            else if (player_pos.x > transform.position.x)  //플레이어가 빅톨의 오른쪽에 위치
            {
                if (transform.position.x + Time.deltaTime * v == player.transform.position.x)    //현재 플레이어의 위치가 되는 경우
                {
                    //소환된 미니톨이 돌진하다가 플레이어를 만난 상황
                    //플레이어 hp 손상을 주고 사라짐
                    gameObject.SetActive(false); //미니톨 비활성화
                    //왜 안만나 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1 수정하기..
                }
                else
                {
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * v, transform.position.y, transform.position.z);
                }
            }   //미니톨 생성할 당시의 플레이어 위치로 돌진..
        }
       
    }
}
