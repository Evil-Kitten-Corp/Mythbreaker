using System;
using BrunoMikoski.ServicesLocation;
using Cinemachine;
using UnityEngine;

public class CinemachineController : MonoBehaviour 
{
    public CinemachineVirtualCamera[] cameraList;
    private int _currentCamera = 0;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterInstance(this);
    }

    public void ChangeCam() 
    {
        _currentCamera++;
        
        if (_currentCamera < cameraList.Length)
        {
            cameraList[_currentCamera - 1].gameObject.SetActive(false);
            cameraList[_currentCamera].gameObject.SetActive(true);
        }
        else 
        {
            cameraList[_currentCamera - 1].gameObject.SetActive(false);
            _currentCamera = 0;
            cameraList[_currentCamera].gameObject.SetActive(true);
        }
    }
}