using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFireball : MonoBehaviour
{
    float t;        //타이머
    float angle;    //생성 각도

    bool wall_col;  //벽 충돌 유무

    Vector3 Pos;
    Vector3 now;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        Pos = transform.position;   //생성 초기위치
        angle = Random.Range(0, 180);   //바닥 윗 부분 각도에 대해서만 생성 각도 설정

        float x = Mathf.Cos(angle * Mathf.PI / 180.0f) * 8 + Pos.x;
        float y = Mathf.Sin(angle * Mathf.PI / 180.0f) * 8 + Pos.y;

        transform.position = new Vector3(x, y, Pos.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //각도에 따라 발사되는 방향 설정
        if(!wall_col)
        {
            if (angle < 90)
            {
                now = new Vector3(0.1f, 0.1f, 0) * t; //북동쪽
                transform.position += now;
            }
            if (angle == 90)
            {
                now = Vector3.up * 0.1f * t; //북서쪽
                transform.position += now;
            }
            if (angle > 90)
            {
                now = new Vector3(-0.1f, 0.1f, 0) * t;  //북서쪽
                transform.position += now;
            }
        }

        //벽에 부딪힌 경우 튕기기 구현!!

        if (Mathf.Abs(transform.position.x) > 60 || transform.position.y > 69)  //벽에 맞은 경우
        {
            wall_col = true;

        }

        if (wall_col)   //벽에 맞은 경우
        {
            transform.position -= now;
        }    

        t += 0.09f;

    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Wall" || other.tag =="Ground")  //tag 에러 방지! -> 스크립트로 인식?
    //    {
    //        wall_col = true;
    //    }
    //}
}
