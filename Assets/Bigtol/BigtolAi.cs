using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigtolAi : MonoBehaviour
{
    public GameObject player;
    public GameObject bigfire;
    public GameObject mini_crash;
    public GameObject mini_summon;

    float hp;               //HP
    //static float hp_std;    //HP 손상 기준

    float dis_std;          //일정 이동거리
    float dis_tmp;          //이동 거리 확인 (일정 이동거리 도달 여부)

    Vector3 dir;            //이동 벡터
    int dir_ran;            //랜덤 이동방향 0:왼쪽, 1:오른쪽

    bool minitol_crash;     //크래쉬 커맨드 미니톨 생성여부
    bool minitol_summon;    //서먼테크 미니톨 생성여부

    // Start is called before the first frame update
    void Start()
    {
        Init(); //초기 설정값 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) //임시로 hp 감소효과 주기
            hp -= 0.1f;

        if (hp == 100.0f) //손상 없는 경우
        {
            //좌우 랜덤으로 일정거리 이동
            if(dis_tmp==0.0f)   //이동 중일 경우 방향 전환 불가
                dir_ran = Random.Range(0, 2);   //0 또는 1을 랜덤으로 반환

            if (dir_ran == 0)   //왼쪽 이동
            {
                if (transform.position.x - dis_std * Time.deltaTime < -43.0f)   //왼쪽 벽 경계 이동제한
                    dis_tmp= 0.0f;
                else
                {
                    transform.position = new Vector3(transform.position.x - dis_std * Time.deltaTime, transform.position.y, transform.position.z);
                    dis_tmp += dis_std * Time.deltaTime;
                }
            }
            else //오른쪽 이동
            {
                if (transform.position.x + dis_std * Time.deltaTime > 43.0f)    //오른쪽 벽 경계 이동제한
                    dis_tmp = 0.0f;
                else
                {
                    transform.position = new Vector3(transform.position.x + dis_std * Time.deltaTime, transform.position.y, transform.position.z);
                    dis_tmp += dis_std * Time.deltaTime;
                }
            }
 
            if (dis_tmp > dis_std) //일정거리 이동 후 초기화
                dis_tmp = 0.0f;
        }

        else if (hp < 100.0f) //손상된 경우 - 플레이어 향해 이동
        {
            if (player.transform.position.x < transform.position.x && transform.position.x - dis_std * Time.deltaTime > -43.0f)   //플레이어가 빅톨의 왼쪽에 위치, 왼쪽 벽 경계 이동제한
            {
                transform.position = new Vector3(transform.position.x - dis_std * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else if(player.transform.position.x > transform.position.x && transform.position.x + dis_std * Time.deltaTime < 43.0f)  //플레이어가 빅톨의 오른쪽에 위치, 오른쪽 벽 경계 이동제한
            {
                transform.position = new Vector3(transform.position.x + dis_std * Time.deltaTime, transform.position.y, transform.position.z);
            }
        }

        //if(hp<빅파이어볼 생성 기준) -> 빅파이어볼 생성 (조건문 나중에 만들자..)
        if (Input.GetKeyDown(KeyCode.Z)) //확인위해 임시 조건문.. 나중에 삭제
        {
            GameObject fireBball = GameObject.Instantiate(bigfire); //파이어볼 생성

            //빅파이어볼 초기 위치 = 빅톨 현재 위치 (파이어볼의 크기 고려해 위로 이동)
            fireBball.transform.position = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
        }

        //if(hp<크래쉬 커맨드 생성 기준) -> 미니톨 소환 후, 돌진 (조건문 나중에 만들자..)
        if (Input.GetKeyDown(KeyCode.X) && !minitol_crash)
        {
            for(int i=0; i<5; i++)
            {
                GameObject crash_tol = GameObject.Instantiate(mini_crash); //미니톨 생성
                crash_tol.transform.position = transform.position;   //미니톨 초기 위치 = 빅톨 현재 위치
            }
            minitol_crash = true;
        }

        //if(hp<서먼 테크 생성 기준) -> 미니톨 소환 후(조건문 나중에 만들자..)
        if (Input.GetKeyDown(KeyCode.C) && !minitol_summon)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject summon_tol = GameObject.Instantiate(mini_summon); //미니톨 생성
                summon_tol.transform.position = transform.position;   //미니톨 초기 위치 = 빅톨 현재 위치
            }
            minitol_summon = true;
        }
        //크래쉬 커맨드, 서먼 테크 실행주기 생각해보기.. (현재 한번 실행 후 flag로 인해 끝남)

    }

    void Init()
    {
        hp = 100.0f;
        //hp_std = 55.0f;
        dis_std = 7.0f;
        dis_tmp = 0.0f;
        minitol_crash = false;
        minitol_summon = false;
    }
}

//후처리
//1. Instantiate, Destroy 너무 반복하면 시스템에 부담 -> 미리넉넉히만들어 놓고 필요시 활성화 하기..(나중에 생각)
//2/ 조건문 길이 줄이기
