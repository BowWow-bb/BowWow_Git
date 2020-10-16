using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    GameObject Ground;
    GameObject[] Floor;

    public GameObject SoundWave = null;
    public GameObject bone = null;
    public GameObject DamageText;
    public Transform head;//데미지 텍스트 뜨는 위치
    public Transform headleft;//데미지 텍스트 반전 위함
    public Transform headright;
    Vector3 position;
    float G; // 중력 가속도
    float Velocityg; // 떨어지는 속도
    float TimeScale;//타임 스케일 조정
    float distance_floor;
    int floor;
    float jump_y;
    float past_y;
    float big;
    float time;
    bool isUp;
    bool isDown;
    bool isFloor;
    bool onFloor;
    bool isbig;
    bool left;

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
        isbig = false;
        big = 1f;
        left = true;
        Ground = GameObject.FindWithTag("Ground");
        Floor = GameObject.FindGameObjectsWithTag("Floor");
        TimeScale = 10000.0f;
        G = 98f / TimeScale * 100f;
      //  G = 98f / TimeScale;
        Velocityg = 0;
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
        if(isbig)
        {
            if (gameObject.transform.position.y - Ground.transform.position.y > 6.5f)
            {
                if (!isUp && !onFloor)
                {
                    Debug.Log(onFloor);
                    isDown = true;
                    Velocityg -= G;
                    gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
                }
            }
            else if (gameObject.transform.position.y < 6.5f)
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
        else if (gameObject.transform.position.y - Ground.transform.position.y > 3f)
        {
            if (!isUp && !onFloor )
            {
                Debug.Log(onFloor);
                isDown = true;
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
        }
        else if (gameObject.transform.position.y < 3f)
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
            if ((gameObject.transform.position.y - Floor[floor].transform.position.y) > 6f)
            {
                   
                Debug.Log("ddd");
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
            else if ((gameObject.transform.position.y - Floor[floor].transform.position.y) <= 6f)
            {
                
                Debug.Log("dd");
                onFloor = true;
                gameObject.transform.position = new Vector3(position.x, Floor[floor].transform.position.y + 6f, position.z);
                Velocityg = 0;
            }
        }
        else if (isFloor)
        {
            if ((gameObject.transform.position.y - Floor[floor].transform.position.y) > 2.2f)
            {
                Debug.Log("ddd");
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
            else if ((gameObject.transform.position.y - Floor[floor].transform.position.y) < 2.1f)
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
                if (gameObject.transform.position.x < Floor[i].transform.position.x + 17.5 && gameObject.transform.position.x > Floor[i].transform.position.x - 17.5)
                {

                    if ((Floor[i].transform.position.y + 6f) < gameObject.transform.position.y)
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
                if (gameObject.transform.position.x < Floor[i].transform.position.x + 17.5 && gameObject.transform.position.x > Floor[i].transform.position.x - 17.5)
                {

                    if ((Floor[i].transform.position.y + 2.1f) < gameObject.transform.position.y)
                    {
                        if (cnt ==0)
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
            if (floor != 150)
            {
                isFloor = true;
            }
        }
        if(onFloor)
        {
            if (Mathf.Abs(Floor[floor].transform.position.x - gameObject.transform.position.x) > 17.5)
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
            
            if (isDown)
            {
                left = true;
                Velocityg -= G;
                if (position.y + (Velocityg * 0.1f) <=3f)
                {
                    gameObject.transform.position = new Vector3(position.x - 1f,3f, position.z);
                }
                else
                    gameObject.transform.position = new Vector3(position.x - 1f, position.y + (Velocityg * 0.1f), position.z);
                
            }
            else
            {
                left = true;
                gameObject.transform.position = new Vector3(position.x - 1f, position.y, position.z);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow) && position.x< 60f)
        {
            if (isDown)
            {
                left = false;
                Velocityg -= G;
                if (position.y + (Velocityg * 0.1f) <= 3f)
                {
                    gameObject.transform.position = new Vector3(position.x - 1f, 3f, position.z);
                }
                else
                    gameObject.transform.position = new Vector3(position.x + 1f, position.y + (Velocityg * 0.1f), position.z);
            }
            else
            {
                left = false;
                gameObject.transform.position = new Vector3(position.x + 1f, position.y, position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if ((!isUp && !isDown) || (onFloor&&!isUp))
            {
                isUp = true;
                past_y = gameObject.transform.position.y;
            }
            
        }
        if (isUp)
        {
            if (jump_y < 20f )
            {
                jump_y += 1f;
         //       jump_y += 0.1f;
                if (Input.GetKey(KeyCode.RightArrow)&& position.x < 60f)
                {
                    if (past_y + jump_y >= 65f)
                    {
                        gameObject.transform.position = new Vector3(position.x, 65f, position.z);
                    }
                    else
                    {
                        left = false;
                        gameObject.transform.position = new Vector3(position.x + 1f, past_y + jump_y, position.z);
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
                        gameObject.transform.position = new Vector3(position.x - 1f, past_y + jump_y, position.z);
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
                    wave.transform.position = gameObject.transform.position + new Vector3(+1, 0, 0);
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
                    Bone.transform.position = gameObject.transform.position + new Vector3(+5, 0, 0);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            isbig = true;
            time = 0;
        }
        if (isbig)
        {
            if (time >= 20f)
            {
                if (big > 1.0f)
                {
                    big -= 0.01f;
                    transform.localScale = new Vector3(big, big, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isbig = false;
                }
            }
            else if (big < 4f)
            {
                transform.localScale = new Vector3(big, big, 1);
                big += 0.01f;
            }

            else
            {
                Debug.Log("?");
                time += 0.01f;
            }
        }

        position = gameObject.transform.position;
        Debug.Log("다운 : " + isDown);
        //Debug.Log("up : " + isUp);
        Debug.Log("florr :" + isFloor);
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
