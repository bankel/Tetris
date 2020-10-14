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

    private float xOffset = width / 2 -0.4f;
    private float yOffset = height / 2 - 0.4f;
    void Start()
    {
        var position = transform.position;
        left = - width / 2;
        right =  width / 2;
        bottom = - height / 2;
        up =  height / 2;
        
        print($"left {left} right {right} bottom {bottom} xOffset{xOffset}, yOffset{yOffset}");
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
                CheckFull();
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


    void CheckFull()
    {
        for (int i = height -1; i >=0; i--)
        {
            if (HasFullLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasFullLine(int row)
    {
        for (int i = 0; i < width; i++)
        {
            if (grids[i, row] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int row)
    {
        for (int i = 0; i < width; i++)
        {
            Destroy(grids[i, row].gameObject);
            grids[i, width] = null;
        }
    }

    void RowDown(int row)
    {
        for(int i = row +1; i< height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grids[j, i] != null)
                {
                    grids[j, i - 1] = grids[j, i];
                    grids[j, i] = null;
                    grids[j, i -1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
    
    
    
    void AddGrid()
    {
        foreach (Transform child in transform)
        {
            int roundX = (int) (child.position.x + xOffset);
            int roundY = (int) (child.position.y + yOffset);
            grids[roundX, roundY] = child;
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
