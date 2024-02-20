using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WumpusWorld : MonoBehaviour
{

    const int ROWS = 4;
    const int COLS = 4;

    public GameObject baseobject;
    GameObject[,] objs = new GameObject[ROWS, COLS];

    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Build()
    {
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                objs[row, col] = GameObject.Instantiate(baseobject, transform);
                float space = baseobject.transform.localScale.x;
                Vector3 pos = new Vector3(row * space, 0.5f, col * space);

                objs[row, col].transform.localPosition = pos;


            }
        }
    }

    void StartGame()
    {


    }
}
