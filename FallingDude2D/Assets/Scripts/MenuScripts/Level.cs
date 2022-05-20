using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI levelTime;
    [SerializeField] GameObject levelLock;
    [SerializeField] Image trophyImage;

    [SerializeField] MenuController menuController;

    private CurrentUser currentUser;

    private void OnEnable()
    {
        currentUser = GameObject.Find("CurrentUser").GetComponent<CurrentUser>();

        if (currentUser.userData.Name == null)
            return;

        Dictionary<string, object> level = new Dictionary<string, object>();
        if (levelName.text == "Level 1")
            level = currentUser.userLevels.Level1;
        else if (levelName.text == "Level 2")
            level = currentUser.userLevels.Level2;
        else if (levelName.text == "Level 3")
            level = currentUser.userLevels.Level3;


        if (!(bool)level["Finished"])
        {
            trophyImage.color = new Color32(0, 0, 0, 255);
        }
        else
        {
            trophyImage.color = new Color32(255, 255, 255, 255);
        }

        if (!(bool)level["Started"])
        {
            levelLock.SetActive(true);
            GetComponent<Button>().interactable = false;
        }
        else
        {
            levelLock.SetActive(false);
            GetComponent<Button>().interactable = true;
        }


        var time = (int)System.Convert.ChangeType(level["Time"], typeof(int));
        levelTime.text = FormatTime(time);
    }

    public void GoToLevelMenu()
    {
        menuController.GoToLevelMenu(levelName.text);
    }

    private string FormatTime(int timeSeconds)
    {
        int hours = timeSeconds / 3600;
        int minutes = (timeSeconds % 3600) / 60;
        int seconds = (timeSeconds % 3600) % 60;

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
}
