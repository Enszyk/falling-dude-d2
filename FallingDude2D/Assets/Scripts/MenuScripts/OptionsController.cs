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

    private void Awake()
    {
        musicSlider.GetComponent<Slider>().value = Settings.musicVolume;
        hudSlider.GetComponent<Slider>().value = Settings.hudVisibility;
        timerToggle.GetComponent<Toggle>().isOn = Settings.isTimerOn;
    }

    public void ChangeSoundValue(float value)
    {
        var audioSource = backgroundMusicSource.GetComponent<AudioSource>();
        audioSource.volume = value;
        Settings.musicVolume = value;
    }

    public void ChangeHudVisibility(float value)
    {
        Settings.hudVisibility = value;
    }

    public void ChangeTimerState(bool state)
    {
        Settings.isTimerOn = state;
    }

}
