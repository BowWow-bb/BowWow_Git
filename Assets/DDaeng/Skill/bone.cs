using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bone : MonoBehaviour
{
    bool left;
    GameObject DD;
    Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        //-45 45
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
        if (left && gameObject.transform.position.x > -45f)
            gameObject.transform.position += new Vector3(-0.2f, 0, 0);
        else if (!left && gameObject.transform.position.x < 45f)
            gameObject.transform.position += new Vector3(0.2f, 0, 0);
        else
            Destroy(gameObject, 0);
    }
}
