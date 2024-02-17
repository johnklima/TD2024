using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CauldronController : MonoBehaviour, IInteractable
{
    public CinemachineVirtualCamera cauldronCamera;
    public CinemachineVirtualCamera mainCamera;
    public GameObject cauldronUI;

    public UnityEvent onCauldronEnter;
    public UnityEvent onCauldronExit;
    
    public void Interact()
    {
       
        if (cauldronCamera && cauldronUI)
        {
            cauldronCamera.Priority = 20;
            cauldronUI.SetActive(true);
            onCauldronEnter.Invoke();
            
            Debug.Log(mainCamera.Priority);
        }
    }

    private void Update()
    {
        Debug.Log(mainCamera);
        Debug.Log(cauldronCamera);

    }

    public void Interact(LeifPlayerController lPC)
    {
        throw new System.NotImplementedException();
    }

    public void CancelCauldronMode()
    {
        cauldronCamera.Priority = 5;
        
        cauldronUI.SetActive(false);
        onCauldronExit.Invoke();
    }

}
