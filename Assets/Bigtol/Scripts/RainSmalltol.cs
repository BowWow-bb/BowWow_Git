using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSmalltol : MonoBehaviour
{
    float x_min;    //생성 x좌표 최소
    float x_max;    //생성 x좌표 최대

    float height;   //생성 높이

    bool up_flag;   //튀어오를지 유무
    float t;        //타이머

    // Start is called before the first frame update
    void Start()
    {
        height = Random.Range(30.0f, 45.0f);  //떨어지는 높이 설정
        up_flag = false;
        t = 0;

        //초기생성위치 = 플레이어 위치 로 설정되있음
        x_min = transform.position.x - 20.0f; //플레이어 +- 거리
        if (x_min < -43.0f) //왼쪽 벽 경계 제한
            x_min = -43.0f;

        x_max = transform.position.x + 20.0f;
        if (x_max > 43.0f) //오른쪽 벽 경계 제한
            x_max = 43.0f;

        float x_rand = Random.Range(x_min, x_max);  //생성지점 결정
        transform.position = new Vector3(x_rand, transform.position.y+height, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject Player = GameObject.Find("DDaeng");

        Vector3 pos = transform.position;   //현재 위치

        //충돌한 경우 - 튕겨야 함.
        if (up_flag)
        {
            pos.y += t;
            transform.position = pos;

            Destroy(gameObject, 0.3f);
        }

        else
        {
            pos.y -= t;

            transform.position = pos;

            //바닥과 충돌한 경우
            if (pos.y < Player.transform.position.y)
            {
                pos.y = Player.transform.position.y;
                transform.position = pos;

                //튕기며 뿅 사라짐
                up_flag = true;
                //Destroy(transform, 0.001f);
            }

            //플레이어와 충돌한 경우
            if (Mathf.Abs(pos.x - Player.transform.position.x) < 0.001f && Mathf.Abs(pos.y - (Player.transform.position.y + 4)) < 0.001f)
            {
                //튕기며 뿅 사라짐
                up_flag = true;
            }
        }

        t += 0.01f;
    }
}
