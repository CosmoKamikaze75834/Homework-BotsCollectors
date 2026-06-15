using UnityEngine;

public class InputHandler : MonoBehaviour
{
    protected CameraControls Controls;

    private void Awake()
    {
        Controls = new CameraControls();
    }
}