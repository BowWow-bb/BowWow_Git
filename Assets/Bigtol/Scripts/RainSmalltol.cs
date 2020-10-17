using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSmalltol : MonoBehaviour
{
    int power;           //공격력

    float x_min;         //생성 x좌표 최소
    float x_max;         //생성 x좌표 최대
    float height;        //생성 높이
    
    float t;             //타이머

    float G;            //중력
    float E;            //탄성 계수
    float now_force;    //현재 공이 받고 있는 힘

    AudioSource Apa;
    AudioSource Tong;
    public AudioClip ApaSound;
    public AudioClip TongSound;


    // Start is called before the first frame update
    void Start()
    {
        power = 20;
        height = Random.Range(43.0f, 60.0f); //생성 높이 랜덤 설정

        //*초기생성위치 = 플레이어 위치 로 설정되있음
        x_min = transform.position.x - 40.0f; //플레이어 +- 거리
        if (x_min < -60.0f) //왼쪽 벽 경계 제한
            x_min = -60.0f;

        x_max = transform.position.x + 40.0f;
        if (x_max > 60.0f) //오른쪽 벽 경계 제한
            x_max = 60.0f;

        float x_rand = Random.Range(x_min, x_max);  //생성지점 결정
        transform.position = new Vector3(x_rand, transform.position.y+height, transform.position.z);

        t = 0;
        G = 0.098f;
        E = 0.9f;
        now_force = 0;

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
        GameObject Player = GameObject.Find("DDaeng");
        Vector3 Pos = transform.position;   //현재 위치

        //계속 내려감
        now_force += G * t;
        Pos.y -= now_force;
        transform.position = Pos;

        //바닥에 닿은 경우 운동방향 바꿔줌
        if (Pos.y < Player.transform.position.y) //바닥과 충돌한 경우
        {
            Tong.Play();
            //Debug.Log("바닥 충돌");
            Pos.y = Player.transform.position.y;
            transform.position = Pos;
            now_force = now_force * E * (-1);
            Destroy(gameObject, 0.3f);
        }

        //플레이어와 충돌한 경우
        //if (dd_flag)
        if(Mathf.Abs(Pos.x-Player.transform.position.x) <6.0f && Mathf.Abs(Pos.y-Player.transform.position.y)<6.0f)
        {
            Apa.Play();
            Tong.Play();
            Move DD = GameObject.Find("DDaeng").GetComponent<Move>();
            DD.TakeDamage(power);//데미지 텍스트 뜨기 위함 
            DD.hpMove(power);

            now_force = now_force * E * (-1);
            Destroy(gameObject, 0.3f);
        }

        t += 0.006f;
    }
}
