using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inGameMenuPanel;
    [SerializeField] private GameObject congratsPanel;

    [SerializeField] private TextMeshProUGUI enemiesAlertedText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI timeSpentText;

    private int enemiesAlerted = 0;
    private int enemiesKilled = 0;
    private float timeSpent = 0;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
    }

    public UIManager Instance { get => instance; set => instance = value; }
    public int EnemiesAlerted { get => enemiesAlerted; set => enemiesAlerted = value; }
    public int EnemiesKilled { get => enemiesKilled; set => enemiesKilled = value; }
    public float TimeSpent { get => timeSpent; set => timeSpent = value; }

    private void Update()
    {
        TimeSpent += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenuPanel();
        }
    }

    public void OpenWinPanelAndSetStats()
    {
        int minutes = Mathf.FloorToInt(timeSpent / 60F);
        int seconds = Mathf.FloorToInt(timeSpent - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        enemiesAlertedText.text = enemiesAlerted.ToString();
        enemiesKilledText.text  = enemiesKilled. ToString();
        timeSpentText.text      = niceTime;

        congratsPanel.SetActive(true);
    }

    private void toggleTime(bool toggle)
    {
        if (toggle)
        {
            Time.timeScale = 1.0f;
        } 
        else
        {
            Time.timeScale = 0.0f;
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void LoadScene(int sceneNumber)
    {
        Debug.Log("Scene " + sceneNumber + " loaded");
        SceneManager.LoadScene(sceneNumber);
    }

    public void ToggleMenuPanel()
    {
        if (inGameMenuPanel != null)
        {
            inGameMenuPanel.SetActive(!inGameMenuPanel.activeInHierarchy);
            toggleTime(!inGameMenuPanel.activeInHierarchy);
        }
    }
}
