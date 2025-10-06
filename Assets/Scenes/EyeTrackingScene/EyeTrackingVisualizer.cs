using System;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackingVisualizer : MonoBehaviour
{
    [SerializeField] private int segments = 24;
    [SerializeField] private float maxDistance = 12;
    [SerializeField] private float gazeAngle = 15;

    [SerializeField] private GameObject cube;

    private List<Transform> segmentTransforms = new();

    private void Awake()
    {
        for (var i = 0; i < segments; i++)
            segmentTransforms.Add(Instantiate(cube, transform).transform);
    }

    private void Update()
    {
        var angleStep = 360f / segments;

        for (var i = 0; i < segments; i++)
        {
            var rotation = Quaternion.AngleAxis(i * angleStep, transform.forward);
            var direction = rotation * Quaternion.AngleAxis(gazeAngle, transform.right) * transform.forward;

            segmentTransforms[i].position = transform.position + direction * maxDistance;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        var angleStep = 360f / segments;

        for (var i = 0; i < segments; i++)
        {
            var rotation = Quaternion.AngleAxis(i * angleStep, transform.forward);
            var direction = rotation * Quaternion.AngleAxis(gazeAngle, transform.right) * transform.forward;

            Gizmos.DrawRay(transform.position, direction * maxDistance);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
    }
}
