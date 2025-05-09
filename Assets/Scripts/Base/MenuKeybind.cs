using UnityEngine;

public class MenuKeybind : MonoBehaviour
{
    public enum KeyType
    {
        InputKey = 0,
        MovementKey = 1
    }

    public KeyType keyType;

    public InputKey inputKey;

    public MovementDirection movementDirection;

    public void OnClick()
    {
    }
}