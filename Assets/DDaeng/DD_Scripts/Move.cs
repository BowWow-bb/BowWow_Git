using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    GameObject Ground; // 땅바닥 오브젝트
    GameObject[] Floor; // 계단 오브젝트

    public GameObject SoundWave = null; // 음파 오브젝트
    public GameObject bone = null; // 뼈다귀 오브젝트
    public GameObject DamageText;
    public Transform head;//데미지 텍스트 뜨는 위치
    public Transform headleft;//데미지 텍스트 반전 위함
    public Transform headright;
    Vector3 position; // 오브젝트 위치 저장
    float G; // 중력 가속도
    float Velocityg; // 떨어지는 속도(중력)
    float TimeScale;//타임 스케일 조정
    float distance_floor; // 계단과의 위치
    int floor; // 계단배열의 인덱스값
    float jump_y; // 점프 한 거리
    float past_y; // 점프 하기 전 y축좌표(높이)
    float scale; // 오브젝트의 scale 값
    float time; // 시간세는 변수
    bool isUp; // 지금 위로 가고있는지
    bool isDown; // 아래로 떨어지고 있는지
    bool isFloor; // 아래에 계단이 있는지
    bool onFloor; // 현재 계단위에 올라와 있는지
    public bool isbig; // 빅보 스킬 활성화 중인지?
    bool left; // 캐릭터의 좌우 저장

    public bool BoneActive; //뼈다귀스킬 활성화
    public bool BigboActive;    //빅보스킬 활성화
    Q_Bone q_Bone;
    W_Bigbo w_Bigbo;
    //h
    public int HP;        //HP
    int HPMax;            //최대 체력
    GameObject hp_bar;  //hp바
    float hpbar_sx;         //hp바 스케일 x값
    float hpbar_tx;         //hp바 위치 x값
    float hpbar_tmp;        //hp바 감소 정도
    //
    AudioSource Attack;
    AudioSource Jump;
    
    public AudioClip AttackSound;
    public AudioClip JumpSound;

    // Start is called before the first frame update
    void Start()
    {
        q_Bone = GameObject.Find("Q_Bone").GetComponent<Q_Bone>();
        w_Bigbo = GameObject.Find("W_Bigbo").GetComponent<W_Bigbo>();
        BoneActive = false;
        BigboActive = false;
        //h
        HPMax = 900;
        HP = HPMax;
        hp_bar = GameObject.FindWithTag("DDaengHp");
        hpbar_sx = GameObject.FindWithTag("DDaengHp").transform.localScale.x;
        hpbar_tx = GameObject.FindWithTag("DDaengHp").transform.localPosition.x;
        hpbar_tmp = hpbar_sx / HPMax;
        //\
        isbig = false; // 초기화
        scale = 1f; // 캐릭터의 기본 스케일 1
        left = true; // 처음엔 왼쪽을 보고 시작
        Ground = GameObject.FindWithTag("Ground"); // 땅바닥 오브젝트 저장
        Floor = GameObject.FindGameObjectsWithTag("Floor"); // 계단들의 오브젝트배열 저장
        TimeScale = 400.0f; //속도 조정 변수
        G = 98f / TimeScale; // 중력 가속도 저장
                             //  G = 98f / TimeScale;
        Velocityg = 0; // 초기 중력 0 
        position = gameObject.transform.position;
        isUp = false;
        isDown = false;
        onFloor = false;
        jump_y = 0;
        floor = 150;
        time = 0f;

        Attack = gameObject.AddComponent<AudioSource>();
        Attack.clip = AttackSound;
        Attack.loop = false;

        Jump = gameObject.AddComponent<AudioSource>();
        Jump.clip = JumpSound;
        Jump.volume = 0.6f;
        Jump.loop = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isbig) // 빅보 활성화 중인지
        {
            // 아래 else 문의 빅보가 비활성화 일때와 y의 거리 기준점만 다르고 똑같다.
            if (gameObject.transform.position.y - Ground.transform.position.y > 6.5f)
            {
                if (!isUp && !onFloor)
                {
                    isDown = true;
                    Velocityg -= G;
                    gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
                }
            }
            else if (gameObject.transform.position.y < 6.5f)
            {
                gameObject.transform.position = new Vector3(position.x, 6.5f, position.z);
            }
            else
            {
                isDown = false;
                Velocityg = 0f;
            }
        }
        else if (gameObject.transform.position.y - Ground.transform.position.y > 3f) // 현재 위치가 땅바닥과 떨어져있는가? ( 3f 가 주인공의 기본 y좌표)
        {
            if (!isUp && !onFloor) // 점프하고 있을때 혹은 계단위에선 적용X
            {
                // 중력적용 - 상태를 내려가고 있음으로 체크
                isDown = true;
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
        }
        else if (gameObject.transform.position.y < 3f)
        {
            // 땅바닥을 뚫는걸 방지 , 바닥보다 내려간다면 위치 고정
            gameObject.transform.position = new Vector3(position.x, 3f, position.z);
        }
        else
        {
            //땅에 내려왔을시 - isDown 다시 false , 중력가속도 0
            isDown = false;
            Velocityg = 0f;
        }



        if (isbig&&(isDown && !isUp && !isFloor))
        {
            distance_floor = 0;
            int cnt = 0;
            for (int i = 0; i < Floor.Length; i++)
            {
                if (gameObject.transform.position.x < Floor[i].transform.position.x + 17.5 && position.x > Floor[i].transform.position.x - 17.5)
                {

                    if ((Floor[i].transform.position.y + 6f) < position.y)
                    {
                        if (cnt == 0)
                        {
                            floor = i;
                            distance_floor = position.y - Floor[i].transform.position.y;

                            cnt++;
                        }
                        else if (distance_floor > position.y - Floor[i].transform.position.y)
                        {
                            floor = i;
                            distance_floor = position.y - Floor[i].transform.position.y;

                        }
                    }

                }
            }
            if (floor != 150)
            {
                isFloor = true;
            }
        }
        else if (isDown && !isUp && !isFloor) // 점프중이 아닌 내려오는중 일때 계단인식
        {
            distance_floor = 0; // 계단과의 거리 초기화
            int cnt = 0;
            for (int i = 0; i < Floor.Length; i++)
            {
                // 1차적으로 플레이어 주변 x 범위 내의 계단을 찾음
                if (gameObject.transform.position.x < Floor[i].transform.position.x + 17.5 && gameObject.transform.position.x > Floor[i].transform.position.x - 17.5)
                {
                    // 그 다음 플레이어 제일 가까이 아래에 있는 계단의 인덱스를 찾는다.
                    if ((Floor[i].transform.position.y + 3.5f) < gameObject.transform.position.y)
                    {
                        if (cnt == 0)
                        {
                            floor = i;
                            distance_floor = gameObject.transform.position.y - Floor[i].transform.position.y;

                            cnt++;
                        }
                        else if (distance_floor > gameObject.transform.position.y - Floor[i].transform.position.y)
                        {
                            floor = i;
                            distance_floor = gameObject.transform.position.y - Floor[i].transform.position.y;

                        }
                    }

                }
            }
            if (floor != 150) // 계단을 찾았는지 안찾았는지 , 찾았다면 isFloor 변경
            {
                isFloor = true;
            }
        }
        if (isbig && isFloor) //아래 else 문의 빅보가 비활성화 일때와 y의 거리 기준점만 다르고 똑같다.
        {
            if ((gameObject.transform.position.y - Floor[floor].transform.position.y) > 6f)
            {
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
            else if ((gameObject.transform.position.y - Floor[floor].transform.position.y) <= 6f)
            {
                onFloor = true;
                gameObject.transform.position = new Vector3(position.x, Floor[floor].transform.position.y + 6f, position.z);
                Velocityg = 0;
            }
        }
        else if (isFloor) // 밑의 계단발견
        {
            if ((gameObject.transform.position.y - Floor[floor].transform.position.y) > 2.2f) // 계단과 플레이어의 수직거리가 일정 거리 이상일 때
            {
                // 중력가속도 적용
                Velocityg -= G;
                gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + (Velocityg * 0.001f), position.z);
            }
            else if ((gameObject.transform.position.y - Floor[floor].transform.position.y) < 2.2f) // 계단위에 플레이어가 올라왔을 때
            {
                // 올라왔다고 상태체크 , y축고정 , 중력가속도 초기화
                onFloor = true;
                gameObject.transform.position = new Vector3(transform.position.x, Floor[floor].transform.position.y + 2.2f, position.z);
                Velocityg = 0;
            }

        }
        if (onFloor)
        {
            if (Mathf.Abs(Floor[floor].transform.position.x - gameObject.transform.position.x) > 17.5)
            {
                // 계단위에 있을때 그 계단의 좌,우에서 벗어나면 on,isFloor 변경 , 떨어지므로 isDown 
                onFloor = false;
                isFloor = false;
                isDown = true;
                floor = 150;
            }
        }
        //좌우이동
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -60f) // 좌로 -60 제한
        {

            if (isDown && !onFloor) // 내려오는 중에 눌렀다면
            {
                // 중력 적용하면서 좌 이동
                left = true;
                Velocityg -= G;
                if (position.y + (Velocityg * 0.1f) <= 3f)
                {
                    gameObject.transform.position = new Vector3(position.x - 0.75f, 3f, position.z);
                }
                else
                    gameObject.transform.position = new Vector3(position.x - 0.75f, transform.position.y + (Velocityg * 0.1f), position.z);

            }
            else
            {
                // 좌로 이동 , left 변경
                left = true;
                gameObject.transform.position = new Vector3(position.x - 0.75f, transform.position.y, position.z);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 60f) // 우로 60 제한
        {
            if (isDown && !onFloor)
            {
                //중력 적용하면서 우로 이동
                left = false;
                Velocityg -= G;
                if (position.y + (Velocityg * 0.1f) <= 3f)
                {
                    gameObject.transform.position = new Vector3(position.x - 0.75f, 3f, position.z);
                }
                else
                    gameObject.transform.position = new Vector3(position.x + 0.75f, transform.position.y + (Velocityg * 0.1f), position.z);
            }
            else
            {
                //우로 이동 , left 변경
                left = false;
                gameObject.transform.position = new Vector3(position.x + 0.75f, transform.position.y, position.z);
            }
        }

        if (Input.GetKey(KeyCode.UpArrow)) //점프 눌렀을시
        {
            // 내려가지도,점프하고있지도 않을때(2단점프 방지) , 계단 위 면서 올라가지 않고있을때 , 계단에서 내려가는 도중 점프 방지
            if ((!isUp && !isDown) || (onFloor && !isUp) || (onFloor && !isDown && isUp && isFloor))
            {
                Jump.Play();

                //눌렀을 당시 y축좌표 저장 , 점프중인지 확인하는 변수 isUp 변경
                isUp = true;
                onFloor = false;
                past_y = gameObject.transform.position.y;
            }

        }
        if (isUp) // 점프중이라면
        {
            if (jump_y < 20f) // 점프 거리가 아직 20 이하일때
            {

                jump_y += 1f; // 점프 거리 늘림
                if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 60f) // 오른/왼쪽을 누르면서 점프할때
                {
                    if (past_y + jump_y >= 65f)
                    {
                        gameObject.transform.position = new Vector3(position.x, 65f, position.z);
                    }
                    else
                    {
                        left = false; // 좌/우로 이동시켜주면서 점프 해줌
                        gameObject.transform.position = new Vector3(position.x + 0.5f, past_y + jump_y, position.z);
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && position.x > -60f)
                {
                    if (past_y + jump_y >= 65f)
                    {
                        gameObject.transform.position = new Vector3(position.x, 65f, position.z);
                    }
                    else
                    {
                        gameObject.transform.position = new Vector3(position.x - 0.5f, past_y + jump_y, position.z);
                        left = true;
                    }
                }
                else
                {

                    if (past_y + jump_y >= 65f) // 점프했을 때의 결과가 천장보다 높다면 고정
                    {
                        gameObject.transform.position = new Vector3(position.x, 65f, position.z);
                    }
                    else // 점프거리를 늘려준 만큼 y축좌표를 높여줌 
                        gameObject.transform.position = new Vector3(position.x, past_y + jump_y, position.z);
                }
            }
            else // 점프거리가 20 이상이 됐을 때
            {
                // isFloor 초기화 ( 다시 계단 인식을 위해 ) ,isUp 초기화 ,점프거리 초기화
                isFloor = false;
                isUp = false;
                jump_y = 0;
            }
        }
        position = gameObject.transform.position; 
    }

    //h
    public void hpMove(int hp_delta)
    {
        if (HP <= 0)
            Destroy(gameObject);

        HP -= hp_delta;
        float move = ((HPMax - HP) + hp_delta) * hpbar_tmp;

        Vector3 Scale = hp_bar.transform.localScale;
        hp_bar.transform.localScale = new Vector3(hpbar_sx - move, Scale.y, Scale.z);

        Vector3 Pos = hp_bar.transform.localPosition;
        hp_bar.transform.localPosition = new Vector3(hpbar_tx - move / 2.0f, Pos.y, Pos.z);
    }
    //
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SoundWave != null)
            {
                GameObject wave = GameObject.Instantiate(SoundWave);
                Attack.Play();

                if (left)
                {
                    // 플레이어가 좌를 보고 있다면 왼쪽에 생성
                    wave.transform.position = transform.position + new Vector3(-1, 0, 0);
                    wave.transform.parent = null;
                }
                else
                {
                    // 우를 보고 있다면 오른쪽에 생성
                    wave.transform.position = gameObject.transform.position + new Vector3(+1, 0, 0);
                    wave.transform.parent = null;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (BoneActive) //뼈다귀 활성화 경우
            {
                if (bone != null)
                {
                    GameObject Bone = GameObject.Instantiate(bone);

                    if (left)// 플레이어가 좌를 보고 있다면 왼쪽에 생성
                    {
                        Bone.transform.position = transform.position + new Vector3(-5, 0, 0);
                        Bone.transform.parent = null;
                    }
                    else// 우를 보고 있다면 오른쪽에 생성
                    {
                        Bone.transform.position = gameObject.transform.position + new Vector3(+5, 0, 0);
                        Bone.transform.parent = null;
                    }
                }
            }
            BoneActive = false;
            q_Bone.isThere = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (BigboActive)    //빅보 활성화 경우
            {
                isbig = true; // 빅보 활성화
                time = 0; // time 초기화
            }
            if (isbig) // 빅보 활성화 시
            {
                if (time >= 20f) //  time 20 이상이 됐다면
                {
                    if (scale > 1.0f) // 아직 scale이 1 이상일 때
                    {
                        scale -= 0.01f; // scale 줄여주고 적용
                        transform.localScale = new Vector3(scale, scale, 1);
                    }
                    else
                    {
                        // 1이하 됐다면 scale 1 고정, 빅보 비활성화
                        transform.localScale = new Vector3(1, 1, 1);
                        isbig = false;
                    }
                }
                else if (scale < 4f)
                {
                    // time 20 이하 , 아직 플레이어가 덜 커졌다면 
                    // scale 키워주고 적용
                    scale += 0.01f;
                    transform.localScale = new Vector3(scale, scale, 1);
                }

                else
                {
                    // 빅보로 인해 scale 완전히 커진 상태 , time 세준다.
                    time += 0.01f;
                }
            }
            BoneActive = false;
            w_Bigbo.isThere = false;
        }
    }
    void OnTriggerEnter(Collider other)//몬스터 때리기 
    {
        if (isbig)  //빅보 활성화 경우만 충돌 적용
        {
            if (other.gameObject.GetComponent<small_toll>() != null)    //스테이지2 몬스터와 충돌한 경우
            {
                small_toll monster = other.GetComponent<small_toll>();
                monster.TakeDamage(20);//공격         
                monster.hpMove(20);
            }
            if (other.gameObject.GetComponent<small_toll_stage1>() != null)  //스테이지1 몬스터와 충돌한 경우
            {
                small_toll_stage1 monster = other.GetComponent<small_toll_stage1>();
                monster.TakeDamage(20);
                monster.hpMove(20);
            }
            if (other.gameObject.GetComponent<Bigtol>() != null)
            {
                Bigtol monster = other.GetComponent<Bigtol>();
                monster.TakeDamage(20);
                monster.hpMove(20);
            }
        }
    }

    public void TakeDamage(int damage)//몬스터 들한테 맞기위함 
    {

        GameObject damageText = Instantiate(DamageText);

        damageText.transform.position = head.position;//기본 head : 오른쪽 
        damageText.GetComponent<DamageText>().damage = damage;
    }
}