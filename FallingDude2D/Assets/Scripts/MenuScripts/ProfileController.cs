using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileController : MonoBehaviour
{
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

    private void OnEnable()
    {
        mainScreen.SetActive(true);
        mainScreen.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        editScreen.SetActive(false);

        var userData = currentUser.GetComponent<CurrentUser>().userData;
        nicknameText.GetComponent<TextMeshProUGUI>().text = userData.Nickname;
        nameText.GetComponent<TextMeshProUGUI>().text = userData.Name;
        surnameText.GetComponent<TextMeshProUGUI>().text = userData.Surname;
        birthDateText.GetComponent<TextMeshProUGUI>().text = GetAge(userData.BirthDate.ToDateTime());
    }

    private string GetAge(System.DateTime birthDate)
    {
        System.DateTime zeroTime = new System.DateTime(1, 1, 1);

        System.TimeSpan span = System.DateTime.Now - birthDate;
        int years = (zeroTime + span).Year - 1;

        return $"{years} lat";
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
        birthDateField.text = userData.BirthDate.ToString();
    }

    public void Save()
    {
        editScreen.SetActive(false);
        mainScreen.SetActive(true);
        mainScreen.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }

}
