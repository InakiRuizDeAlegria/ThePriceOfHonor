using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public InputActionReference leftHandSecondaryButtonAction;

    public GameObject pauseMenu;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    private void OnEnable()
    {
        if (leftHandSecondaryButtonAction != null)
        {
            leftHandSecondaryButtonAction.action.performed += TogglePauseMenu;
            leftHandSecondaryButtonAction.action.Enable();
        }

        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        if (leftHandSecondaryButtonAction != null)
        {
            leftHandSecondaryButtonAction.action.performed -= TogglePauseMenu;
            leftHandSecondaryButtonAction.action.Disable();
        }

        resumeButton.onClick.RemoveListener(ResumeGame);
        quitButton.onClick.RemoveListener(QuitGame);
    }

    private void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit(); 
    }
}
