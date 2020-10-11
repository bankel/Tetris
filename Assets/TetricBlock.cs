using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TetricBlock : MonoBehaviour
{
    public Vector3 rotatePivot;
    private float previousTime;

    public float fallInterval = 0.8f;

    public static float height = 20;
    public static float width = 10;

    private float left, right, bottom, up;
    // Start is called before the first frame update
    
    void Start()
    {
        var position = transform.position;
        left = - width / 2;
        right =  width / 2;
        bottom = - height / 2;
        up =  height / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0); 
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);    
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1,0,0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1,0,0);
            }

        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallInterval / 10f : fallInterval))
        {
            transform.position += new Vector3(0,-1,0);
            
            if (!ValidMove())
            {
                transform.position -= new Vector3(0,-1,0);
                this.enabled = false;
                FindObjectOfType<TetrisSpawner>().NewTetris();
            }
            
            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotatePivot), new Vector3(0,0,-1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotatePivot), new Vector3(0,0,-1), -90);
            }
        }
    }
    
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            var position = children.transform.position;
            
            if (position.x < left || position.x > right || position.y < bottom || position.y > up)
            {
                return false;
            }
        }
        return true;
    }
}
