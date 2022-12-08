using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[System.Serializable]
public class CameraShake
{
    [SerializeField] float shakeIntensity = 5f, shakeTiming = 0.5f;
    CinemachineBasicMultiChannelPerlin noise;
    CinemachineVirtualCamera cmFreeCam => CameraManager.Instance.CinemachineCamera;

    public void Initialize()
    {
        noise = cmFreeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        cmFreeCam.StartCoroutine(ProcessShake());
    }

    IEnumerator ProcessShake()
    {
        Noise(shakeIntensity);
        yield return new WaitForSeconds(shakeTiming);
        Noise(0);
    }

    void Noise(float amplitudeGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
    }
}

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;

    public CinemachineVirtualCamera CinemachineCamera => cinemachineCamera; 

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
}
