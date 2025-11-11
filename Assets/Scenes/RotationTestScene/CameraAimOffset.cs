using UnityEngine;

public class CameraAimOffset : MonoBehaviour
{
    public static CameraAimOffset instance;

    [SerializeField] private Transform debugTransform1;
    [SerializeField] private Transform debugTransform2;
    [SerializeField] private Transform debugTransform3;

    public Transform targetTransform;
    public bool overrideAim;

    public Quaternion rotationOffset;
    private Quaternion yawOffset;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (overrideAim)
        {
            var desiredWorldRotation =
                Quaternion.LookRotation(targetTransform.position - CameraInputTracking.instance.transform.position,
                    Vector3.up);
            var offsetRotation = desiredWorldRotation *
                                 Quaternion.Inverse(Quaternion.Euler(CameraInputTracking.instance.simulatedInputData));
            var desiredForward = offsetRotation * Vector3.forward;
            var flatDirection = new Vector3(desiredForward.x, 0, desiredForward.z).normalized;

            var qY = Quaternion.LookRotation(flatDirection, Vector3.up);
            var qXZ = Quaternion.Inverse(qY) * offsetRotation;

            debugTransform1.rotation = desiredWorldRotation;
            debugTransform2.rotation = qY;
            debugTransform3.rotation = qXZ;

            rotationOffset = Quaternion.Lerp(rotationOffset, qXZ, 3 * Time.deltaTime);
            yawOffset = Quaternion.Lerp(yawOffset, qY, 3 * Time.deltaTime);
        }
        else
            rotationOffset = Quaternion.LerpUnclamped(rotationOffset, Quaternion.identity, 5 * Time.deltaTime);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, yawOffset, 33 * Time.deltaTime);
    }
}
