using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("Confiner")]
    [SerializeField] private CinemachineConfiner2D confiner;

    private CinemachineCamera virtualCamera;

    private void Awake()
    {
        instance = this;

        virtualCamera = GetComponent<CinemachineCamera>();

        if (confiner == null)
            confiner = GetComponent<CinemachineConfiner2D>();
    }

    public void InitializeForScene(Transform player, Collider2D cameraBounds)
    {
        if (player == null || virtualCamera == null)
            return;

        Physics2D.SyncTransforms();

        if (confiner != null && cameraBounds != null)
        {
            confiner.BoundingShape2D = cameraBounds;

            confiner.InvalidateBoundingShapeCache();
            confiner.InvalidateLensCache();
        }

        virtualCamera.Follow = player;
        virtualCamera.LookAt = player;

        virtualCamera.PreviousStateIsValid = false;

        Vector3 targetPosition = player.position;
        targetPosition.z = transform.position.z;

        transform.position = targetPosition;

        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 mainCameraPosition = player.position;
            mainCameraPosition.z = mainCamera.transform.position.z;

            mainCamera.transform.position = mainCameraPosition;
        }

        if (confiner != null)
        {
            var vcamBase = virtualCamera as CinemachineVirtualCameraBase;

            if (vcamBase != null)
            {
                confiner.BakeBoundingShape(vcamBase, 1f);
            }
        }

        virtualCamera.PreviousStateIsValid = false;
    }
}