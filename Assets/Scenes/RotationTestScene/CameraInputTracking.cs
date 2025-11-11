using System;
using UnityEngine;

public class CameraInputTracking : MonoBehaviour
{
    public static CameraInputTracking instance;

    public Vector3 simulatedInputData;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        SetLocalTransform(Vector3.zero, Quaternion.Euler(simulatedInputData));
    }

    private void SetLocalTransform(Vector3 newPosition, Quaternion newRotation)
    {
        transform.localPosition = newPosition;
        transform.localRotation = CameraAimOffset.instance.rotationOffset * newRotation;
    }
}
