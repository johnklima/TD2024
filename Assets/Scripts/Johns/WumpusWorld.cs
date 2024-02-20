using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WumpusWorld : MonoBehaviour
{

    const int ROWS = 4;
    const int COLS = 4;


    //should do a typedef enum but rather I shall do it in simplest terms
    public const int EMPTY = 0;
    public const int BREEZE = 1;
    public const int STENCH = 2;
    public const int TREASURE = 3;
    public const int PIT = 4;
    public const int WUMPUS = 5;
    public const int AGENT = 6;


    public GameObject baseobject;
    GameObject[,] objs = new GameObject[ROWS, COLS];

    // Start is called before the first frame update
    void Start()
    {
        Build();
        StartGame();
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

                // get the data object (script component) of the game object 
                WumpusData data = objs[row, col].GetComponent<WumpusData>();
                data.row = row;
                data.col = col;

                //set up this cell
                ApplyInitialState(objs[row, col]);


            }
        }
    }
    
    //apply our creation rules to this cell
    void ApplyInitialState(GameObject thisobj)
    {
        thisobj.SetActive(true);

        // get the data object (script component) of the game object 
        WumpusData data = thisobj.GetComponent<WumpusData>();

        //apply hard rules as to where things are, based on the classic demo
        // row 0 (1)
        if (data.row == 0 && data.col == 0)
        {
            data.cellContents = WumpusWorld.AGENT;
            return;
        }
        if (data.row == 0 && data.col == 1)
        {
            data.cellContents = WumpusWorld.BREEZE;
            return;
        }
        if (data.row == 0 && data.col == 2)
        {
            data.cellContents = WumpusWorld.PIT;
            return;
        }
        if (data.row == 0 && data.col == 3)
        {
            data.cellContents = WumpusWorld.BREEZE;
            return;
        }

        // row 1 (2)
        if (data.row == 1 && data.col == 0)
        {
            data.cellContents = WumpusWorld.STENCH;
            return;
        }
        if (data.row == 1 && data.col == 1)
        {
            data.cellContents = WumpusWorld.EMPTY;
            return;
        }
        if (data.row == 1 && data.col == 2)
        {
            data.cellContents = WumpusWorld.BREEZE;
            return;
        }
        if (data.row == 0 && data.col == 3)
        {
            data.cellContents = WumpusWorld.EMPTY;
            return;
        }

        // row 2 (3)
        if (data.row == 2 && data.col == 0)
        {
            data.cellContents = WumpusWorld.WUMPUS;
            return;
        }
        if (data.row == 2 && data.col == 1)
        {
            data.cellContents = WumpusWorld.TREASURE;
            return;
        }
        if (data.row == 2 && data.col == 2)
        {
            data.cellContents = WumpusWorld.PIT;
            return;
        }
        if (data.row == 2 && data.col == 3)
        {
            data.cellContents = WumpusWorld.BREEZE;
            return;
        }

        // row 3 (4)
        if (data.row == 3 && data.col == 0)
        {
            data.cellContents = WumpusWorld.STENCH;
            return;
        }
        if (data.row == 3 && data.col == 1)
        {
            data.cellContents = WumpusWorld.EMPTY;
            return;
        }
        if (data.row == 3 && data.col == 2)
        {
            data.cellContents = WumpusWorld.BREEZE;
            return;
        }
        if (data.row == 3 && data.col == 3)
        {
            data.cellContents = WumpusWorld.PIT;
            return;
        }



    }

    void StartGame()
    {

        //Activate player
        objs[0, 0].GetComponent<WumpusData>().Expose(true);


        //if we want to debug
        if (true)
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    objs[row, col].GetComponent<WumpusData>().Expose(true);

                }
            }

        }

    }
}
