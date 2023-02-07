using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[System.Serializable]
public class CameraShake
{
    [SerializeField] float shakeIntensity = 5f;
    CameraManager cmFreeCam => CameraManager.Instance;

    public void Shake()
    {
        cmFreeCam.Noise(shakeIntensity);
    }
}

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] float endShakeSpeed = 10f;
    CinemachineBasicMultiChannelPerlin noise;
    public CinemachineVirtualCamera CinemachineCamera => cinemachineCamera;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        noise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Noise(float amplitudeGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
    }

    void LateUpdate()
    {
        noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, 0, endShakeSpeed * Time.deltaTime);
    }
}
