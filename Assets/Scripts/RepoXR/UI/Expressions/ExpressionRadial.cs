using UnityEngine;
using UnityEngine.UI;

namespace RepoXR.UI.Expressions
{
    public class ExpressionRadial : MonoBehaviour
    {
        public Transform handTransform;

        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private Transform canvasTransform;
        [SerializeField] private Transform previewTransform;
        [SerializeField] private Image previewBackground;
        [SerializeField] private ExpressionPart[] parts;

        private void Update()
        {
            var centerToHand = handTransform.position - canvasTransform.position;
            var projected = Vector3.ProjectOnPlane(centerToHand, canvasTransform.forward);
            var angle = Vector3.SignedAngle(canvasTransform.up, projected, -canvasTransform.forward);

            if (angle < 0)
                angle += 360;

            var part = (int)angle * parts.Length / 360;

            for (var i = 0; i < parts.Length; i++)
                parts[i].SetHovered(i == part);
        }
    }
}