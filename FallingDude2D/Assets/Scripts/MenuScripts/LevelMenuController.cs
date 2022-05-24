using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelMenuController : MonoBehaviour
{
    public string levelMenuName = "";

    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI levelTime;
    [SerializeField] TextMeshProUGUI levelFinishTime;
    [SerializeField] Button resetButton;
    [SerializeField] Image trophyImage;

    [SerializeField] MenuController menuController;

    private CurrentUser currentUser;

    private void OnEnable()
    {
        currentUser = GameObject.Find("CurrentUser").GetComponent<CurrentUser>();

        if (currentUser.userData.Name == null)
            return;

        levelName.text = levelMenuName;

        Dictionary<string, object> level = new Dictionary<string, object>();
        if (levelMenuName== "Level 1")
            level = currentUser.userLevels.Level1;
        else if (levelMenuName == "Level 2")
            level = currentUser.userLevels.Level2;
        else if (levelMenuName == "Level 3")
            level = currentUser.userLevels.Level3;


        if (!(bool)level["Finished"])
        {
            trophyImage.color = new Color32(0, 0, 0, 255);
            levelFinishTime.text = "--:--:--";
        }
        else
        {
            trophyImage.color = new Color32(255, 255, 255, 255);
            var finishTime = (int)System.Convert.ChangeType(level["FinishTime"], typeof(int));
            levelFinishTime.text = FormatTime(finishTime);
        }

        var time = (int)System.Convert.ChangeType(level["Time"], typeof(int));
        if (time == 0)
            resetButton.interactable = false;

        levelTime.text = FormatTime(time);
    }

    public void GoBack()
    {
        menuController.GoToLevels();
    }

    public void ResetConfirm()
    {
        menuController.ShowConfirmationScreen();
    }

    public void NoConfirmation()
    {
        menuController.HideConfirmScreen();
    }

    public void Play()
    {
        if (levelMenuName == "Level 1")
            SceneManager.LoadScene("Level1");
        else if (levelMenuName == "Level 2")
            Debug.Log("No level 2");
        else if (levelMenuName == "Level 3")
            Debug.Log("No level 2");
    }

    public void Reset()
    {
        if (levelMenuName == "Level 1")
        {
            currentUser.userLevels.Level1["Time"] = 0;
            currentUser.userLevels.Level1["Finished"] = false;
        }
        else if (levelMenuName == "Level 2")
        {
            currentUser.userLevels.Level2["Time"] = 0;
            currentUser.userLevels.Level2["Finished"] = false;
        }
        else if (levelMenuName == "Level 3")
        {
            currentUser.userLevels.Level3["Time"] = 0;
            currentUser.userLevels.Level3["Finished"] = false;
        }

        menuController.HideConfirmScreen();
        GoBack();
        currentUser.WriteLevels();
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
