using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBigbo : MonoBehaviour
{
    float t;    //타이머
    // Start is called before the first frame update
    void Start()
    {
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //일정시간 경과 후 아이템 소멸
        if (t >= 100.0f)
            Destroy(gameObject);

        t += 0.1f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //땡이와 충돌한 경우
        {
            Debug.Log("땡이와 충돌!");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("땡이가 먹음");
                Destroy(gameObject);
            }
            //Move DD = GameObject.Find("DDaeng").GetComponent<Move>();
            //if (DD.eatFlag)    //플레이어가 아이템을 습득한 경우
            //{
            //    DD.eatFlag = false; //다시 초기화
            //    Debug.Log("땡이가 먹음");
            //    Destroy(gameObject);
            //}
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null)  //땡이와 충돌한 경우
        {
            Debug.Log("땡이와 충돌!");
            if(Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("땡이가 먹음");
                Destroy(gameObject);
            }
            //Move DD = GameObject.Find("DDaeng").GetComponent<Move>();
            //if (DD.eatFlag)    //플레이어가 아이템을 습득한 경우
            //{
            //    DD.eatFlag = false; //다시 초기화
            //    Debug.Log("땡이가 먹음");
            //    Destroy(gameObject);
            //}
        }
    }
}
