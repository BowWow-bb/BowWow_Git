using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bone : MonoBehaviour
{
    bool left;
    GameObject DD;
    Vector3 position;
    float check;
    float rotate;
    // Start is called before the first frame update
    void Start()
    {
        //-45 45
        rotate = 0;
        check = gameObject.transform.position.x;
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
        check += 0.1f;
        rotate += 10f;
        if (left && gameObject.transform.position.x > -45f)
            gameObject.transform.position = new Vector3(check, position.y + (6f) * Mathf.Sin(3.14f * (check - position.x) / (46 - position.x)), 0);
        else if (!left && gameObject.transform.position.x < 45f)
            gameObject.transform.position = new Vector3(check, position.y + (6f)*Mathf.Sin(3.14f * (check-position.x)/(46-position.x)),0);
        else
        {
            Destroy(gameObject, 0);
        }
        Debug.Log(gameObject.transform.position.y);

        gameObject.transform.rotation = Quaternion.Euler(rotate, rotate, 0);
    }
}
