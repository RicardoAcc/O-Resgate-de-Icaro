using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera virtualCamera;
    private CameraController instance;
    
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        SetPlayerAsTarget();
    }

    private void SetPlayerAsTarget()
    {
        if (JogadorScript.instance != null)
        {
            virtualCamera.Follow = JogadorScript.instance.transform;
            virtualCamera.LookAt = JogadorScript.instance.transform;
        }
    }
}
