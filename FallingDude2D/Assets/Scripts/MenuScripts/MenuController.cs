using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] public GameObject startScreen;
    [SerializeField] public GameObject signInScreen;
    [SerializeField] public GameObject profileScreen;
    [SerializeField] public GameObject menuScreen;
    [SerializeField] public GameObject optionsScreen;
    [SerializeField] public GameObject levelsScreen;
    [SerializeField] public GameObject levelMenuScreen;
    [SerializeField] public GameObject rankingScreen;

    [SerializeField] public GameObject confirmScreen;

    [SerializeField] public GameObject loadingScreen;

    [SerializeField] public GameObject errorScreen;
    [SerializeField] public GameObject errorMessage;
    [SerializeField] public GameObject backgroundMusic;
    [SerializeField] public float screenChangeSpeed;
    public GameObject currentScreen;
    private GameObject nextScreen;
    public Vector3 mainPosition;
    private bool fadeIn = false;
    private bool fadeOut = false;

    public float loadingTimeout = 10f;

    public static bool offline = false;

    private void Awake()
    {
        if (startScreen != null)
        {
            startScreen.SetActive(false);
            signInScreen.SetActive(false);
            profileScreen.SetActive(false);
            optionsScreen.SetActive(false);
            errorScreen.SetActive(false);
            levelsScreen.SetActive(false);
            confirmScreen.SetActive(false);
            levelMenuScreen.SetActive(false);
            loadingScreen.SetActive(false);


            mainPosition = startScreen.GetComponent<RectTransform>().anchoredPosition;
            currentScreen = startScreen;
        }

        if (AuthManager.instance != null)
        {
            GoToMenu();
            backgroundMusic.GetComponent<AudioSource>().Play();
            backgroundMusic.GetComponent<AudioSource>().volume = Settings.musicVolume;
        }
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
                currentScreen.GetComponent<RectTransform>().SetBottom(50);
                currentScreen.GetComponent<RectTransform>().SetTop(400);
                currentScreen.GetComponent<RectTransform>().SetLeft(55);
                currentScreen.GetComponent<RectTransform>().SetRight(55);
                currentScreen.GetComponent<CanvasGroup>().alpha = 0;
                currentScreen.SetActive(true);
                fadeIn = true;
            }
        }

    }

    public void DisableMenu()
    {
        menuScreen.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void ChangeScreen(GameObject screen)
    {
        if (!fadeIn && !fadeOut)
        {
            fadeOut = true;
            nextScreen = screen;
        }
    }

    public void GoToSignInScreen()
    {
        ChangeScreen(signInScreen);
    }

    public void GoToLevels()
    {
        ChangeScreen(levelsScreen);
    }

    public void GoToRanking()
    {
        ChangeScreen(rankingScreen);
    }

    public void GoToStartScreen()
    {
        ChangeScreen(startScreen);
    }

    public void ShowLoadingScreen()
    {
        StartCoroutine(LoadingTimeout());
        currentScreen.SetActive(false);
        loadingScreen.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(mainPosition.x, mainPosition.y + 100, mainPosition.z);
        loadingScreen.SetActive(true);
        currentScreen = loadingScreen;
    }

    public void GoToLevelMenu(string name)
    {
        levelMenuScreen.GetComponent<LevelMenuController>().levelMenuName = name;
        ChangeScreen(levelMenuScreen);
    }

    public void ShowConfirmationScreen()
    {
        confirmScreen.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(mainPosition.x, mainPosition.y + 100, mainPosition.z);
        confirmScreen.SetActive(true);
    }

    public void HideConfirmScreen()
    {
        confirmScreen.SetActive(false);
    }

    public void SignIn()
    {
        var authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();
        authManager.SignInButton();
    }

    public void SignOut()
    {
        var authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();
        authManager.SignOutButton();
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

    public void PlayOffline()
    {
        offline = true;
        var currentUser = GameObject.Find("CurrentUser").GetComponent<CurrentUser>();
        currentUser.LoadOfflineSave();
        GoToMenu();
    }

    public void GoToProfile()
    {
        ChangeScreen(profileScreen);
    }

    public void GoToOptions()
    {
        ChangeScreen(optionsScreen);
    }

    IEnumerator LoadingTimeout()
    {
        yield return new WaitForSeconds(loadingTimeout);
        if(currentScreen == loadingScreen)
        {
            GoToStartScreen();
            ShowError("Problem z internetem. Nie mo??na wczyta?? gry");
        }
    }

}

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}