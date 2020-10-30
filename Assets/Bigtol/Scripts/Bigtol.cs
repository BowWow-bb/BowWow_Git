using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bigtol : MonoBehaviour
{
    public GameObject Bigfireball_Perfab;
    public GameObject Raintol_Perfab;
    public GameObject Summon_Perfab;
    public GameObject DamageText;

    public Transform head;  //데미지 텍스트 뜨는 위치 

    public int HP;          //HP
    int HPMax;              //최대 체력
    GameObject hp_bar;      //hp바
    float hpbar_sx;         //hp바 스케일 x값
    float hpbar_tx;         //hp바 위치 x값
    float hpbar_tmp;        //hp바 감소 정도

    float move;             //일정 이동거리
    float move_tmp;         //현재 이동 거리(일정 이동거리 도달 여부)
    float move_v;           //이동 속도 조절

    Vector3 dir;            //이동 벡터
    int move_Rdir;          //랜덤 이동방향 0:왼쪽, 1:오른쪽
    int move_Tdir;           //추적 이동방향 0:왼쪽, 1:오른쪽

    int raintol_n;          //레인 미니톨 생성 개수
    int summon_n;           //서먼 미니톨 생성 개수
    int tolHptag_n;         //서먼 미니톨 hp바 태그 관리

    float timer;            //타이머

    // Start is called before the first frame update
    void Start()
    {
        HPMax = 400;
        HP = HPMax;
        hp_bar = GameObject.FindWithTag("BigtolHp");
        hpbar_sx = hp_bar.transform.localScale.x;
        hpbar_tx = hp_bar.transform.localPosition.x;
        hpbar_tmp = hpbar_sx / HPMax;   //최대 체력에 따른 hp바 이동량 설정

        move = 10.0f;
        move_v = 0.02f;

        raintol_n = 4;
        summon_n = 3;
        tolHptag_n = 1;

        timer = 0;

        StartCoroutine("MoveRandom");
        StartCoroutine("MoveTrace");
    }

    IEnumerator MoveRandom()    //랜덤 이동
    { 
        move_Rdir = Random.Range(0, 2);  //랜덤 방향 설정: 0 or 1

        if (move_Rdir == 0)
            StartCoroutine("MoveRandom_Left");  //왼쪽 이동
        else //move_Tdir == 1
            StartCoroutine("MoveRandom_Right"); //오른쪽 이동

        yield return new WaitForSeconds(2f);
    }
    IEnumerator MoveRandom_Left()
    {
        move_Rdir = 0;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("MoveRandom");       
    }
    IEnumerator MoveRandom_Right()
    {
        move_Rdir = 1;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("MoveRandom");       //방향 재설정

    }

    IEnumerator MoveTrace() //추적 이동
    {
        GameObject Player = GameObject.Find("DDaeng");  //현재 플레이어의 위치 받기

        if (Player.transform.position.x < transform.position.x)
            StartCoroutine("MoveTrace_Left");  //왼쪽 이동
        else if (Player.transform.position.x > transform.position.x)
            StartCoroutine("MoveTrace_Right"); //오른쪽 이동
        else
            StartCoroutine("MoveRandom");

        yield return new WaitForSeconds(2f);
    }
    IEnumerator MoveTrace_Left()
    {
        move_Tdir = 0;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("MoveTrace");
    }
    IEnumerator MoveTrace_Right()
    {
        move_Tdir = 1;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("MoveTrace");  

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject Player = GameObject.Find("DDaeng");  //현재 플레이어의 위치 받기

        //hp손상 + 일정반경 내에 플레이어가 있는 경우 - 플레이어 향해 이동
        if (HP < HPMax && Mathf.Abs(transform.position.x - Player.transform.position.x) < 35)
        {
            if (move_Tdir == 0)  //왼쪽 이동
            {
                if (transform.position.x - move * move_v < -56.0f)  //왼쪽 벽 경계 이동제한
                {
                    StopCoroutine("MoveTrace_Left");   //왼쪽 이동 중지
                    StopCoroutine("MoveTrace");
                    StartCoroutine("MoveTrace");       //방향 재설정
                }
                else
                    transform.position = new Vector3(transform.position.x - move * move_v, transform.position.y, transform.position.z);
            }
            else if (move_Tdir == 1)   //오른쪽 이동
            {
                if (transform.position.x + move * move_v > 56.0f)   //오른쪽 벽 경계 이동제한
                {
                    StopCoroutine("MoveTrace_Right");  //오른쪽 이동 중지
                    StopCoroutine("MoveTrace");
                    StartCoroutine("MoveTrace");   //방향 재설정
                }
                else
                    transform.position = new Vector3(transform.position.x + move * move_v, transform.position.y, transform.position.z);
            }
        }

        else
        {   //hp손상 X or 일정반경 내에 플레이어가 없는 경우 - 좌우 랜덤 이동
            if (move_Rdir == 0)  //왼쪽 이동
            {
                if (transform.position.x - move * move_v < -56.0f)  //왼쪽 벽 경계 이동제한
                {
                    StopCoroutine("MoveRandom_Left");   //왼쪽 이동 중지
                    StopCoroutine("MoveRandom");
                    StartCoroutine("MoveRandom");       //방향 재설정
                }
                else
                    transform.position = new Vector3(transform.position.x - move * move_v, transform.position.y, transform.position.z);
            }
            else if(move_Rdir == 1)  //오른쪽 이동
            {
                if (transform.position.x + move * move_v > 56.0f)   //오른쪽 벽 경계 이동제한
                {
                    StopCoroutine("MoveRandom_Right");  //오른쪽 이동 중지
                    StopCoroutine("MoveRandom");
                    StartCoroutine("MoveRandom");   //방향 재설정
                }
                else
                    transform.position = new Vector3(transform.position.x + move * move_v, transform.position.y, transform.position.z);
            }
        }

        //1. 파이어볼 스킬
        if (Mathf.Abs(Player.transform.position.x - transform.position.x) < 25  && timer%200 ==0)   //플레이어가 일정거리 내 접근한 경우
            Bigfire();

        //2.레인 커맨드 스킬
        if (HP < HPMax - 100 && timer%300 == 0) //hp에 따른 자동 적용 스킬
            Rain(Player);
            

        //3. 서먼테크 스킬
        if (HP < HPMax - 150 && timer % 400 == 0) //hp에 따른 자동 적용 스킬
            Summon();

        timer += 1;
        if (timer >= 400)
            timer = 0;
        Debug.Log(timer);
    }

    public void hpMove(int hp_delta)    //hp바 동작 구현
    {
        if (HP-hp_delta <= 0)
            Destroy(gameObject);

        float move = ((HPMax - HP) + hp_delta) * hpbar_tmp;
        HP -= hp_delta;

        Vector3 Scale = hp_bar.transform.localScale;
        hp_bar.transform.localScale = new Vector3(hpbar_sx - move, Scale.y, Scale.z);

        Vector3 Pos = hp_bar.transform.localPosition;
        hp_bar.transform.localPosition = new Vector3(hpbar_tx - move/2.0f, Pos.y, Pos.z);
    }
    void Bigfire()  //빅 파이어볼 스킬
    {
        GameObject bigfireball = GameObject.Instantiate(Bigfireball_Perfab); //빅파이어볼 생성

        //빅파이어볼 초기 위치 = 빅톨 현재 위치 (파이어볼의 크기 고려해 위로 이동)
        bigfireball.transform.position = new Vector3(transform.position.x, transform.position.y + 6.0f, transform.position.z);
        bigfireball.transform.parent = null;    //독립된 개체
    }
    void Rain(GameObject Player) //레인 커맨드 스킬
    {
        for (int i = 0; i < raintol_n; i++)
        {
            GameObject rain_tol = GameObject.Instantiate(Raintol_Perfab); //미니톨 생성
            rain_tol.transform.position = Player.gameObject.transform.position; //미니톨 초기 위치 = 플레이어 현재 위치   
            rain_tol.transform.parent = null;    //독립된 개체
        }
    }
    void Summon()   //서먼 테크 스킬
    {
        for (int i = 0; i < summon_n; i++)
        {
            if (tolHptag_n > 9) //제한된 태그 갯수 초과
                tolHptag_n = 1;

            GameObject summon_tol = GameObject.Instantiate(Summon_Perfab); //미니톨 생성
            summon_tol.transform.position = transform.position;   //미니톨 초기 위치 = 빅톨 현재 위치 
            summon_tol.transform.parent = null;    //독립된 개체

            string tag_name = ("tolHp" + tolHptag_n++).ToString();
            summon_tol.transform.Find("HpBar").transform.Find("Hp").tag = tag_name;
        }
    }
    public void TakeDamage(int damage)//땡이한테 맞기위함 
    {
        GameObject damageText = Instantiate(DamageText);
        damageText.transform.position = head.position;
        damageText.GetComponent<DamageText>().damage = damage;
    }
}
