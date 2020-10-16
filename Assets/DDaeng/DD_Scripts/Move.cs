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
    bool isbig; // 빅보 스킬 활성화 중인지?
    bool left; // 캐릭터의 좌우 저장

    //h
    public int HP;        //HP
    int HPMax;            //최대 체력
    GameObject hp_bar;  //hp바
    float hpbar_sx;         //hp바 스케일 x값
    float hpbar_tx;         //hp바 위치 x값
    float hpbar_tmp;        //hp바 감소 정도
    //

    // Start is called before the first frame update
    void Start()
    {
        //h
        HPMax = 400;
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
        TimeScale = 1000.0f; //속도 조정 변수
        G = 98f / TimeScale ; // 중력 가속도 저장
      //  G = 98f / TimeScale;
        Velocityg = 0; // 초기 중력 0 
        position = gameObject.transform.position; 
        isUp = false;
        isDown = false;
        onFloor = false;
        jump_y = 0;
        floor = 150;
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isbig) // 빅보 활성화 중인지
        {
            if ( position.y - Ground.transform.position.y > 6.5f)
            {
                if (!isUp && !onFloor)
                {
                    Debug.Log(onFloor);
                    isDown = true;
                    Velocityg -= G;
                    gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
                }
            }
            else if (position .y< 6.5f)
            {
                 Debug.Log("t");
                gameObject.transform.position = new Vector3(position.x, 6.5f, position.z);
            }
            else
            {
                 Debug.Log("ttt");
                isDown = false;
                Velocityg = 0f;
            }
        }
        else if (position .y- Ground.transform.position.y > 3f) // 현재 위치가 땅바닥과 떨어져있는가? ( 3f 가 주인공의 기본 y좌표)
        {
            if (!isUp && !onFloor ) //
            {
                Debug.Log(onFloor);
                isDown = true;
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
        }
        else if (position .y< 3f)
        {
            Debug.Log("t");
            gameObject.transform.position = new Vector3(position.x, 3f, position.z);
        }
        else
        {
            Debug.Log("ttt");
            isDown = false;
            Velocityg = 0f;
        }

        if(isbig && isFloor)
        {
            if (( position.y - Floor[floor].transform.position.y) > 6f)
            {
                   
                Debug.Log("ddd");
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
            else if ((position .y- Floor[floor].transform.position.y) <= 6f)
            {
                
                Debug.Log("dd");
                onFloor = true;
                gameObject.transform.position = new Vector3(position.x, Floor[floor].transform.position.y + 6f, position.z);
                Velocityg = 0;
            }
        }
        else if (isFloor)
        {
            if ((position.y - Floor[floor].transform.position.y) > 2.2f)
            {
                Debug.Log("ddd");
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
            else if ((position.y - Floor[floor].transform.position.y) < 2.1f)
            {
                Debug.Log("dd");
                onFloor = true;
                gameObject.transform.position = new Vector3(position.x, Floor[floor].transform.position.y + 2.1f, position.z);
                Velocityg = 0;
            }

        }

        if(isbig)
        {
            distance_floor = 0;
            int cnt = 0;
            for (int i = 0; i < Floor.Length; i++)
            {
                if (position.x < Floor[i].transform.position.x + 17.5 && position.x > Floor[i].transform.position.x - 17.5)
                {

                    if ((Floor[i].transform.position.y + 6f) < position.y )
                    {
                        if (cnt == 0)
                        {
                            floor = i;
                            distance_floor = position .y- Floor[i].transform.position.y;

                            cnt++;
                        }
                        else if (distance_floor > position.y -  Floor[i].transform.position.y)
                        {
                            floor = i;
                            distance_floor = position .y - Floor[i].transform.position.y;

                        }
                    }

                }
            }
            if (floor != 150)
            {
                isFloor = true;
            }
        }
        else if (isDown && !isUp && !isFloor )
        {
            distance_floor = 0;
            int cnt = 0;
            for (int i = 0; i < Floor.Length; i++)
            {
                if (position .x < Floor[i].transform.position.x + 17.5 && position.x > Floor[i].transform.position.x - 17.5)
                {

                    if ((Floor[i].transform.position.y + 2.1f) < position.y )
                    {
                        if (cnt ==0)
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
        if(onFloor)
        {
            if (Mathf.Abs(Floor[floor].transform.position.x - position.x ) > 17.5)
            {
                onFloor = false;
                isFloor = false;
                isDown = true;
                floor = 150;
            }
        }
        //좌우이동
        if (Input.GetKey(KeyCode.LeftArrow) && position.x > -60f )
        {
            
            if (isDown && !onFloor)
            {
                left = true;
                Velocityg -= G;
                if (position.y + (Velocityg * 0.1f) <=3f)
                {
                    gameObject.transform.position = new Vector3(position.x - 0.5f,3f, position.z);
                }
                else
                    gameObject.transform.position = new Vector3(position.x - 0.5f, position.y + (Velocityg * 0.1f), position.z);
                
            }
            else
            {
                left = true;
                gameObject.transform.position = new Vector3(position.x - 0.5f, position.y, position.z);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow) && position.x< 60f)
        {
            if (isDown && !onFloor)
            {
                left = false;
                Velocityg -= G;
                if (position.y + (Velocityg * 0.1f) <= 3f)
                {
                    gameObject.transform.position = new Vector3(position.x - 0.5f, 3f, position.z);
                }
                else
                    gameObject.transform.position = new Vector3(position.x + 0.5f, position.y + (Velocityg * 0.1f), position.z);
            }
            else
            {
                left = false;
                gameObject.transform.position = new Vector3(position.x + 0.5f, position.y, position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if ((!isUp && !isDown) || (onFloor&&!isUp) || (onFloor&& !isDown && isUp && isFloor)) // 마지막은 계단 내려가는도중의 경우
            {
                isUp = true;
                past_y = position.y;
            }
            
        }
        if (isUp)
        {
            if (jump_y < 20f )
            {
               
                jump_y += 0.5f;
                if (Input.GetKey(KeyCode.RightArrow)&& position.x < 60f)
                {
                    if (past_y + jump_y >= 65f)
                    {
                        gameObject.transform.position = new Vector3(position.x, 65f, position.z);
                    }
                    else
                    {
                        left = false;
                        gameObject.transform.position = new Vector3(position.x + 0.5f, past_y + jump_y, position.z);
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow)&& position.x > -60f)
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
                    if(past_y+jump_y >= 65f)
                    {
                        gameObject.transform.position = new Vector3(position.x, 65f, position.z);
                    }
                    else
                        gameObject.transform.position = new Vector3(position.x, past_y + jump_y, position.z);
                }
            }
            else
            {
                isFloor = false;
                isUp = false;
                jump_y = 0;
            }
        }

        //음파 발사
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SoundWave != null)
            {
                GameObject wave = GameObject.Instantiate(SoundWave);

                if (left)
                {
                    wave.transform.position = transform.position + new Vector3(-1, 0, 0);
                    wave.transform.parent = null;
                }
                else
                {
                    wave.transform.position = position + new Vector3(+1, 0, 0);
                    wave.transform.parent = null;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(bone !=null)
            {
                GameObject Bone = GameObject.Instantiate(bone);
                
                if (left)
                {
                    Bone.transform.position = transform.position + new Vector3(-5, 0, 0);
                    Bone.transform.parent = null;
                }
                else
                {
                    Bone.transform.position = position + new Vector3(+5, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.W)) 
        {
            isbig = true; // 빅보 활성화
            time = 0; // time 초기화
        }
        if (isbig)
        {
            if (time >= 20f) //  time 20 이내
            {
                if (scale > 1.0f) // 
                {
                    scale -= 0.01f;
                    transform.localScale = new Vector3(scale, scale, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isbig = false;
                }
            }
            else if (scale < 4f)
            {
                transform.localScale = new Vector3(scale, scale, 1);
                scale += 0.01f;
            }

            else
            {
                Debug.Log("?");
                time += 0.01f;
            }
        }

        position = gameObject.transform.position; // 위치 저장
        Debug.Log("다운 : " + isDown);
        Debug.Log("up : " + isUp);
       // Debug.Log("florr :" + isFloor);
        Debug.Log(floor + "ON?? : " + onFloor);
    }
    public void BigBo()
    {

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
    public void TakeDamage(int damage)//몬스터 들한테 맞기위함 
    {
        
        GameObject damageText = Instantiate(DamageText);

        damageText.transform.position = head.position;//기본 head : 오른쪽 
        damageText.GetComponent<DamageText>().damage = damage;
    }
}
