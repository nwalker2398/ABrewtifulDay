using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed = 1.0f;
    public float y_start = 1.4f;
    public float y_range = 0.5f;
  
    void Update()
    {
        float y = calcY();
        var curr_pos = transform.position;
        transform.position = new Vector3(curr_pos.x, y, curr_pos.z);
    }

    public float calcY() 
    {
        return Mathf.PingPong(Time.time * speed, 1) * y_range + y_start;
    } 
    public void set_y(float f)
    {
        y_start = f;
    }
}
