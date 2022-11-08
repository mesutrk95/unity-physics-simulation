using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    int getUtcNow()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
        return cur_time;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("[update] frame : " + Time.frameCount  );
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("[fixed update] frames : " + Time.frameCount);
    }
}
