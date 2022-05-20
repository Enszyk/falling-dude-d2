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
    [SerializeField] Button resetButton;
    [SerializeField] Image trophyImage;

    [SerializeField] MenuController menuController;

    private CurrentUser currentUser;

    private void OnEnable()
    {
        currentUser = GameObject.Find("CurrentUser").GetComponent<CurrentUser>();

        if (currentUser.userData.Name == null)
            return;

        Dictionary<string, object> level = new Dictionary<string, object>();
        if (levelMenuName== "Level 1")
            level = currentUser.userLevels.Level1;
        else if (levelMenuName == "Level 2")
            level = currentUser.userLevels.Level2;
        else if (levelMenuName == "Level 3")
            level = currentUser.userLevels.Level3;


        if (!(bool)level["Finished"])
            trophyImage.color = new Color32(0, 0, 0, 255);

        var time = (float)System.Convert.ChangeType(level["Time"], typeof(float));
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
            currentUser.userLevels.Level1["Time"] = 0;
        else if (levelMenuName == "Level 2")
            currentUser.userLevels.Level2["Time"] = 0;
        else if (levelMenuName == "Level 3")
            currentUser.userLevels.Level3["Time"] = 0;

        menuController.HideConfirmScreen();
        GoBack();
    }

    private string FormatTime(float timeSeconds)
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
}
