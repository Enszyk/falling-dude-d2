using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject signInScreen;
    [SerializeField] private GameObject profileScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject errorScreen;
    [SerializeField] private GameObject errorMessage;
    [SerializeField] private float screenChangeSpeed;
    private GameObject currentScreen;
    private GameObject nextScreen;
    private Vector3 mainPosition;
    private bool fadeIn = false;
    private bool fadeOut = false;

    private void Awake()
    {
        startScreen.SetActive(false);
        menuScreen.SetActive(false);

        mainPosition = startScreen.GetComponent<RectTransform>().anchoredPosition;
        currentScreen = startScreen;
    }

    private void Update()
    {
        if(fadeIn)
        {
            var canvas = currentScreen.GetComponent<CanvasGroup>();
            canvas.alpha += Time.deltaTime * screenChangeSpeed;
            if(canvas.alpha >= 1)
                fadeIn = false;
        }

        if (fadeOut)
        {
            var canvas = currentScreen.GetComponent<CanvasGroup>();
            canvas.alpha -= Time.deltaTime * screenChangeSpeed;
            if (canvas.alpha == 0)
            {
                fadeOut = false;
                currentScreen.SetActive(false);
                currentScreen = nextScreen;
                currentScreen.GetComponent<RectTransform>().anchoredPosition = mainPosition;
                currentScreen.GetComponent<CanvasGroup>().alpha = 0;
                currentScreen.SetActive(true);
                fadeIn = true;
            }
        }

    }

    private void ChangeScreen(GameObject screen)
    {
        fadeOut = true;
        nextScreen = screen;
    }

    public void GoToSignInScreen()
    {
        ChangeScreen(signInScreen);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToStartScreen()
    {
        ChangeScreen(startScreen);
    }

    public void ShowError(string message)
    {
        errorScreen.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(mainPosition.x, mainPosition.y + 100, mainPosition.z);
        errorScreen.SetActive(true);
        errorMessage.GetComponent<TMP_Text>().text = message;
    }

    public void HideError()
    {
        errorScreen.SetActive(false);
    }

    public void GoToMenu()
    {
        ChangeScreen(menuScreen);
    }

    public void GoToProfile()
    {
        ChangeScreen(profileScreen);
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
