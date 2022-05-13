using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileController : MonoBehaviour
{
    [SerializeField] private GameObject nicknameText;
    [SerializeField] private GameObject nameText;
    [SerializeField] private GameObject surnameText;
    [SerializeField] private GameObject birthDateText;

    [SerializeField] private GameObject currentUser;

    private void OnEnable()
    {
        var userData = currentUser.GetComponent<CurrentUser>().userData;
        Debug.Log(userData.Name);
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

}
