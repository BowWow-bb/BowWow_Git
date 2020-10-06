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
    Vector3 position;
    float G; // 중력 가속도
    float Velocityg; // 떨어지는 속도
    float TimeScale;//타임 스케일 조정
    float jump_y;
    float past_y;
    float distance_floor;
    int floor;
    bool isGround;
    bool isUp;
    bool isDown;
    bool isFloor;
    // Start is called before the first frame update
    void Start()
    {
        Ground = GameObject.FindWithTag("Ground");
        Floor = GameObject.FindGameObjectsWithTag("Floor");
        Debug.Log("층 계수" + Floor.Length);
        TimeScale = 10000.0f;
        G = 9.8f / TimeScale;
        Velocityg = 0;
        position = gameObject.transform.position;
        isUp = false;
        isDown = false;
        isFloor = false;
        jump_y = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.transform.position.y - Ground.transform.position.y > 3.2f)
        {
            if (isUp == false)
            {
                isDown = true;
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x, position.y + (Velocityg * 0.1f), position.z);
            }
        }
        else if (gameObject.transform.position.y < 3.2f)
        {
            gameObject.transform.position = new Vector3(position.x, 3.2f, position.z);
        }
        else
        {
            isDown = false;
            Velocityg = 0f;
        }
        if (isDown)
        {
            floor = 150;
            distance_floor = 0;
            for (int i = 0; i < Floor.Length; i++)
            {
                if (gameObject.transform.position.x < Floor[i].transform.position.x + 9.5 && gameObject.transform.position.x > Floor[i].transform.position.x - 9.5) { 
                    
                    if ((Floor[i].transform.position.y + 2.5f) < gameObject.transform.position.y)
                    {
                        if (distance_floor > gameObject.transform.position.y - Floor[i].transform.position.y)
                        {
                            floor = i;
                            distance_floor = gameObject.transform.position.y - Floor[i].transform.position.y;
                        }
                    }
                    
                }
            }
            if (floor != 150)
                isFloor = true;
        }
        if (isFloor)
        {

        }

        //좌우이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isDown)
            {
                gameObject.transform.localScale = new Vector3(-4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x - 0.05f, position.y + (Velocityg * 0.1f), position.z);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                gameObject.transform.position = new Vector3(position.x - 0.05f, position.y, position.z);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isDown)
            {
                gameObject.transform.localScale = new Vector3(+4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                Velocityg -= G;
                gameObject.transform.position = new Vector3(position.x + 0.05f, position.y + (Velocityg * 0.1f), position.z);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(+4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                gameObject.transform.position = new Vector3(position.x + 0.05f, position.y, position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            isUp = true;
            past_y = gameObject.transform.position.y;
        }
        if (isUp)
        {
            if (jump_y < 16f)
            {
                jump_y += 0.13f;
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    gameObject.transform.localScale = new Vector3(+4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                    gameObject.transform.position = new Vector3(position.x + 0.05f, past_y + jump_y, position.z);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    gameObject.transform.position = new Vector3(position.x - 0.05f, past_y + jump_y, position.z);
                    gameObject.transform.localScale = new Vector3(-4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                }
                else
                {
                    gameObject.transform.position = new Vector3(position.x, past_y + jump_y, position.z);
                }
            }
            else
            {
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
                
                if (gameObject.transform.localScale.x < 0)
                {
                    wave.transform.position = transform.position + new Vector3(-1, 0, 0);
                    wave.transform.parent = null;
                }
                else
                {
                    wave.transform.position = gameObject.transform.position + new Vector3(+1, 0, 0);
                }
            }
        }

        position = gameObject.transform.position;
    }
}
