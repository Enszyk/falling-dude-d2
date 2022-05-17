using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private GameObject backgroundMusicSource;
    [SerializeField] private GameObject musicSlider;
    [SerializeField] private GameObject hudSlider;
    [SerializeField] private GameObject timerToggle;
    private GameObject currentUser = null;

    private void OnEnable()
    {
        currentUser = GameObject.Find("CurrentUser");
        musicSlider.GetComponent<Slider>().value = Settings.musicVolume;
        hudSlider.GetComponent<Slider>().value = Settings.hudVisibility;
        timerToggle.GetComponent<Toggle>().isOn = Settings.isTimerOn;
    }

    public void ChangeSoundValue(float value)
    {
        var audioSource = backgroundMusicSource.GetComponent<AudioSource>();
        audioSource.volume = value;
        Settings.musicVolume = value;
        if (currentUser != null && !(currentUser.GetComponent<CurrentUser>().userData.Nickname == ""))
            currentUser.GetComponent<CurrentUser>().userData.MusicVolume = value;
    }

    public void ChangeHudVisibility(float value)
    {
        Settings.hudVisibility = value;
        GameObject gameController = GameObject.Find("GameController");
        if (gameController != null)
            gameController.GetComponent<GameController>().ChangeHudVisibility();
        if (currentUser != null && !(currentUser.GetComponent<CurrentUser>().userData.Nickname == ""))
            currentUser.GetComponent<CurrentUser>().userData.HudVisibility = value;
    }

    public void ChangeTimerState(bool state)
    {
        Settings.isTimerOn = state;
        GameObject gameController = GameObject.Find("GameController");
        if (gameController != null)
            gameController.GetComponent<GameController>().ChangeTimerState();
        if (currentUser != null && !(currentUser.GetComponent<CurrentUser>().userData.Nickname == ""))
            currentUser.GetComponent<CurrentUser>().userData.IsTimerOn = state;
    }

    private void OnDisable()
    {
        if(currentUser != null && currentUser.GetComponent<CurrentUser>().userData.Nickname != null)
            currentUser.GetComponent<CurrentUser>().WriteUser();
    }

}
