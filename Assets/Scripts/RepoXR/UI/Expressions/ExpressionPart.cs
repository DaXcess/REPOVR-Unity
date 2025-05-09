using UnityEngine;
using UnityEngine.UI;

namespace RepoXR.UI.Expressions
{
    public class ExpressionPart : MonoBehaviour
    {
        public Expression expression;
        
        public Color defaultColor;
        public Color hoverColor;
        public Color activeColor;

        public Color textDefaultColor;
        public Color textActiveColor;

        public AnimationCurve triggerAnimation;
        
        private Image background;

        private bool isHovered;
        
        private void Awake()
        {
            background = GetComponent<Image>();
        }

        private void Update()
        {
            if (isHovered)
            {
                background.color = Color.Lerp(background.color,
                    new Color(background.color.r, background.color.g, background.color.b, 1f / 255 * 150),
                    8 * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, 1.1f * Vector3.one, 8 * Time.deltaTime);
            }
            else
            {
                background.color = Color.Lerp(background.color,
                    new Color(background.color.r, background.color.g, background.color.b, 1f / 255 * 50),
                    8 * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 8 * Time.deltaTime);
            }
        }

        public void SetHovered(bool hovered)
        {
            isHovered = hovered;
        }

        public enum Expression
        {
            Angry,
            Sad,
            Suspicious,
            EyesClosed,
            Crazy,
            Happy
        }
    }
}