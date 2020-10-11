using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_toll : MonoBehaviour
{
    public GameObject fireballPrefab;//파이어볼 프리팹
    public GameObject DDaeng;//땡이
    public GameObject smalltoll;//스몰톨
    Transform st;

    Vector3 target;//땡이 위치
    Vector3 me;//스몰톨 위치 

    public float d = 20f;//범위 거리 설정  
    float movePower = 5f;//움직이는 속력

    float RateMin = 0.5f;//최소 생성 주기 
    float RateMax = 3f;//최대 생성 주기
    float Rate;//파이어볼 생성 주기 

    private float timeAfter;//발사 후 지난 시간
    float timeball;//파이어볼 생성에 필요한 시간 

    int movementFlag = 0;//0: 정지, 1: 왼쪽, 2: 오른쪽
    string dist = "";//이동 방향 

    bool isTracing = false;//거리 내에 들어와서 유지 중인 상태 
    bool Enter = false;//거리 내에 들어오면 (처음)
    bool isStop = false;//멈췄다가 파이어볼 쏘기
    bool isHeart = false;//플레이어에게 공격 받음 여부
    bool isBall = false;//공 한번만 

    public float HPMax = 100.0f;//최대 체력  
    public float HP;//현재 체력

    //h
    GameObject hp_bar;  //hp바
    float hpbar_sx;     //hp바 스케일 x값
    float hpbar_tx;     //hp바 위치 x값
    float hpbar_tmp;    //hp바 감소 정도
    string tag_name;    //hp바 태그
    //

    public int Power_run;//런크래쉬 공격력
    public int Power_fireball;//파이어볼 공격력

    // Start is called before the first frame update
    void Start()
    {
        //h
        HP = HPMax;//체력 설정 
        tag_name = transform.Find("HpBar").transform.Find("Hp").tag;
        hp_bar = GameObject.FindWithTag(tag_name);

        hpbar_sx = GameObject.FindWithTag(tag_name).transform.localScale.x;
        hpbar_tx = GameObject.FindWithTag(tag_name).transform.localPosition.x;
        hpbar_tmp = hpbar_sx / HPMax;
        //

        DDaeng = GameObject.Find("DDaeng");//하이라키 내에서 찾기 위함 

        timeAfter = 0f;//파이어볼 생성 시간 초기화 
        Rate = Random.Range(RateMin, RateMax);//처음 파이어볼 생성 주기 설정

        st = smalltoll.transform.Find("warning");//warning 활성/비활성화 위함
        st.gameObject.SetActive(false);

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(1, 3);//움직임 설정 랜덤
        float movetime = Random.Range(RateMin,RateMax);

        yield return new WaitForSeconds(movetime);//랜덤 초 동안 실행 

        StartCoroutine("ChangeMovement");//다른 움직임 또 하게 호출 
    }

    IEnumerator MoveStop()
    {
        timeball = 0;
        while (true)
        {
            timeball += Time.deltaTime;

            transform.position += Vector3.zero;

            //Debug.Log("timeball: " + timeball);

            if (timeball >= 0.25f && isBall ==false)//0.25초 지나 정지했다가 
            {
                FireballMake();//파이어볼 발사

                timeAfter = 0;
                isStop = false;

                break;
            }
            yield return null;//다시 움직임 시작 
        }  
    }
    IEnumerator FireballDelay()
    {
        yield return new WaitForSeconds(0.1f);
    }

    //스몰톨이 카메라 벗어나지 않게 제한 
    IEnumerator ClipMovementleft()//왼쪽으로 가는 코루틴 실행
    {
        movementFlag = 1;
        //Debug.Log("코루틴 left");

        yield return new WaitForSeconds(2f);

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ClipMovementright()//오른쪽으로 가는 코루틴 실행 
    {
        movementFlag = 2;
        //Debug.Log("코루틴 right");

        yield return new WaitForSeconds(2f);

        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeAfter += Time.deltaTime;//시간 갱신

        Distance();//거리 파악. 트리거 대신 
        Move();//거리 파악 후 움직임, 파이어볼 발사 
    }

    //h
    public void hpMove(float hp_delta)
    {
        float move = ((HPMax-HP) + hp_delta) * hpbar_tmp;

        Vector3 Scale = hp_bar.transform.localScale;
        hp_bar.transform.localScale = new Vector3(hpbar_sx - move, Scale.y, Scale.z);

        Vector3 Pos = hp_bar.transform.localPosition;
        hp_bar.transform.localPosition = new Vector3(hpbar_tx - move / 2.0f, Pos.y, Pos.z);
    }
    //

    void Distance()
    {
        target = DDaeng.transform.position;
        
        float distance = Mathf.Abs(DDaeng.transform.position.x - transform.position.x);//땡이위치 - 스몰톨 위치 절댓(x값만)

        //Debug.Log("땡이랑 거리: " + distance);

        if (distance <= d)//범위 내에 처음 들어오면
        {
            Enter = true;
            //Debug.Log("범위 내에 들어옴");
            StopCoroutine("ChangeMovement");//이동하던 거 멈추고 추적 시작 
        }
        if (Enter == true && distance <= d && distance >8)//들어 온 상태이고 범위 내에 계속 있으면 (닿진 않았고)
        {
            isTracing = true;
        }

        if (isTracing == true && distance > d)//거리 벗어나면 
        {
            Enter = false;
            isTracing = false;
            StartCoroutine("ChangeMovement");//다시 랜덤 이동 시작 
        }

        if(distance<=6)//크러쉬커맨드? 그거 공격범위 내이면 (닿았다면)
        {
            st.gameObject.SetActive(true);
            DDaeng.GetComponent<Move>().TakeDamage(10);//몸빵 공격 

            isTracing = false;
            if (target.x < me.x)
            {
                StartCoroutine("ClipMovementright");
            }

            else if (target.x > me.x)
            {
                StartCoroutine("ClipMovementleft");
            }
            
        }
    }

    void Move()
    {
        me = transform.position;

        Vector3 moveVelocity = Vector3.zero;
        if(isStop ==false)
        {
            if (isTracing || isHeart)//일정 거리 내 이거나 공격 받으면 플레이어 쪽으로 이동  
            {
                st.gameObject.SetActive(true);

                movePower = 12;//추적 시에 속도 빠르게

                if (target.x < me.x)//땡이가 왼쪽이면
                {
                    if(isHeart)
                    {
                        StartCoroutine("ClipMovementleft");//3초동안 왼쪽으로 
                    }

                    if (timeAfter >= Rate)//설정해 둔 파이어볼 생성 주기보다 timeAfter가 크면 
                    {
                        isBall = false;
                        isStop = true;//멈춤 후 공격 
                    }
                    else
                    {
                        dist = "Left";//왼쪽으로 가라 
                    }

                }

                else if (target.x > me.x)//땡이가 오른쪽이면
                {
                    if(isHeart)
                    {
                        StartCoroutine("ClipMovementright");//3초동안 오른쪽으로 
                    }

                    if (timeAfter >= Rate)//설정해 둔 파이어볼 생성 주기보다 timeAfter가 크면 
                    {
                        isStop = true;
                    }
                    else
                    {
                        dist = "Right";
                    }
                }
            }
            else//거리 밖이면 (평소)
            {
                st.gameObject.SetActive(false);

                movePower = 5;

                if (me.x >= 40)
                {
                    StopCoroutine("ChangeMovement");
                    StartCoroutine("ClipMovementleft");
                }
                else if (me.x <= -40)
                {
                    StopCoroutine("ChangeMovement");
                    StartCoroutine("ClipMovementright");
                }

                if (movementFlag == 1)
                    dist = "Left";
                else if (movementFlag == 2)
                    dist = "Right";

            }
            //좌우 이동 
            if (dist == "Left")
            {
                moveVelocity = Vector3.left;
                //transform.localScale = new Vector3(1, 1, 1);
            }
            else if (dist == "Right")
            {
                moveVelocity = Vector3.right;
                //transform.localScale = new Vector3(-1, 1, 1);
            }
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }

        else//정지 상태인 경우 
        {
            StartCoroutine("MoveStop");
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
    }

    void Die()//체력 0일 경우
    {
        
    }

    void FireballMake()
    {

        GameObject ball = GameObject.Instantiate(fireballPrefab); //파이어볼 생성
        isBall = true;

        ball.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 15f);//파이어볼 초기 위치 z:15
        ball.transform.parent = null;

        Rate = Random.Range(RateMin, RateMax);//다음 번 파이어볼 생성 주기 설정 

    }
}