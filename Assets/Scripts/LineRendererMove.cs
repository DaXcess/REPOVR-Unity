using UnityEngine;

public class LineRendererMove : MonoBehaviour
{
    private Material lineMaterial;

    private void Awake()
    {
        lineMaterial = GetComponent<LineRenderer>().material;
    }

    private void Update()
    {
        lineMaterial.mainTextureOffset += Vector2.left * (Time.deltaTime * 2f);
    }
}
