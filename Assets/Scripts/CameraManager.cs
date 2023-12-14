using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[System.Serializable]
public class CameraShake
{
    [SerializeField] bool active = true;
    [SerializeField] float shakeIntensity = 5f;
    CameraManager cmFreeCam => CameraManager.Instance;

    public void Shake()
    {
        if (active)
            cmFreeCam.Noise(shakeIntensity);
    }
}

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera CinemachineCamera => cinemachineCamera;
    public static CameraManager Instance;
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] float endShakeSpeed = 10f;

    [Header("Offset camera")]
    [SerializeField] float borderSize = 200f;
    [SerializeField] float offsetAmount = 3f;

    CinemachineBasicMultiChannelPerlin noise;
    CinemachineFramingTransposer framingTransposer;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        noise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        framingTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start()
    {
        GetPlayer();
    }

    void GetPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        cinemachineCamera.Follow = player.transform;
        cinemachineCamera.Follow = player.transform;
    }

    public void Noise(float amplitudeGain)
    {
        StartCoroutine(DelayedNoise(amplitudeGain));
    }

    IEnumerator DelayedNoise(float amplitudeGain)
    {
        yield return new WaitForEndOfFrame();
        noise.m_AmplitudeGain = amplitudeGain;
    }

    private void Update()
    {
        ManageOffset();
    }

    void ManageOffset()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        bool canOffset = Input.mousePosition.y > screenHeight - borderSize
            || Input.mousePosition.y < borderSize
            || Input.mousePosition.x > screenWidth - borderSize
            || Input.mousePosition.x < borderSize
            ;

        if (canOffset)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 offset = mousePosition - (Vector2)framingTransposer.FollowTarget.position;
            framingTransposer.m_TrackedObjectOffset = offset.normalized * offsetAmount;
        }
        else
            framingTransposer.m_TrackedObjectOffset = Vector2.zero;
    }

    void LateUpdate()
    {
        noise.m_AmplitudeGain = Mathf.Lerp(noise.m_AmplitudeGain, 0, endShakeSpeed * Time.deltaTime);
    }
}
