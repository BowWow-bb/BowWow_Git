using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    GameObject Ground;
    public GameObject SoundWave = null;
    Vector3 position;
    float G; // 중력 가속도
    float Velocityg; // 떨어지는 속도
    float BallPower; // 탄성에 의해 적용된 힘
    float TimeScale;//타임 스케일 조정
    float jump_y;
    float past_y;
    bool isGround;
    bool isUp;
    // Start is called before the first frame update
    void Start()
    {
        Ground = GameObject.FindWithTag("Ground");
        TimeScale = 10000.0f;
        BallPower = 0;
        G = 9.8f / TimeScale;
        Velocityg = 0;
        position = gameObject.transform.position;
        position.y = 5;
        isUp = false;
        jump_y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y - Ground.transform.position.y > 3.2f)
        {
            Debug.Log("중더학;");
            Velocityg -= G;
            gameObject.transform.position = new Vector3(position.x, position.y+(Velocityg*0.1f), position.z);
        }
        else if(gameObject.transform.position.y < 3.2f)
        {
            gameObject.transform.position = new Vector3(position.x, 3.2f, position.z);
        }
        else
        {
            
            Velocityg = 0f;
        }
        //좌우이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isUp)
            {
                if (jump_y < 10f)
                {
                    jump_y += 0.1f;
                    gameObject.transform.position = new Vector3(position.x - 0.05f, past_y + jump_y, position.z);
                    gameObject.transform.localScale = new Vector3(-4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                }
                else
                {
                    isUp = false;
                    jump_y = 0;
                }
            }
            else
            {
                gameObject.transform.localScale = new Vector3(-4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                gameObject.transform.position = new Vector3(position.x - 0.05f, position.y, position.z);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.localScale = new Vector3(+4, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            gameObject.transform.position = new Vector3(position.x + 0.05f, position.y, position.z);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            isUp = true;
            past_y = gameObject.transform.position.y;
        }
        if(isUp)
        {
            if (jump_y < 10f)
            {
                jump_y += 0.1f;
                gameObject.transform.position = new Vector3(position.x, past_y + jump_y, position.z);
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
            if(SoundWave !=null)
            {
                GameObject wave = GameObject.Instantiate(SoundWave);
                if(gameObject.transform.localScale.x < 0)
                {
                    wave.transform.position = gameObject.transform.position + new Vector3(-1,0,0);

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
