using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


//고칠꺼 겁나 많음!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1

public class BigFireball : MonoBehaviour
{
    public GameObject Minifireball_Perfab;     //미니파이어볼 가져오기

    float t=0;                    //시간
    float move=20;                //일정 이동거리
    float move_tmp=0;             //현재 이동 거리(일정 이동거리 도달 여부)
    float move_v=0.03f;           //이동 속도
    float rot_v = 5000.0f;        //회전 속도

    float height;                 //높이 조절
    

    int mini_n;         //미니 파이어볼 개수
    int mini_cnt;       //미니 파이어볼 현재 생성개수
    bool mini_flag;     //미니 파이어볼 생성 여부 true: 생성완료, false: 생성전
             
    Vector3 PlayerPos;  //플레이어 위치

    // Start is called before the first frame update
    void Start()
    {
        mini_n = 8;
        mini_cnt = 0;
        mini_flag = false;

        height = transform.position.y;  //빅 파이어볼 초기 y좌표

        GameObject player = GameObject.Find("DDaeng");
        PlayerPos = player.transform.position;  //파이어볼 생성 당시의 플레이어 위치
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        if (move_tmp < move)  //일정거리 이동 못한 경우
        {
            if(PlayerPos.x < transform.position.x)  //플레이어가 빅톨의 왼쪽에 위치
            {
                //계속 내려감
                
                //바닥에 닿은 경우 운동방향 바꿔줌


                //float fpower = ball_power - gv;
                //height += fpower;
                //gv += G;

                //if (height< 4.0f) //바닥에 닿은 경우
                //{
                    
                //    height = 4.0f;
                //    gv = 0;
                //    ball_power = -fpower * E;
                //}
                //transform.position = new Vector3(transform.position.x - t * move_v, height, transform.position.z);
                //transform.Rotate(0, 0, -t * rot_v);   //반시계 방향으로 속도만큼 회전

                //move_tmp += t * move_v;
                //t += 0.1f;
            }
            else if (PlayerPos.x > transform.position.x) //플레이어가 빅톨의 오른쪽에 위치
            {
                
            }
            //위치가 같은경우는..?

            //if (Mathf.Abs(transform.position.x) > 48)   //벽 경계를 넘어서는 경우 폭팔
            //    move_tmp = move;
        }
        else if(!mini_flag) //이동 완료
        {
            Destroy(gameObject, 0.08f);  //**초뒤 빅파이어볼 비활성화
            for(int i=0; i<mini_n; i++)
            {
                GameObject minifireball = GameObject.Instantiate(Minifireball_Perfab); //미니 파이어볼 n개 생성
                minifireball.transform.position = transform.position;   //미니 파이어볼 초기 위치 = 빅 파이어볼 현재 위치
            }
            mini_flag = true;
        }
    }

}
