using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemBone : MonoBehaviour
{
    // Start is called before the first frame update
    float t;    //타이머
    // Start is called before the first frame update
    public GameObject imageObj;
    public Sprite myimage;

    AudioSource Item_pick;
    public AudioClip Item_Sound;

    void Start()
    {
        t = 0;
        Item_pick = gameObject.AddComponent<AudioSource>();
        Item_pick.clip = Item_Sound;
        Item_pick.loop = false;
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
            Item_pick.Play();
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("땡이가 먹음");
                Destroy(gameObject);
            }
            //  if (Input.GetKeyDown(KeyCode.Z))
            {
                //Debug.Log("땡이가 먹음");
                //imageObj = GameObject.FindWithTag("Q");
                //imageObj.GetComponent<SpriteRenderer>().sprite = myimage;

                //Destroy(gameObject);
            }
            //Move DD = GameObject.Find("DDaeng").GetComponent<Move>();
            //if(DD.eatFlag)    //플레이어가 아이템을 습득한 경우
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
            if (Input.GetKeyDown(KeyCode.Z))
            {
                
                Debug.Log("땡이가 먹음");
                Destroy(gameObject);
            }
            //Move DD = GameObject.Find("DDaeng").GetComponent<Move>();
            //if(DD.eatFlag)    //플레이어가 아이템을 습득한 경우
            //{
            //    DD.eatFlag = false; //다시 초기화
            //    Debug.Log("땡이가 먹음");
            //    Destroy(gameObject);
            //}
        }
    }
}
