using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileController : MonoBehaviour
{
    [SerializeField] private GameObject menuController;

    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject editScreen;

    [SerializeField] private GameObject nicknameText;
    [SerializeField] private GameObject nameText;
    [SerializeField] private GameObject surnameText;
    [SerializeField] private GameObject birthDateText;

    [SerializeField] private TMP_InputField nicknameField;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField surnameField;
    [SerializeField] private TMP_InputField birthDateField;

    [SerializeField] private GameObject currentUser;

    private void Awake()
    {
        currentUser = GameObject.Find("CurrentUser");
    }

    private void OnEnable()
    {
        GoToMain();
    }

    private string GetAge(System.DateTime birthDate)
    {
        System.DateTime zeroTime = new System.DateTime(1, 1, 1);

        System.TimeSpan span = System.DateTime.Now - birthDate;
        int years = (zeroTime + span).Year - 1;

        return $"{years} lat";
    }

    private string GetBirthDate(System.DateTime birthDate)
    {
        int year = birthDate.Year;
        int month = birthDate.Month;
        int day = birthDate.Day + 1;

        string dayS = $"{day}";
        if (day < 10)
            dayS = "0" + dayS;

        string monthS = $"{month}";
        if (month < 10)
            monthS = "0" + monthS;

        return $"{dayS}-{monthS}-{year}";
    }

    public void GoToMain()
    {
        editScreen.SetActive(false);
        mainScreen.SetActive(true);
        mainScreen.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;

        var userData = currentUser.GetComponent<CurrentUser>().userData;
        nicknameText.GetComponent<TextMeshProUGUI>().text = userData.Nickname;
        nameText.GetComponent<TextMeshProUGUI>().text = userData.Name;
        surnameText.GetComponent<TextMeshProUGUI>().text = userData.Surname;
        birthDateText.GetComponent<TextMeshProUGUI>().text = GetAge(userData.BirthDate.ToDateTime());
    } 

    public void GoToEdit()
    {
        mainScreen.SetActive(false);
        editScreen.SetActive(true);
        editScreen.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;

        var userData = currentUser.GetComponent<CurrentUser>().userData;

        nicknameField.text = userData.Nickname;
        nameField.text = userData.Name;
        surnameField.text = userData.Surname;
        birthDateField.text = GetBirthDate(userData.BirthDate.ToDateTime());
    }

    private bool CheckTextField(string text)
    {
        if (text.Length < 2)
            return false;
        return true;
    }

    private bool CheckDateField(string text)
    {
        string[] dateFields = text.Split('-');

        if (dateFields.Length != 3)
            return false;

        bool success = System.Int32.TryParse(dateFields[0], out int day);
        if (!success)
            return false;
        success = System.Int32.TryParse(dateFields[1], out int month);
        if (!success)
            return false;
        success = System.Int32.TryParse(dateFields[2], out int year);
        if (!success)
            return false;

        try
        {
            var date = new System.DateTime(year, month, day);
        }
        catch(System.ArgumentOutOfRangeException)
        {
            return false;
        }

        return true;
    }

    public void Save()
    {
        if (!CheckTextField(nicknameField.text))
        {
            menuController.GetComponent<MenuController>().ShowError("Niepoprawny pseudonim");
            return;
        }
        if (!CheckTextField(nameField.text))
        {
            menuController.GetComponent<MenuController>().ShowError("Niepoprawne nazwisko");
            return;
        }
        if (!CheckTextField(surnameField.text))
        {
            menuController.GetComponent<MenuController>().ShowError("Niepoprawne imię");
            return;
        }
        if (!CheckDateField(birthDateField.text))
        {
            menuController.GetComponent<MenuController>().ShowError("Niepoprawna data ur.");
            return;
        }

        var user = currentUser.GetComponent<CurrentUser>();

        user.userData.Nickname = nicknameField.text;
        user.userData.Name = nameField.text;
        user.userData.Surname = surnameField.text;
        user.SetBirthDate(birthDateField.text);

        currentUser.GetComponent<CurrentUser>().WriteUser();

        GoToMain();
    }

}
