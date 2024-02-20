using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WumpusData : MonoBehaviour
{

    public int cellContents = 0; //who or what is in the cell
    public int row; //row located in grid
    public int col; //column located in grid

    public bool exposed = false;

    [SerializeField] Material[] mats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Activate(bool isExposed)
    {
        
        if(isExposed)
        {
            exposed = true;

            //set its contents
            SetVisualContents();
        }
        

    }
    private void SetVisualContents()
    {
        //Based on the index of the contents in data, we choose a material
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = mats[cellContents];   
    }
}
