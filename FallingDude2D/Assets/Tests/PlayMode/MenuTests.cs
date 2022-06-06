using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MenuTests
{
    private MenuController menuController;

    private GameObject CreateScreen()
    {
        var screen = new GameObject();
        screen.AddComponent<RectTransform>();
        screen.AddComponent<CanvasGroup>();
        screen.SetActive(false);

        return screen;
    }

    public MenuTests()
    {
        var menuObject = new GameObject();
        menuController = menuObject.AddComponent<MenuController>();

        menuController.menuScreen = CreateScreen();
        menuController.startScreen = CreateScreen();
        menuController.signInScreen = CreateScreen();
        menuController.loadingScreen = CreateScreen();
        menuController.levelMenuScreen = CreateScreen();
        menuController.confirmScreen = CreateScreen();
        menuController.levelsScreen = CreateScreen();
        menuController.errorScreen = CreateScreen();
        menuController.optionsScreen = CreateScreen();
        menuController.rankingScreen = CreateScreen();
        menuController.profileScreen = CreateScreen();
        menuController.screenChangeSpeed = 2f;

        menuController.currentScreen = menuController.startScreen;
        menuController.mainPosition = menuController.startScreen.GetComponent<RectTransform>().anchoredPosition;

        menuController.startScreen.SetActive(true);
    }

    [UnityTest]
    public IEnumerator GoToMenu()
    {
        menuController.GoToMenu();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(true, menuController.menuScreen.activeSelf);

        menuController.GoToStartScreen();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(false, menuController.menuScreen.activeSelf);

        Assert.AreEqual(true, menuController.startScreen.activeSelf);

    }

    [UnityTest]
    public IEnumerator GoToSignInScreen()
    {
        menuController.GoToSignInScreen();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(true, menuController.signInScreen.activeSelf);

        menuController.GoToStartScreen();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(false, menuController.signInScreen.activeSelf);

        Assert.AreEqual(true, menuController.startScreen.activeSelf);

    }

    [UnityTest]
    public IEnumerator GoToProfileScreen()
    {
        menuController.GoToProfile();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(true, menuController.profileScreen.activeSelf);

        menuController.GoToStartScreen();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(false, menuController.profileScreen.activeSelf);

        Assert.AreEqual(true, menuController.startScreen.activeSelf);

    }

    [UnityTest]
    public IEnumerator GoToRankingScreen()
    {
        menuController.GoToRanking();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(true, menuController.rankingScreen.activeSelf);

        menuController.GoToStartScreen();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(false, menuController.rankingScreen.activeSelf);

        Assert.AreEqual(true, menuController.startScreen.activeSelf);

    }

    [UnityTest]
    public IEnumerator GoToOptionsScreen()
    {
        menuController.GoToOptions();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(true, menuController.optionsScreen.activeSelf);

        menuController.GoToStartScreen();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(false, menuController.optionsScreen.activeSelf);

        Assert.AreEqual(true, menuController.startScreen.activeSelf);

    }

    [UnityTest]
    public IEnumerator GoToLevelsScreen()
    {
        menuController.GoToLevels();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(true, menuController.levelsScreen.activeSelf);

        menuController.GoToStartScreen();

        yield return new WaitForSeconds(1.5f);

        Assert.AreEqual(false, menuController.levelsScreen.activeSelf);

        Assert.AreEqual(true, menuController.startScreen.activeSelf);

    }
}
