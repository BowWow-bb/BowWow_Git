using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class small_toll_stage1 : MonoBehaviour
{
    //스몰톨 스테이지1

    public GameObject DDaeng;//땡이
    public GameObject DamageText;
    public GameObject smalltoll;//스몰톨
    public GameObject Bone_Perfab;
    public GameObject Bigbo_Perfab;

    GameObject[] Floor;//계단 오브젝트
    GameObject Ground; // 땅바닥 오브젝트

    public Transform head;//데미지 텍스트 뜨는 위치
    public Transform headleft;
    public Transform headright;
    Transform st;

    Vector3 target;//땡이 위치
    Vector3 me;//스몰톨 위치 

    public float d = 20f;//범위 거리 설정
    float distance;
    float movePower = 5f;//움직이는 속력
    float RateMin = 0.5f;//최소 생성 주기 
    float RateMax = 3f;//최대 생성 주기
    float Rate;//파이어볼 생성 주기
    float Ypos;
    float timeAfter;//발사 후 지난 시간

    float G;//중력 가속도
    float Velocityg;//떨어지는 속도(중력)
    float TimeScale;
    float distance_floor;//계단과의 위치

    int floor;//계단배열들의 인덱스 

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

    bool isDown = false;//땡이 추적하면서 바닥에 떨어짐
    bool isFloor = false;//떨어지면서 계단 파악
    bool onFloor = false;//현재 계단 위 인지 

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
    AudioSource Apa;
    public AudioClip ApaSound;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("위치: " + gameObject.transform.position); //world 좌표임

        //h
        HPMax = 100;
        HP = HPMax;
        tag_name = transform.Find("HpBar").transform.Find("Hp").tag;
        hp_bar = GameObject.FindWithTag(tag_name);
        //Debug.Log(transform.Find("HpBar").transform.Find("Hp").tag);

        hpbar_sx = hp_bar.transform.localScale.x;
        hpbar_tx = hp_bar.transform.localPosition.x;
        hpbar_tmp = hpbar_sx / HPMax;
        //

        DDaeng = GameObject.Find("DDaeng");
        Ground = GameObject.FindWithTag("Ground"); // 땅바닥 오브젝트 저장
        Floor = GameObject.FindGameObjectsWithTag("Floor"); // 계단들의 오브젝트배열 저장
        TimeScale = 400.0f; //속도 조정 변수
        G = 98f / TimeScale; // 중력 가속도 저장
                             //  G = 98f / TimeScale;


        timeAfter = 0f;
        Rate = Random.Range(RateMin, RateMax);

        st = smalltoll.transform.Find("warning");
        st.gameObject.SetActive(false);

        StartCoroutine("ChangeMovement");

        Apa = gameObject.AddComponent<AudioSource>();//땡이 때리는 소리 
        Apa.clip = ApaSound;
        Apa.volume = 0.3f;
        Apa.loop = false;
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
        StartCoroutine("ChangeMovement");
    }
    //스몰톨이 카메라 벗어나지 않게 제한 
    IEnumerator ClipMovementleft()//왼쪽으로 가는 코루틴 실행
    {
        movementFlag = 1;
        //Debug.Log("코루틴 left");

        yield return new WaitForSeconds(2f);

        if (!isAttack_once)
        {
            StartCoroutine("ChangeMovement");
        }
    }

    IEnumerator ClipMovementright()//오른쪽으로 가는 코루틴 실행 
    {
        movementFlag = 2;
        //Debug.Log("코루틴 right");

        yield return new WaitForSeconds(2f);

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
        StartCoroutine("ChangeMovement");
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        timeAfter += Time.deltaTime;//시간 갱신
        Vector3 position = transform.position;

        Distance();

        if (isDown&& (gameObject.transform.position.y - Ground.transform.position.y > 3f)) // 현재 위치가 땅바닥과 떨어져있는가? ( 3f 가 주인공의 기본 y좌표)
        {
            if (!onFloor) // 점프하고 있을때 혹은 계단위에선 적용X
            {
                Debug.Log(onFloor + "D");
                // 중력적용 - 상태를 내려가고 있음으로 체크
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
        }
        else if (gameObject.transform.position.y < 3f)
        {
            // 땅바닥을 뚫는걸 방지 , 바닥보다 내려간다면 위치 고정
            //    Debug.Log("t");
            gameObject.transform.position = new Vector3(position.x, 3f, position.z);
        }
        else
        {
            //땅에 내려왔을시 - isDown 다시 false , 중력가속도 0
            // Debug.Log("ttt");
            isDown = false;
            Velocityg = 0f;
        }

        if (isDown&&!isFloor)//추락하는 상태이면
        {
            distance_floor = 0;//계단과의 거리
            int cnt = 0;
            for(int i=0;i<Floor.Length;i++)//계단 인식 
            {
                //톨이 주변 계단 찾기
                if(gameObject.transform.position.x<Floor[i].transform.position.x+17.5&&gameObject.transform.position.x>Floor[i].transform.position.x-17.5)
                {
                    if((Floor[i].transform.position.y+3.5f)<gameObject.transform.position.y)
                    {
                        if(cnt==0)
                        {
                            floor = i;
                            distance_floor = gameObject.transform.position.y - Floor[i].transform.position.y;
                            cnt++;
                        }
                        else if(distance_floor>gameObject.transform.position.y-Floor[i].transform.position.y)
                        {
                            floor = i;
                            distance_floor = gameObject.transform.position.y - Floor[i].transform.position.y;
                        }
                    }
                }
            }
            if(floor !=150)//계단을 찾았는지
            {
                isFloor = true;
            }
        }

        if (isFloor)//계단발견 한 경우 
        {
            if ((gameObject.transform.position.y - Floor[floor].transform.position.y) > 2.2f) // 계단과 플레이어의 수직거리가 일정 거리 이상일 때
            {
                //     Debug.Log("ddd");
                // 중력가속도 적용
                //   Velocityg -= G;
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + (Velocityg * 0.001f), position.z);
            }
            else if ((gameObject.transform.position.y - Floor[floor].transform.position.y) < 2.2f) // 계단위에 플레이어가 올라왔을 때
            {
                //  Debug.Log("dd");
                // 올라왔다고 상태체크 , y축고정 , 중력가속도 초기화
                if (Mathf.Abs(Floor[floor].transform.position.x - gameObject.transform.position.x) < 17.5)
                {
                    onFloor = true;
                    gameObject.transform.position = new Vector3(transform.position.x, Floor[floor].transform.position.y + 2f, position.z);
                    Velocityg = 0;
                }
            }

            // 계단위에 있을때 그 계단의 좌,우에서 벗어나면 on,isFloor 변경 , 떨어지므로 isDown 
            if (Mathf.Abs(Floor[floor].transform.position.x - gameObject.transform.position.x) > 17.5)
            {
                //벗어난경우
                onFloor = false;
                isFloor = false;
                floor = 150;
            }
        }
        Move();
    }

    //h
    public void hpMove(int hp_delta)
    {
        if (HP - hp_delta <= 0)
        {
            //아이템 생성 후 몬스터 소멸
            itemManager();
            Destroy(gameObject);
        }

        float move = ((HPMax - HP) + hp_delta) * hpbar_tmp;
        HP -= hp_delta;

        Vector3 Scale = hp_bar.transform.localScale;
        hp_bar.transform.localScale = new Vector3(hpbar_sx - move, Scale.y, Scale.z);

        Vector3 Pos = hp_bar.transform.localPosition;
        hp_bar.transform.localPosition = new Vector3(hpbar_tx - move / 2.0f, Pos.y, Pos.z);
    }
    void itemManager()  //아이템생성
    {
        int i = Random.Range(0, 30);
        if (i > 10)   //뼈다귀 스킬 아이템 생성
        {
            GameObject Bone = GameObject.Instantiate(Bone_Perfab);
            Bone.transform.position = transform.position;  //생성위치 = 현재 몬스터 위치
            Bone.transform.parent = null;
        }
        else if (i <= 10)   //빅보 스킬 아이템 생성 (뼈다귀 스킬 보다 확률 더 적게 조정)
        {
            GameObject Bigbo = GameObject.Instantiate(Bigbo_Perfab);
            Bigbo.transform.position = transform.position; //생성위치 = 현재 몬스터 위치
            Bigbo.transform.parent = null;
        }
    }
    //

    void Distance()//거리 파악. 트리거 대신 
    {
        target = DDaeng.transform.position;
        me = transform.position;

        distance = Mathf.Abs(target.x - me.x);
        Ypos = Mathf.Abs(target.y - transform.position.y);//절댓값(땡이의 y값 - 스몰 톨의 y값)

        if (Ypos <= 8)
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

        if (distance <= d && isY)//범위 내에 처음 들어오면.
        {
            st.gameObject.SetActive(true);
            Enter = true;
            StopCoroutine("ChangeMovement");//이동하던 거 멈추고 추적 시작
        }

        if (Enter == true && distance <= d && distance > 9 && isY)//들어 온 상태이고 범위 내에 계속 있으면 
        {
            isTracing = true;//추격 중 
            isAttack = false;
        }

        if(distance <=11 && isY)//빠르게 움직
        {
            isAttack = true;
        }

        if (isTouch && isY)//공격 범위 내이면 
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

            if (isAttack_once&&!dd.isbig)//한 번 만 공격 
            {
                Apa.Play();
                dd.TakeDamage(10);//텍스트 데미지 
                dd.hpMove(10);
            }
            isAttack_once = false;
        }
        if(!isTracing&&!isY&&isAttack)//근접 공격중 땡이가 계단 올라가면  
        {
            //Debug.Log("distance: " + distance);
            StopAllCoroutines();
            isAttack = false;
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
        if ((isTracing == true && distance > d && isY) || (isTracing == true && distance > d && !isY))//거리 벗어나면 
        {
            StopAllCoroutines();

            st.gameObject.SetActive(false);
            Enter = false;
            isTracing = false;
            isY = false;
            isHeart = false;

            StartCoroutine("ChangeMovement");
        }
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (isStop == false)
        {
            //일정 거리 내이면 추적. 벽이랑 접해있지 않을 때 
            if ((isTracing && isY &&!isTouch) || (!isAttack && !isTracing && isY && isHeart && !isTouch))
            {
                if (timeAfter >= Rate && !isHeart)
                {
                    isStop = true;//멈춤 후 공격 
                }

                //추격 중에 Y값 조건 체크 
                if (Ypos <= 8)
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
                    movePower = 25;//추적 시에 속도 빠르게
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
        if (other.gameObject.tag == "DDaeng")
        {
            //Debug.Log(other.gameObject.transform.position.x - transform.position.x);
            isAttack_once = true;
            isTouch = true;
        }

        if (other.gameObject.tag == "SoundWave")
        {
            if (!isTouch && isY)//땡이랑 닿지 않았을 때 
            {
                StopCoroutine("ChangeMovement");
                isHeart = true;
                StartCoroutine("Heart");
            }
            else if (isTracing && distance > d && !isY)
            {
                Debug.Log("soundWave쪽");

                StopAllCoroutines();
                isTracing = false;
                isHeart = false;
                StartCoroutine("ChangeMovement");
            }
        }
        if(other.gameObject.tag == "miniwall")//미니월 만나면 
        {
            //StopCoroutine("ChangeMovement");
            if(onFloor&&isTracing)
            {
                //Debug.Log("isOnFloor: " + onFloor);
                isDown = true;
                onFloor = false;
            }

            else
            {
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
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "miniwall")//미니월 만나면 
        {
            //StopCoroutine("ChangeMovement");
            if (isTracing&&isY)
            {
                isDown = true;//추적 중인 경우에만 아래로 가는 거 허용 
            }

            else
            {
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
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "DDaeng")
        {
            isAttack_once = false;
        }
        if (other.gameObject.tag == "miniwall")
        {
            isTracing = false;
        }
    }
}