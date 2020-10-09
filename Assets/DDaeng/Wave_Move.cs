using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Move : MonoBehaviour
{
    GameObject DD;
    bool left;
    Vector3 position;
    float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
        DD = GameObject.Find("DDaeng");
        if (DD.transform.position.x > position.x) // 음파가 왼쪽일때
            left = true;
        else
            left = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(DD.transform.position.x - gameObject.transform.position.x) > 30)
        {
            Destroy(gameObject);
        }
        gameObject.transform.localRotation = Quaternion.Euler(time, time, 0);
        if (left) // 음파가 왼쪽일때
        {
            gameObject.transform.position = new Vector3(position.x - 0.05f, position.y, position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(position.x + 0.05f, position.y, position.z);
        }
        position = gameObject.transform.position;
        time += 3f;
    }
}
