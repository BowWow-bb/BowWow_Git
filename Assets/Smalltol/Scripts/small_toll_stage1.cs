using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_toll_stage1 : MonoBehaviour
{
    //스몰톨 스테이지1

    public GameObject DDaeng;//땡이
    public GameObject DamageText;
    public GameObject smalltoll;//스몰톨
    
    public Transform head;//데미지 텍스트 뜨는 위치
    Transform st;

    Vector3 target;//땡이 위치
    Vector3 me;//스몰톨 위치 

    public float d = 20f;//범위 거리 설정
    float movePower = 5f;//움직이는 속력
    float RateMin = 0.5f;//최소 생성 주기 
    float RateMax = 3f;//최대 생성 주기
    float Rate;//파이어볼 생성 주기
    float Ypos;
    float timeAfter;//발사 후 지난 시간

    int movementFlag = 0;//0: 정지, 1: 왼쪽, 2: 오른쪽
    string dist = "";//이동 방향 

    bool isTracing = false;//거리 내에 들어와서 유지 중인 상태 
    bool Enter = false;//거리 내에 들어오면 (처음)
    bool isHeart = false;
    bool isStop = false;
    bool isAttack = false;//공격 여부
    bool isAttack_once = false;//근접 공격 한번만 적용
    bool isY = false;//y값 비교, 추적, 공격 여부
    bool isTouch = false;
    bool isWall = false;//벽 파악 

    public int HP;              //HP
    int HPMax;                  //최대 체력
    public int Power_run;       //런크래쉬 공격력

    //h
    GameObject hp_bar;  //hp바
    float hpbar_sx;     //hp바 스케일 x값
    float hpbar_tx;     //hp바 위치 x값
    float hpbar_tmp;    //hp바 감소 정도
    string tag_name;    //hp바 태그
    //

   

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("위치: " + gameObject.transform.position); //world 좌표임

        //h
        HPMax = 200;
        HP = HPMax;
        tag_name = transform.Find("HpBar").transform.Find("Hp").tag;
        hp_bar = GameObject.FindWithTag(tag_name);
        //Debug.Log(transform.Find("HpBar").transform.Find("Hp").tag);

        hpbar_sx = hp_bar.transform.localScale.x;
        hpbar_tx = hp_bar.transform.localPosition.x;
        hpbar_tmp = hpbar_sx / HPMax;
        //

        DDaeng = GameObject.Find("DDaeng");

        timeAfter = 0f;
        Rate = Random.Range(RateMin, RateMax);

        st = smalltoll.transform.Find("warning");
        st.gameObject.SetActive(false);

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(1, 3);//움직임 설정 랜덤 

        yield return new WaitForSeconds(3f);//3초동안 실행 

        StartCoroutine("ChangeMovement");//다른 움직임 또 하게 호출 
    }

    IEnumerator MoveStop()
    {
        yield return new WaitForSeconds(0.25f);

        timeAfter = 0;
        isStop = false;
    }
    //스몰톨이 카메라 벗어나지 않게 제한 
    IEnumerator ClipMovementleft()//왼쪽으로 가는 코루틴 실행
    {
        movementFlag = 1;
        //Debug.Log("코루틴 left");

        yield return new WaitForSeconds(3f);

        if (!isAttack_once)
        {
            StartCoroutine("ChangeMovement");
        }
    }

    IEnumerator ClipMovementright()//오른쪽으로 가는 코루틴 실행 
    {
        movementFlag = 2;
        //Debug.Log("코루틴 right");

        yield return new WaitForSeconds(3f);

        if (!isAttack_once)
        {
            StartCoroutine("ChangeMovement");
        }
    }

    IEnumerator Heart()
    {
        st.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);//2초 동안 땡이 방향으로 빠르게 이동 

        isHeart = false;//isHeart 플래그 끄기
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        timeAfter += Time.deltaTime;//시간 갱신
        Distance();
        Move();
    }

    //h
    public void hpMove(int hp_delta)
    {
        if (HP <= 0)
            Destroy(gameObject);

        HP -= hp_delta;
        float move = ((HPMax - HP) + hp_delta) * hpbar_tmp;

        if (HP <= 0)
            Destroy(gameObject);

        Vector3 Scale = hp_bar.transform.localScale;
        hp_bar.transform.localScale = new Vector3(hpbar_sx - move, Scale.y, Scale.z);

        Vector3 Pos = hp_bar.transform.localPosition;
        hp_bar.transform.localPosition = new Vector3(hpbar_tx - move / 2.0f, Pos.y, Pos.z);
    }
    //

    void Distance()//거리 파악. 트리거 대신 
    {
        target = DDaeng.transform.position;
        me = transform.position;

        float distance = Vector3.Distance(target, transform.position);//거리 구하는 함수
        Ypos = Mathf.Abs(target.y - transform.position.y);//절댓값(땡이의 y값 - 스몰 톨의 y값)

        if (Ypos <= 5)
        {
            isY = true;
        }
        else
        {
            isY = false;
        }

        if (distance <= 6.5f)
        {
            isTouch = true;
        }
        else
        {
            isTouch = false;
        }

        if (distance <= d && isY && !isWall)//범위 내에 처음 들어오면. 벽이랑 접해있지 않을 때 
        {
            st.gameObject.SetActive(true);
            Enter = true;
            StopCoroutine("ChangeMovement");//이동하던 거 멈추고 추적 시작
        }

        if (Enter == true && distance <= d && distance > 9 && isY &&!isWall)//들어 온 상태이고 범위 내에 계속 있으면 
        {
            isTracing = true;//추격 중 
            isAttack = false;
        }

        if(distance <=11 && isY&&! isWall)//빠르게 움직
        {
            isAttack = true;
        }

        if (isTouch && isY && !isWall)//공격 범위 내이면 
        {
            isAttack = false;//공격 후
            isTracing = false;

            Move dd = GameObject.Find("DDaeng").GetComponent<Move>();//땡이 스크립트 가져오기

            //데미지 텍스트 설정 
            if (target.x > me.x)//땡이가 오른쪽이면 
            {
                dd.head.position = DDaeng.GetComponent<Move>().headleft.position;
            }
            else
            {
                dd.head.position = DDaeng.GetComponent<Move>().headright.position;//기본 head
            }

            if (target.x <= me.x)//땡이가 왼쪽이면 
            {
                StartCoroutine("ClipMovementright");
            }

            else if (target.x > me.x)//땡이가 오른쪽이면 
            {
                StartCoroutine("ClipMovementleft");
            }

            if (isAttack_once)//한 번 만 공격 
            {
                dd.TakeDamage(5);//텍스트 데미지 
                dd.hpMove(5);
            }
            isAttack_once = false;
        }
        if(!isTracing&&!isY&&isAttack)//근접 공격중 땡이가 계단 올라가면  
        {
            Debug.Log("distance: " + distance);
            StopAllCoroutines();
            StartCoroutine("ChangeMovement");
            isAttack = false;
            Debug.Log("isY: " + isY);
            Debug.Log("isAttack: " + isAttack);
            Debug.Log("isTracing: " + isTracing);
        }
        if (isTracing == true && distance > d && isY)//거리 벗어나면 
        {
            st.gameObject.SetActive(false);
            Enter = false;
            isTracing = false;
            isY = false;
            StartCoroutine("ChangeMovement");
        }
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (isStop == false)
        {
            //일정 거리 내이면 추적. 벽이랑 접해있지 않을 때 
            if ((isTracing && isY && !isWall && !isTouch) || (!isAttack && !isTracing && isY && !isWall && isHeart && !isTouch))
            {
                if (timeAfter >= Rate && !isHeart)
                {
                    isStop = true;//멈춤 후 공격 
                }

                //추격 중에 Y값 조건 체크 
                if (Ypos <= 5)
                {
                    isY = true;
                }
                else
                {
                    isY = false;
                }

                if (isAttack)
                {
                    if (timeAfter >= Rate)
                    {
                        isStop = true;//멈춤 후 공격 
                    }

                    movePower = 50;
                    isAttack = false;
                }
                else
                {
                    movePower = 12;//추적 시에 속도 빠르게
                }

                if (target.x < me.x)//땡이가 왼쪽이면
                {
                    dist = "Left";
                }

                else if (target.x > me.x)//땡이가 오른쪽이면
                {
                    dist = "Right";
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
            }
            else if (dist == "Right")
            {
                moveVelocity = Vector3.right;
            }
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        else if(isStop == true)//정지 상태인 경우 
        {
            movePower = 0;
            StartCoroutine("MoveStop");
        }
    }

    public void TakeDamage(int damage)//땡이한테 맞기위함 
    {
        GameObject damageText = Instantiate(DamageText);
        damageText.transform.position = head.position;
        damageText.GetComponent<DamageText>().damage = damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "miniwall")
        {
            isWall = true;
            if (other.gameObject.transform.position.x <= transform.position.x)//벽이 왼쪽이면 
            {
                StartCoroutine("ClipMovementright");
            }
            else if (other.gameObject.transform.position.x > transform.position.x)
            {
                StartCoroutine("ClipMovementleft");
            }
        }

        if (other.gameObject.tag == "DDaeng")
        {
            //Debug.Log(other.gameObject.transform.position.x - transform.position.x);
            isAttack_once = true;
            isTouch = true;
        }

        if (other.gameObject.tag == "SoundWave")
        {
            if (!isTouch && isY && !isWall)//땡이랑 닿지 않았을 때 
            {
                StopCoroutine("ChangeMovement");
                isHeart = true;
                StartCoroutine("Heart");
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "miniwall")
        {
            isWall = true;
            if (other.gameObject.transform.position.x <= transform.position.x)//벽이 왼쪽이면 
            {
                StopCoroutine("ChangeMovement");
                StartCoroutine("ClipMovementright");
            }
            else if (other.gameObject.transform.position.x > transform.position.x)//벽이 오른쪽이면 
            {
                StopCoroutine("ChangeMovement");
                StartCoroutine("ClipMovementleft");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "miniwall")
        {
            //Debug.Log("벽 트리거 끝");
            isWall = false;//벽이 없음 
        }
        if (other.gameObject.tag == "DDaeng")
        {
            isAttack_once = false;
        }
    }
}