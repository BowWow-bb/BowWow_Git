using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBballMove : MonoBehaviour
{
    public GameObject bt;       //빅톨 가져오기
    public GameObject player;   //플레이어 가져오기
    public GameObject mini;     //미니파이어볼 가져오기

    float rot_v = 1000.0f;      //회전 속도
    float dis_v = 0.5f;         //이동 속도
    float dis_std;              //일정 이동거리
    float dis_tmp;              //이동 거리 확인 (일정 이동거리 도달 여부)

    bool mini_flag;             //미니 파이어볼 생성 여부 true: 생성완료, false: 생성미완

    // Start is called before the first frame update
    void Start()
    {
        dis_std = 20.0f;
        dis_tmp = 0.0f;
        mini_flag = false;
    }

    // Update is called once per frame 
    void Update()
    {

        if (dis_tmp < dis_std)  //일정거리 이동 못한 경우
        {
            transform.Rotate(Vector3.down * rot_v * Time.deltaTime);    //반시계 방향으로 속도만큼 회전
            if(player.transform.position.x < transform.position.x)  //플레이어가 빅톨의 왼쪽에 위치
            {
                transform.position = new Vector3(transform.position.x - dis_std * Time.deltaTime*dis_v, transform.position.y, transform.position.z);  //시간에 따른 이동
                dis_tmp += dis_std * Time.deltaTime * dis_v;
            }
            else //플레이어가 빅톨의 오른쪽에 위치
            {
                transform.position = new Vector3(transform.position.x + dis_std * Time.deltaTime * dis_v, transform.position.y, transform.position.z);  //시간에 따른 이동
                dis_tmp += dis_std * Time.deltaTime;
            }
        }
        else if(!mini_flag) //이동 완료
        {
            for(int i=0; i<10; i++)
            {
                GameObject ball = GameObject.Instantiate(mini); //미니 파이어볼 n개 생성
                ball.transform.position = transform.position;   //미니 파이어볼 초기 위치 = 빅 파이어볼 현재 위치
                                                                //ball.transform.LookAt(??); 360도를 10으로 나눠서 각 방향을 바라보게 설정하기

                //float tmp_x = Random.Range(-10.0f, 10.0f);   //-10.0 ~ 10.0 랜덤 난수 발생
                //float tmp_y = Random.Range(-10.0f, 10.0f);   //-10.0 ~ 10.0 랜덤 난수 발생
                //Vector3 dir = new Vector3(ball.transform.position.x + tmp_x, ball.transform.position.y + tmp_y, ball.transform.position.z);
                //ball.transform.LookAt(dir);
            }
            mini_flag = true;  //미니 파이어볼 생성 완료
        }

        //파이어볼은 미니파이어볼 생성 이후 사라지나..?
    }
}
