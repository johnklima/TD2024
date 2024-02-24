using UnityEngine;

public class CrossHairController : MonoBehaviour
{
    public GameObject defaultCrossHair;
    public GameObject interactiveCrossHair;

    private bool isShowingInteractiveCrosshair;

    //crosshair logic
    public void ToggleCrosshair()
    {
        isShowingInteractiveCrosshair = !isShowingInteractiveCrosshair;
        defaultCrossHair.SetActive(!isShowingInteractiveCrosshair);
        interactiveCrossHair.SetActive(isShowingInteractiveCrosshair);
    }
}