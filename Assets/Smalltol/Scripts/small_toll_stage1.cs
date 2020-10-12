using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class small_toll_stage1 : MonoBehaviour
{
    public GameObject DDaeng;//땡이
    public GameObject DamageText;

    public Transform head;//데미지 텍스트 뜨는 위치 

    Vector3 target;//땡이 위치
    Vector3 me;//스몰톨 위치 

    public float d = 20f;//범위 거리 설정
    private float time;
    float movePower = 5f;//움직이는 속력
    float RateMin = 0.5f;//최소 생성 주기 
    float RateMax = 3f;//최대 생성 주기
    float Rate;//파이어볼 생성 주기
    float Ypos;

    private float timeAfter;//발사 후 지난 시간

    int movementFlag = 0;//0: 정지, 1: 왼쪽, 2: 오른쪽
    string dist = "";//이동 방향 

    bool isTracing = false;//거리 내에 들어와서 유지 중인 상태 
    bool Enter = false;//거리 내에 들어오면 (처음)
    bool isHeart = false;
    bool isStop = false;
    bool isAttack = false;//공격 여부
    bool isY = false;//y값 비교, 추적, 공격 여부 

    public int HPMax;//최대 체력
    public int HP;//현재 체력
    public int Power_run;//런크래쉬 공격력

    //h
    GameObject hp_bar;  //hp바
    float hpbar_sx;     //hp바 스케일 x값
    float hpbar_tx;     //hp바 위치 x값
    float hpbar_tmp;    //hp바 감소 정도
    string tag_name;    //hp바 태그
    //

    public GameObject smalltoll;//스몰톨
    Transform st;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("위치: " + gameObject.transform.position);world 좌표임

        /*//h
        HP = HPMax;//체력 설정 
        tag_name = transform.Find("HpBar").transform.Find("Hp").tag;
        hp_bar = GameObject.FindWithTag(tag_name);

        hpbar_sx = GameObject.FindWithTag(tag_name).transform.localScale.x;
        hpbar_tx = GameObject.FindWithTag(tag_name).transform.localPosition.x;
        hpbar_tmp = hpbar_sx / HPMax;
        //*/

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
        time = 0;
        while (true)
        {
            time += Time.deltaTime;
            transform.position += Vector3.zero;

            //Debug.Log("timeball: " + timeball);

            if (time >= 0.25f)//0.25초 지나 정지했다가 
            {
                timeAfter = 0;
                isStop = false;

                break;
            }
            yield return null;//다시 움직임 시작 
        }

    }
    //스몰톨이 카메라 벗어나지 않게 제한 
    IEnumerator ClipMovementleft()//왼쪽으로 가는 코루틴 실행
    {
        movementFlag = 1;
        //Debug.Log("코루틴 left");

        yield return new WaitForSeconds(1f);

        StartCoroutine("ChangeMovement");
    }

    IEnumerator ClipMovementright()//오른쪽으로 가는 코루틴 실행 
    {
        movementFlag = 2;
        //Debug.Log("코루틴 right");

        yield return new WaitForSeconds(1f);

        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        timeAfter += Time.deltaTime;//시간 갱신
        Distance();
        Move();
    }

    //h
    public void hpMove(float hp_delta)
    {
        float move = ((HPMax - HP) + hp_delta) * hpbar_tmp;

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

        //Debug.Log("땡이랑 거리: " + distance);

        if (distance <= d && isY)//범위 내에 처음 들어오면
        {
            st.gameObject.SetActive(true);
            Enter = true;
            StopCoroutine("ChangeMovement");//이동하던 거 멈추고 추적 시작 
        }

        if (Enter == true && distance <= d && distance > 9.5 && isY)//들어 온 상태이고 범위 내에 계속 있으면 
        {
            isTracing = true;//추격 중 
            isAttack = false;
        }

        if (isTracing == true && distance > d && isY)//거리 벗어나면 
        {
            st.gameObject.SetActive(false);
            Enter = false;
            isTracing = false;
            isY = false;
            StartCoroutine("ChangeMovement");
        }

        if( distance <=10 && isY)//빠르게 움직
        {
            isAttack = true;
        }

        if (distance <= 6 && isY)//공격 범위 내이면 
        {
            isAttack = false;//공격 후
            isTracing = false;

            DDaeng.GetComponent<Move>().TakeDamage(5);//공격

            Move dd = GameObject.Find("DDaeng").GetComponent<Move>();
            dd.hpMove(5.0f);

            if (dd.HP <= 0)
            {
                Destroy(DDaeng);
            }

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
        //Debug.Log("movePower: " + movePower);
        Vector3 moveVelocity = Vector3.zero;

        if (isStop == false)
        {
            if (isTracing && isY)//일정 거리 내이면 추적
            {
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
                    movePower = 50;
                    isAttack = false;
                }
                else
                {
                    movePower = 12;//추적 시에 속도 빠르게
                }

                if (target.x < me.x)//땡이가 왼쪽이면
                {
                    if (isHeart)
                    {
                        StartCoroutine("ClipMovementleft");//3초동안 왼쪽으로 
                    }

                    if (timeAfter >= Rate)//설정해 둔 파이어볼 생성 주기보다 timeAfter가 크면 
                    {
                        isStop = true;//멈춤 후 공격 
                    }
                    
                    else
                    {
                        dist = "Left";
                    }
                }

                else if (target.x > me.x)//땡이가 오른쪽이면
                {
                    if (isHeart)
                    {
                        StartCoroutine("ClipMovementright");//3초동안 오른쪽으로 
                    }

                    if (timeAfter >= Rate)//설정해 둔 파이어볼 생성 주기보다 timeAfter가 크면 
                    {
                        isStop = true;//멈춤 후 공격 
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag =="miniwall")
        {
            if(other.gameObject.transform.position.x <= transform.position.x)//벽이 왼쪽이면 
            {
                StartCoroutine("ClipMovementright");
            }
            else if(other.gameObject.transform.position.x > transform.position.x)
            {
                StartCoroutine("ClipMovementleft");
            }
        }
    }
    public void TakeDamage(int damage)//땡이한테 맞기위함 
    {
        GameObject damageText = Instantiate(DamageText);
        damageText.transform.position = head.position;
        damageText.GetComponent<DamageText>().damage = damage;
    }

}