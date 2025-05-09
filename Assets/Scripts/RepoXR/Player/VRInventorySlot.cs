using System;
using System.Collections.Generic;
using UnityEngine;

namespace RepoXR.Player
{
    public class VRInventorySlot : MonoBehaviour
    {
        public int slotIndex = -1;
        public LineRenderer lineRenderer;
        public Collider collider;

        private void Update()
        {
            if (!lineRenderer)
                return;
            
            var points = new List<Vector3>
            {
                transform.TransformPoint(new Vector3(-0.045f, 0, -0.07f)),
                transform.TransformPoint(new Vector3(0.045f, 0, -0.07f)),
                transform.TransformPoint(new Vector3(0.045f, 0, 0.07f)),
                transform.TransformPoint(new Vector3(-0.045f, 0, 0.07f))
            };

            lineRenderer.positionCount = 4;
            lineRenderer.SetPositions(points.ToArray());
            lineRenderer.material.mainTextureOffset += Vector2.left * (Time.deltaTime * 2f);
        }
    }
}