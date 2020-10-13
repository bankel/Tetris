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

    public static int height = 20;
    public static int width = 10;

    private float left, right, bottom, up;
    
    
    private static Transform[,] grids = new Transform[width, height];
    // Start is called before the first frame update
    
    void Start()
    {
        var position = transform.position;
        left = - width / 2;
        right =  width / 2;
        bottom = - height / 2;
        up =  height / 2;
        
        print($"left {left} right {right} bottom {bottom}");
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
                AddGrid();
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

    
    
    
    void AddGrid()
    {
        float xOffset = width / 2-0.5f;
        float yOffset = height / 2-0.5f;
        foreach (Transform child in transform)
        {
            int roundX = (int) (child.position.x + xOffset);
            int roundY = (int) (child.position.y + yOffset);
            grids[roundX, roundY] = child;
        }
    }
    
    bool ValidMove()
    {
        int xOffset = width / 2;
        int yOffset = height / 2;
        
        foreach (Transform children in transform)
        {
            var position = children.transform.position;
            
            if (position.x < left || position.x > right || position.y < bottom || position.y > up)
            {
                return false;
            }
            
            int roundX = (int) (children.position.x + xOffset);
            int roundY = (int) (children.position.y + yOffset);
            if (grids[roundX, roundY] != null)
            {
                return false;
            }
            
        }
        return true;
    }
}
