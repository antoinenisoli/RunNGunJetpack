using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
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
    public CinemachineCamera CinemachineCamera => cinemachineCamera;
    public static CameraManager Instance;
    public Camera Camera;
    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] float endShakeSpeed = 10f;

    [Header("Offset camera")]
    [SerializeField] float borderSize = 200f;
    [SerializeField] float offsetAmount = 3f;

    CinemachineBasicMultiChannelPerlin noise;
    CinemachinePositionComposer framingTransposer;

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        Camera = GetComponent<Camera>();
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        noise = cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
        framingTransposer = cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachinePositionComposer;
    }

    public Vector2 MousePosition()
    {
        return Camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Start()
    {
        GetPlayer();
    }

    void GetPlayer()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
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
        noise.AmplitudeGain = amplitudeGain;
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
            framingTransposer.TargetOffset = offset.normalized * offsetAmount;
        }
        else
            framingTransposer.TargetOffset = Vector2.zero;
    }

    void LateUpdate()
    {
        noise.AmplitudeGain = Mathf.Lerp(noise.AmplitudeGain, 0, endShakeSpeed * Time.deltaTime);
    }
}
