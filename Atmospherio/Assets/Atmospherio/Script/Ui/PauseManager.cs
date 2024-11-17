using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool _gameIsPaused;
    [SerializeField] private GameObject _uiPause;
    [SerializeField] private GameObject _uiButtons;
    [SerializeField] private GameObject _uiOptions;
    [SerializeField] private PlayerInput _playerInput;

    private void Start()
    {
        _playerInput.actions.actionMaps[2].Enable();
    }
    
    public void Escape(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled) return;
        
        Pause();
    }

    public void Pause()
    {
        if (_gameIsPaused)
        {
            _gameIsPaused = false;
            Time.timeScale = 1f;
            _uiButtons.SetActive(true);
            _uiOptions.SetActive(false);
            _uiPause.SetActive(false);
        }
        else
        {
            _gameIsPaused = true;
            Time.timeScale = 0f;
            _uiPause.SetActive(true);
            _playerInput.actions.actionMaps[0].Disable();
        }
    }
    public void Option()
    {
        _uiButtons.SetActive(false);
        _uiOptions.SetActive(true);
    }
    public void Back()
    {
        _uiButtons.SetActive(true);
        _uiOptions.SetActive(false);
    }
    public void MainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
