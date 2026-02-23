using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject fpsCam;
    public GameObject tpsCam;

    PlayerInputActions input;
    bool usingFPS = true;

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Player.SwitchCamera.performed += OnSwitch;
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.SwitchCamera.performed -= OnSwitch;
        input.Player.Disable();
    }

    void Start()
    {
        fpsCam.SetActive(usingFPS);
        tpsCam.SetActive(!usingFPS);
    }

    void OnSwitch(InputAction.CallbackContext ctx)
    {
        usingFPS = !usingFPS;

        fpsCam.SetActive(usingFPS);
        tpsCam.SetActive(!usingFPS);
    }
}