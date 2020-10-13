using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigtol : MonoBehaviour
{
    public GameObject Bigfireball_Perfab;
    public GameObject Raintol_Perfab;
    public GameObject Summon_Perfab;
    public GameObject DamageText;

    public Transform head;//데미지 텍스트 뜨는 위치 

    public float HP;               //HP
    float HPMax = 400.0f;   //최대 체력
    GameObject hp_bar;  //hp바
    float hpbar_sx;         //hp바 스케일 x값
    float hpbar_tx;         //hp바 위치 x값
    float hpbar_tmp;        //hp바 감소 정도

    float move;             //일정 이동거리
    float move_tmp;         //현재 이동 거리(일정 이동거리 도달 여부)
    float move_v;           //이동 속도

    Vector3 dir;            //이동 벡터
    int move_dir;           //랜덤 이동방향 0:왼쪽, 1:오른쪽

    int raintol_n;          //레인 미니톨 생성 개수
    int summon_n;           //서먼 미니톨 생성 개수

    // Start is called before the first frame update
    void Start()
    {
        HP= HPMax;
        hp_bar = GameObject.FindWithTag("BigtolHp");
        hpbar_sx = hp_bar.transform.localScale.x;
        hpbar_tx = hp_bar.transform.localPosition.x;
        hpbar_tmp = hpbar_sx / HPMax;

        move = 7.0f;
        move_tmp = 0;
        move_v = 0.8f;

        raintol_n = 5;
        summon_n = 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject Player = GameObject.Find("DDaeng");
            
        //손상없거나 + 일정반경 내에 플레이어가 없는 경우 - 좌우 랜덤 이동
        if (move_tmp == 0.0f)   //랜덤 방향 이동 완료된 경우
            move_dir = Random.Range(0, 2);  //랜덤 방향 설정: 0 or 1

        if (move_dir == 0)  //왼쪽 이동
        {
            if (transform.position.x - move * Time.deltaTime * move_v < -56.5f)  //왼쪽 벽 경계 이동제한
                move_tmp = 0.0f;
            else
            {
                transform.position = new Vector3(transform.position.x - move * Time.deltaTime * move_v, transform.position.y, transform.position.z);
                move_tmp += move * Time.deltaTime * move_v;  //현재 이동거리 업데이트
            }
        }
        else  //오른쪽 이동
        {
            if (transform.position.x + move * Time.deltaTime * move_v > 56.5f)   //오른쪽 벽 경계 이동제한
                move_tmp = 0.0f;
            else
            {
                transform.position = new Vector3(transform.position.x + move * Time.deltaTime * move_v, transform.position.y, transform.position.z);
                move_tmp += move * Time.deltaTime * move_v;  //현재 이동거리 업데이트
            }
        }
        
        if (HP < 100.0f && Mathf.Abs(transform.position.x-Player.transform.position.x) < 20.0f) //손상된 경우 + 일정반경 내에 있는 경우 - 플레이어 향해 이동
        {
            if (Player.transform.position.x < transform.position.x && transform.position.x - move * Time.deltaTime * move_v > -43.0f)   //플레이어가 빅톨의 왼쪽에 위치, 왼쪽 벽 경계 이동제한
            {
                transform.position = new Vector3(transform.position.x - move * Time.deltaTime * move_v, transform.position.y, transform.position.z);
                move_tmp += move * Time.deltaTime * move_v;  //현재 이동거리 업데이트
            }
            else if(Player.transform.position.x > transform.position.x && transform.position.x + move * Time.deltaTime * move_v < 43.0f)  //플레이어가 빅톨의 오른쪽에 위치, 오른쪽 벽 경계 이동제한
            {
                transform.position = new Vector3(transform.position.x + move * Time.deltaTime * move_v, transform.position.y, transform.position.z);
                move_tmp += move * Time.deltaTime * move_v;  //현재 이동거리 업데이트
            }
        }

        if (move_tmp > move)    //일정거리 이동 완료한 경우
            move_tmp = 0.0f;    //현재 이동거리 초기화

        //1. 파이어볼 스킬
        //if (hp < 빅파이어볼 생성 기준) -> 빅파이어볼 생성(조건문 나중에 만들자..)
        if (Input.GetKeyDown(KeyCode.Z)) //확인위해 임시 조건문.. 나중에 삭제
        {
            GameObject bigfireball = GameObject.Instantiate(Bigfireball_Perfab); //빅파이어볼 생성

            //빅파이어볼 초기 위치 = 빅톨 현재 위치 (파이어볼의 크기 고려해 위로 이동)
            bigfireball.transform.position = new Vector3(transform.position.x, transform.position.y + 6.0f, transform.position.z);
            bigfireball.transform.parent = null;    //독립된 개체
        }


        //2.레인 커맨드 스킬
        //if(hp<레인 커맨드 생성 기준) -> 껍데기 미니톨 소환 후 비처럼 떨어져~ (조건문 나중에 만들자..)
        if (Input.GetKeyDown(KeyCode.X))
        {
            for(int i=0; i<raintol_n; i++)
            {
                GameObject rain_tol = GameObject.Instantiate(Raintol_Perfab); //미니톨 생성
                rain_tol.transform.position = Player.gameObject.transform.position; //미니톨 초기 위치 = 플레이어 현재 위치   
                rain_tol.transform.parent = null;    //독립된 개체
            }
        }

        //3. 서먼테크 스킬
        //if(hp<서먼 테크 생성 기준) -> 미니톨 소환 후(조건문 나중에 만들자..)
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < summon_n; i++)
            {
                GameObject summon_tol = GameObject.Instantiate(Summon_Perfab); //미니톨 생성
                summon_tol.transform.position = transform.position;   //미니톨 초기 위치 = 빅톨 현재 위치 
                summon_tol.transform.parent = null;    //독립된 개체

                string tag_name = ("SummontolHp"+i).ToString();
                summon_tol.transform.Find("HpBar").transform.Find("Hp").tag= tag_name;
                //Debug.Log(summon_tol.transform.Find("HpBar").transform.Find("Hp").tag);

                StartCoroutine(SummonDelay());//미니톨 생성시간 달리하기
            }
        }
    }

    public void hpMove(float hp_delta) 
    {
        HP -= hp_delta;
        float move = ((HPMax - HP) + hp_delta) * hpbar_tmp;

        Vector3 Scale = hp_bar.transform.localScale;
        hp_bar.transform.localScale = new Vector3(hpbar_sx - move, Scale.y, Scale.z);

        Vector3 Pos = hp_bar.transform.localPosition;
        hp_bar.transform.localPosition = new Vector3(hpbar_tx - move/2.0f, Pos.y, Pos.z);

        //hp 원상태 =100
        //hp -1 => - hp바 길이(=scale.x)/100
        //            => 스케일 조정한 길이의 1/2만큼 위치이동
        //            => hp바 위치(=Position.x) - -hp바 길이(= scale.x) / 100 /2
    }

    IEnumerator SummonDelay()
    {
        yield return new WaitForSeconds(0.1f);  //미니톨 생성후 일정시간 대기
    }

    public void TakeDamage(int damage)//땡이한테 맞기위함 
    {
        GameObject damageText = Instantiate(DamageText);
        damageText.transform.position = head.position;
        damageText.GetComponent<DamageText>().damage = damage;
    }
}

//후처리
//1. Instantiate, Destroy 너무 반복하면 시스템에 부담 -> 미리넉넉히만들어 놓고 필요시 활성화 하기..(나중에 생각)
//2/ 조건문 길이 줄이기
