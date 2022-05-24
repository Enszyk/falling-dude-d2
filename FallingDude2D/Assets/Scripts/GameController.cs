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

        //var user = GameObject.Find("CurrentUser").GetComponent<CurrentUser>();

        //var currentLevel = SceneManager.GetActiveScene().name;
        //if (currentLevel == "Level1")
        //{
        //    timeSeconds = (int)System.Convert.ChangeType(user.userLevels.Level1["Time"], typeof(int));
        //}
        //else if (currentLevel == "Level2")
        //{
        //    timeSeconds = (int)System.Convert.ChangeType(user.userLevels.Level2["Time"], typeof(int));
        //}
        //else if (currentLevel == "Level3")
        //{
        //    timeSeconds = (int)System.Convert.ChangeType(user.userLevels.Level3["Time"], typeof(int));
        //}

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
        var user = GameObject.Find("CurrentUser").GetComponent<CurrentUser>();

        var currentLevel = SceneManager.GetActiveScene().name;
        if (currentLevel == "Level1")
        {
            user.userLevels.Level1["Time"] = (int)System.Convert.ChangeType(user.userLevels.Level1["Time"], typeof(int)) + timeSeconds; 
            if (!(bool)user.userLevels.Level1["Finished"] || (int)System.Convert.ChangeType(user.userLevels.Level1["FinishTime"], typeof(int)) > timeSeconds)
            {
                user.userLevels.Level1["Finished"] = true;
                user.userLevels.Level1["FinishTime"] = timeSeconds;
                user.userLevels.Level2["Started"] = true;
            }
        }
        else if (currentLevel == "Level2")
        {
            user.userLevels.Level2["Time"] = (int)System.Convert.ChangeType(user.userLevels.Level2["Time"], typeof(int)) + timeSeconds;
            if (!(bool)user.userLevels.Level2["Finished"] || (int)System.Convert.ChangeType(user.userLevels.Level2["FinishTime"], typeof(int)) > timeSeconds)
            {
                user.userLevels.Level2["Finished"] = true;
                user.userLevels.Level2["FinishTime"] = timeSeconds;
                user.userLevels.Level3["Started"] = true;
            }
        }
        else if (currentLevel == "Level3")
        {
            user.userLevels.Level3["Time"] = (int)System.Convert.ChangeType(user.userLevels.Level3["Time"], typeof(int)) + timeSeconds;
            if (!(bool)user.userLevels.Level3["Finished"] || (int)System.Convert.ChangeType(user.userLevels.Level3["FinishTime"], typeof(int)) > timeSeconds)
            {
                user.userLevels.Level3["Finished"] = true;
                user.userLevels.Level3["FinishTime"] = timeSeconds;
            }
        }

        resultsText.text = $"Czas: {GetGameTime()}";
        ShowResults();
        Time.timeScale = 0;
        user.WriteLevels();
    }

    public void FinishLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
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
        var user = GameObject.Find("CurrentUser").GetComponent<CurrentUser>();

        var currentLevel = SceneManager.GetActiveScene().name;
        if (currentLevel == "Level1")
            user.userLevels.Level1["Time"] = (int)System.Convert.ChangeType(user.userLevels.Level1["Time"], typeof(int)) + timeSeconds;
        else if (currentLevel == "Level2")
            user.userLevels.Level2["Time"] = (int)System.Convert.ChangeType(user.userLevels.Level2["Time"], typeof(int)) + timeSeconds;
        else if (currentLevel == "Level3")
            user.userLevels.Level3["Time"] = (int)System.Convert.ChangeType(user.userLevels.Level3["Time"], typeof(int)) + timeSeconds;

        user.WriteLevels();

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
