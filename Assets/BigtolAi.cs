using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigtolAi : MonoBehaviour
{
    float hp;               //HP
    static float hp_std;    //HP 손상 기준

    float dis_std;          //일정 이동거리
    float dis_v;            //이동 속도
    float dis_tmp;          //이동 거리 확인 (일정 이동거리 도달 여부)

    Vector3 dir;            //이동 벡터
    int dir_ran;            //랜덤 이동방향 0:왼쪽, 1:오른쪽

    // Start is called before the first frame update
    void Start()
    {
        Init(); //초기 설정값 
    }

    // Update is called once per frame
    void Update()
    {
        if (hp == 100.0f) //손상 없는 경우
        {
            //좌우 랜덤으로 일정거리 이동
            dir_ran = Random.Range(0, 2);   //0 또는 1을 랜덤으로 반환

            if (dir_ran == 0) //왼쪽 이동
            {
                while (dis_tmp < dis_std)
                {
                    dir = new Vector3(dis_std, 0, 0);
                    transform.position -= dir * dis_v * Time.deltaTime;
                    dis_tmp -= dis_std * dis_v * Time.deltaTime;
                }
                dis_tmp = 0.0f;

            }
            else //오른쪽 이동
            {
                while (dis_tmp < dis_std)
                {
                    dir = new Vector3(dis_std, 0, 0);
                    transform.position += dir * dis_v * Time.deltaTime;
                    dis_tmp += dis_std * dis_v * Time.deltaTime;
                }
                dis_tmp = 0.0f;
            }
        }
    }

    void Init()
    {
        hp = 100.0f;
        hp_std = 55.0f;
        dis_std = 5.0f;
        dis_v = 0.9f;
        dis_tmp = 0.0f;
    }
}
