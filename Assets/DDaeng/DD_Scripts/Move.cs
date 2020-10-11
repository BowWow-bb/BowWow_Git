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

    Vector3 position;
    float G; // 중력 가속도
    float Velocityg; // 떨어지는 속도
    float TimeScale;//타임 스케일 조정
    float distance_floor;
    int floor;
    float jump_y;
    float past_y;
    bool isGround;
    bool isUp;
    bool isDown;
    bool isFloor;
    bool onFloor;
    bool left;
    // Start is called before the first frame update
    void Start()
    {
        left = true;
        Ground = GameObject.FindWithTag("Ground");
        Floor = GameObject.FindGameObjectsWithTag("Floor");
        Debug.Log("층 계수" + Floor.Length);
        TimeScale = 10000.0f;
        G = 98f / TimeScale;
        Velocityg = 0;
        position = gameObject.transform.position;
        isUp = false;
        isDown = false;
        onFloor = false;
        jump_y = 0;
        floor = 150;
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.transform.position.y - Ground.transform.position.y > 3f)
        {
            if (!isUp && !onFloor )
            {
                //Debug.Log(onFloor);
                isDown = true;
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
        }
        else if (gameObject.transform.position.y < 3f)
        {
           // Debug.Log("t");
            gameObject.transform.position = new Vector3(position.x, 3f, position.z);
        }
        else
        {
           // Debug.Log("ttt");
            isDown = false;
            Velocityg = 0f;
        }
        if (isFloor)
        {
            if ((gameObject.transform.position.y - Floor[floor].transform.position.y) > 2.5f)
            {
             //   Debug.Log("ddd");
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
            else if ((gameObject.transform.position.y - Floor[floor].transform.position.y) <= 2.5f)
            {
              //  Debug.Log("dd");
                onFloor = true;
                gameObject.transform.position = new Vector3(position.x, Floor[floor].transform.position.y + 2.5f, position.z);
                Velocityg = 0;
            }

        }
        if (isDown && !isUp && !isFloor )
        {
            distance_floor = 0;
            int cnt = 0;
            for (int i = 0; i < Floor.Length; i++)
            {
                if (gameObject.transform.position.x < Floor[i].transform.position.x + 9.5 && gameObject.transform.position.x > Floor[i].transform.position.x - 9.5)
                {

                    if ((Floor[i].transform.position.y + 2.5f) < gameObject.transform.position.y)
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
            if (Mathf.Abs(Floor[floor].transform.position.x - gameObject.transform.position.x) > 9.5f)
            {
                onFloor = false;
                isFloor = false;
                floor = 150;
            }
        }
        //좌우이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isDown)
            {
                left = true;
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x - 0.05f, position.y + (Velocityg * 0.1f), position.z);
            }
            else
            {
                left = true;
                gameObject.transform.position = new Vector3(position.x - 0.05f, position.y, position.z);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isDown)
            {
                left = false;
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x + 0.05f, position.y + (Velocityg * 0.1f), position.z);
            }
            else
            {
                left = false;
                gameObject.transform.position = new Vector3(position.x + 0.05f, position.y, position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isUp ||( isDown&& !isFloor ) )
            {
                isUp = true;
                past_y = gameObject.transform.position.y;
            }
            
        }
        if (isUp)
        {
            if (jump_y < 16f)
            {
                jump_y += 0.1f;
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    gameObject.transform.localScale = new Vector3(+1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    gameObject.transform.position = new Vector3(position.x + 0.05f, past_y + jump_y, position.z);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    gameObject.transform.position = new Vector3(position.x - 0.05f, past_y + jump_y, position.z);
                    gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                }
                else
                {
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
                    Bone.transform.position = transform.position + new Vector3(-1, 0, 0);
                    Bone.transform.parent = null;
                }
                else
                {
                    Bone.transform.position = gameObject.transform.position + new Vector3(+1, 0, 0);
                }
            }
        }


        position = gameObject.transform.position;
        //Debug.Log("다운 : " + isDown);
        //Debug.Log("up : " + isUp);
        //Debug.Log("florr :" + isFloor);
        //Debug.Log(floor + "ON?? : " + onFloor);
    }
    public void TakeDamage(int damage)//몬스터들한테 맞기위함 
    {
        GameObject damageText = Instantiate(DamageText);
        damageText.transform.position = head.position;
        damageText.GetComponent<DamageText>().damage = damage;
    }
}
