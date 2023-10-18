using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverButtonManager : MonoBehaviour
{
    [SerializeField] private Button ButtonPlay;
    [SerializeField] private Button ButtonSettings;
    [SerializeField] private Button ExitButton;
    [SerializeField] private string RecomecarFase;
    [SerializeField] private GameObject ObjectToDisable;
    [SerializeField] private GameObject ObjectToEnable;


    private void Awake()
    {
        ButtonPlay.onClick.AddListener(OnButtonRestartClick);
        ButtonSettings.onClick.AddListener(OnButtonSettingsClick);
        ExitButton.onClick.AddListener(OnButtonExitClick);
    }

    private void OnButtonRestartClick()
    {
        // Depois configurar a opção de áudio
        Play();
    }


    private void Play()
    {
        SceneManager.LoadScene(RecomecarFase);
    }

    private void OnButtonSettingsClick()
    {
        // Depois configurar a opção de áudio
        Settings();
    }

    private void Settings()
    {
        ObjectToDisable.SetActive(false);
        ObjectToEnable.SetActive(true);
    }
    private void OnButtonExitClick()
    {
        // Depois configurar a opção de áudio
        Exit();
    }

    private void Exit()
    {
        ObjectToDisable.SetActive(true);
        ObjectToEnable.SetActive(false);
    }
}
