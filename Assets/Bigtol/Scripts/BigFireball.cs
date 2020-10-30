using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BigFireball : MonoBehaviour
{
    public GameObject Minifireball_Perfab;     //미니파이어볼 가져오기

    int power;      //공격력

    float t;                    //시간
    float move;                //일정 이동거리
    float move_tmp;             //현재 이동 거리(일정 이동거리 도달 여부)
    float move_v;           //좌우 이동 속도

    float height;                 //높이 조절
    

    int mini_n;         //미니 파이어볼 개수
    bool mini_flag;     //미니 파이어볼 생성 여부 true: 생성완료, false: 생성전

    float G; //중력
    float E; //탄성 계수
    float now_force;    //현재 공이 받고 있는 힘

             
    Vector3 PlayerPos;  //플레이어 초기 위치
    Vector3 BallPos;    //파이어볼 초기 위치

    AudioSource Bomb;
    AudioSource Apa;
    AudioSource Tong;
    public AudioClip BombSound;
    public AudioClip ApaSound;
    public AudioClip TongSound;
    // Start is called before the first frame update
    void Start()
    {
        power = 20;

        t = 0;                    
        move = 35;                 
        move_tmp = 0;             
        move_v = 0.8f;           

        height = transform.position.y;  //빅 파이어볼 초기 y좌표

        mini_n = 10;
        mini_flag = false;

        G = 0.098f; 
        E = 0.9f;
        now_force = 0;

        GameObject Player = GameObject.Find("DDaeng");
        PlayerPos = Player.transform.position;  //파이어볼 생성 당시의 플레이어 위치
        BallPos = transform.position;   //파이어볼 생성 초기 위치

        Bomb = gameObject.AddComponent<AudioSource>();
        Bomb.clip = BombSound;
        Bomb.loop = false;
        Apa = gameObject.AddComponent<AudioSource>();
        Apa.clip = ApaSound;
        Apa.loop = false;
        Tong = gameObject.AddComponent<AudioSource>();
        Tong.clip = TongSound;
        Tong.loop = false;

    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        Vector3 Pos = transform.position;   //현재 위치

        if (move_tmp < move)  //일정거리 이동 못한 경우
        {
            if(PlayerPos.x < BallPos.x)  //플레이어가 빅톨의 왼쪽에 위치
            {
                //계속 내려감
                now_force += G * t;
                Pos.y -= now_force;
                transform.position = new Vector3(Pos.x - t * move_v, Pos.y, Pos.z);

                move_tmp += t * move_v;

                //바닥에 닿은 경우 운동방향 바꿔줌
                if (transform.position.y < 3.3) //바닥과 충돌한 경우
                {
                    Tong.Play();
                    Pos.y = 3.3f;
                    transform.position = Pos;
                    now_force = now_force * E * (-1);
                }
            }
            else if (PlayerPos.x > BallPos.x) //플레이어가 빅톨의 오른쪽에 위치
            {
                //계속 내려감
                now_force += G * t;
                Pos.y -= now_force;
                transform.position = new Vector3(Pos.x + t * move_v, Pos.y, Pos.z);

                move_tmp += t * move_v;

                //바닥에 닿은 경우 운동방향 바꿔줌
                if (transform.position.y < 3.3) //바닥과 충돌한 경우
                {
                    Tong.Play();
                    Pos.y = 3.3f;
                    transform.position = Pos;
                    now_force = now_force * E * (-1);
                }
            }

            if (Mathf.Abs(transform.position.x) > 60.0f)   //양쪽 벽 경계를 넘어서는 경우 폭팔
                move_tmp = move;
        }
        else if(!mini_flag) //이동 완료
        {
            Destroy(gameObject, 0.000001f);  //**초뒤 빅파이어볼 비활성화
            Minifire(); //미니 파이어볼 생성
            mini_flag = true;
        }

        t += 0.02f;
    }
    void Minifire()
    {
        Bomb.Play();
        for (int i = 0; i < mini_n; i++)
        {
            GameObject minifireball = GameObject.Instantiate(Minifireball_Perfab); //미니 파이어볼 생성
            minifireball.transform.position = transform.position;   //미니 파이어볼 초기 위치 = 빅 파이어볼 현재 위치
            minifireball.transform.parent = null;    //독립된 개체
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //tag 에러 방지! -> 스크립트로 인식?
        {
            Apa.Play();
            Minifire();
            Destroy(gameObject,0.5f);    //파이어볼 사라짐

            Move DD = GameObject.Find("DDaeng").GetComponent<Move>();

            DD.TakeDamage(power);//데미지 텍스트 뜨기 위함 
            DD.hpMove(power);
        }
    }
}
