using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    
    
    private int timeSeconds = 0;
    [SerializeField]
    private Transform lvl1;
    [SerializeField]
    private Transform lvl2;
    [SerializeField]
    private Transform lvl3;
    [SerializeField]
    private TextMeshProUGUI timer;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject options;
    [SerializeField]
    private GameObject resultsScreen;
    [SerializeField]
    private TextMeshProUGUI resultsText;
    [SerializeField]
    private GameObject backgroundMusic;
    [SerializeField]
    private GameObject[] buttons;
    
    public static GameController instance;
    private void Awake()
    {
        instance = this;
        menu.SetActive(false);
        options.SetActive(false);
        resultsScreen.SetActive(false);

        backgroundMusic.GetComponent<AudioSource>().volume = Settings.musicVolume;

        ChangeHudVisibility();
        ChangeTimerState();

        StartCoroutine(MeasureTime(1.0f));
    }

    public void ChangeTimerState()
    {
        if (!Settings.isTimerOn)
            timer.alpha = 0;
        else
        {
            timer.alpha = 1;
            DisplayTime();
        }
    }

    public void EndLevel()
    {
        resultsText.text = $"Time: {GetGameTime()}";
        ShowResults();
        Time.timeScale = 0;;
    }

    public void ChangeHudVisibility()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].GetComponent<CanvasGroup>().alpha = Settings.hudVisibility;
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
    }

    public void HideMenu()
    {
        menu.SetActive(false);
    }

    public void ShowResults()
    {
        resultsScreen.SetActive(true);
    }

    public void ShowOptions()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void HideOptions()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void SaveAndQuit()
    {
        SceneManager.LoadScene("Menu");
    }

    private string GetGameTime()
    {
        var hours = timeSeconds / 3600;
        var minutes = (timeSeconds % 3600) / 60;
        var seconds = (timeSeconds % 3600) % 60;

        var hoursS = $"{hours}";
        if (hours < 10)
            hoursS = "0" + hoursS;
        var minutesS = $"{minutes}";
        if (minutes < 10)
            minutesS = "0" + minutesS;
        var secondsS = $"{seconds}";
        if (seconds < 10)
            secondsS = "0" + secondsS;
        return $"{hoursS}:{minutesS}:{secondsS}";
    }

    private void DisplayTime()
    {
        
        timer.text = GetGameTime();
    }

    IEnumerator MeasureTime(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            timeSeconds += 1;
            if (Settings.isTimerOn)
               DisplayTime();
        }
    }
    
    

}
