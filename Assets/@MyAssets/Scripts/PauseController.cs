using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public GameObject menuPanel;
    public InputActionReference openMenuAction;
    public Transform playerCamera;

    private bool isPaused = false;

    private void Awake()
    {
        openMenuAction.action.Enable();
        openMenuAction.action.performed += ToggleMenu;
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDestroy()
    {
        openMenuAction.action.Disable();
        openMenuAction.action.performed -= ToggleMenu;
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        menuPanel.SetActive(isPaused);

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        PositionMenu();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private void PositionMenu()
    {
        Vector3 forwardDirection = playerCamera.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize();

        menuPanel.transform.position = playerCamera.position + forwardDirection * 2f;
        menuPanel.transform.LookAt(playerCamera);
        menuPanel.transform.Rotate(0, 180, 0);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                openMenuAction.action.Disable();
                openMenuAction.action.performed -= ToggleMenu;
                break;
            case InputDeviceChange.Reconnected:
                openMenuAction.action.Enable();
                openMenuAction.action.performed += ToggleMenu;
                break;
        }
    }

    void Start()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitApplication()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void ResumeGameButton()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

}
