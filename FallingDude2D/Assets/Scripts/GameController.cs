using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    private GameObject backgroundMusic;
    [SerializeField]
    private GameObject[] buttons;

    private void Awake()
    {
        menu.SetActive(false);
        options.SetActive(false);

        backgroundMusic.GetComponent<AudioSource>().volume = Settings.musicVolume;

        ChangeHudVisibility();
        ChangeTimerState();

        StartCoroutine(MeasureTime(1.0f));
    }

    private void Update()
    {

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

    private void DisplayTime()
    {
        int hours = timeSeconds / 3600;
        int minutes = (timeSeconds % 3600) / 60;
        int seconds = (timeSeconds % 3600) % 60;

        string hoursS = $"{hours}";
        if (hours < 10)
            hoursS = "0" + hoursS;
        string minutesS = $"{minutes}";
        if (minutes < 10)
            minutesS = "0" + minutesS;
        string secondsS = $"{seconds}";
        if (seconds < 10)
            secondsS = "0" + secondsS;
        timer.text = $"{hoursS}:{minutesS}:{secondsS}";
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
