using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private CinemachineVirtualCamera cinemachineCam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one CameraShake instance");
        }

        cinemachineCam = GetComponent<CinemachineVirtualCamera>();
    }

    private float totalShakeAmount = 0f;
    private float totalShakeLength = 0f;
    private float shakeTimer;

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin perlin = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = Mathf.Lerp(totalShakeAmount, 0f, 1 - (shakeTimer / totalShakeLength));
            }
        }
    }

    public void Shake(float amount, float length)
    {
        CinemachineBasicMultiChannelPerlin perlin = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = amount;

        totalShakeAmount = amount;
        totalShakeLength = length;
        shakeTimer = length;
    }
}
