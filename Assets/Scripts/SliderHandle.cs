using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderHandle : MonoBehaviour
{

    Slider mySlider;
    
    // Start is called before the first frame update
    void Start()
    {
        mySlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getSlderValue()
    { 
        Debug.Log(mySlider.value);
    }

}
