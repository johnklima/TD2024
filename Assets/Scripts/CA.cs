using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA : MonoBehaviour
{
    // visualize cells
    public GameObject baseObject;
    GameObject[] cellObjs;

    int[] cells;

    int width = 32;
    int generation = 0;

    public int[] ruleset = { 0, 0, 0, 1, 1, 1, 1, 0 };  //rule 30

    float timer = 0;

    public int TheRule = 30;
    public int TheBinary = 0;

    // Start is called before the first frame update
    void Start()
    {
        cellObjs = new GameObject[width];
        cells = new int[width];

        InstantiateGeneration();
        generation = 1;

        cellObjs[width / 2].SetActive(true);
        cells[width / 2] = 1;

        timer = Time.time;

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - timer > 0.1f)
        {
            //lets keep it square ish
            if (generation < width )
            {
                generate();
            }
            
            timer = Time.time;
        }

        if(Input.GetKeyDown(KeyCode.R)) 
        {

            Reset();


        }
            
    }
    private void Reset()
    {


        //delete old stuff
        foreach (Transform obj in transform)
        {
            Destroy(obj.gameObject);
        }

        cellObjs = new GameObject[width];
        cells = new int[width];

        //create the first generation
        generation = 0;
        InstantiateGeneration();
        generation = 1;

        //start with the center cellObj/cell alive
        cellObjs[width / 2].SetActive(true);
        cells[width / 2] = 1;

        timer = Time.time;



        TheBinary = decimal_to_binary(TheRule);

        //clear the array
        for (int i = 0; i < 8; i++)
        {
            ruleset[i] = 0;
        }
        string B = TheBinary.ToString();

        char[] C = B.ToCharArray();
        int d = 7;
        for (int i = C.Length - 1; i > -1; i--)
        {
            ruleset[d] = (int)char.GetNumericValue(C[i]);
            d--;
        }


    }
    void generate() 
    {
        //create the next row of cellObjs
        InstantiateGeneration();

        // First we create an empty array for the new values
        int[] nextgen = new int[width];

        // Ignore edges that only have one neighor
        for (int i = 1; i < width - 1; i++)
        {
            int left = cells[i - 1];    // Left neighbor state
            int me = cells[i];          // Current state
            int right = cells[i + 1];   // Right neighbor state
            nextgen[i] = Rules(left, me, right); // Compute next generation state based on ruleset
        }

        // The current generation is the new generation
        cells = nextgen;

        for (int i = 0; i < width ; i++)
        {
            if (cells[i] == 1)
                cellObjs[i].SetActive(true);
            else
                cellObjs[i].SetActive(false);
        }

        generation++;

    }

    private int Rules(int a, int b, int c)
    {
        if (a == 1 && b == 1 && c == 1) return ruleset[0];
        if (a == 1 && b == 1 && c == 0) return ruleset[1];
        if (a == 1 && b == 0 && c == 1) return ruleset[2];
        if (a == 1 && b == 0 && c == 0) return ruleset[3];
        if (a == 0 && b == 1 && c == 1) return ruleset[4];
        if (a == 0 && b == 1 && c == 0) return ruleset[5];
        if (a == 0 && b == 0 && c == 1) return ruleset[6];
        if (a == 0 && b == 0 && c == 0) return ruleset[7];


        return 0;
    }

    void InstantiateGeneration()
    {
        for (int i = 0; i < width; i++) 
        {
            cellObjs[i] = Instantiate(baseObject);
            cellObjs[i].transform.parent = transform;
            cellObjs[i].transform.localPosition = Vector3.right * i + Vector3.forward * generation;
            cellObjs[i].SetActive(false);

        }
    }

    int decimal_to_binary(int n)
    {
        int binary = 0;
        int remainder, i, flag = 1;
        for (i = 1; n != 0; i = i * 10)
        {
            remainder = n % 2;
            n /= 2;
            binary += remainder * i;
        }
        return binary;
    }

}
