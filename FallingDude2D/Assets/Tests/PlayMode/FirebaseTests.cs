using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class FirebaseTests
{
    private string userId = "Q38EqBktYyRGmerXFGNFn5dgxqN2";

    private GameObject CreateScreen()
    {
        var screen = new GameObject();
        screen.AddComponent<RectTransform>();
        screen.AddComponent<CanvasGroup>();
        screen.SetActive(false);

        return screen;
    }

    [UnityTest]
    public IEnumerator LoadUserDataTest()
    {
        var gameObject = new GameObject();
        var currentUser = gameObject.AddComponent<CurrentUser>();
        currentUser.SetUserId(userId);
        Assert.AreEqual(true, currentUser.userData.Name == null);
        currentUser.GetUserData();
        yield return new WaitWhile(() => currentUser.userData.Name == null);
        Assert.AreEqual(false, currentUser.userData.Name == null);
        CurrentUser.instance = null;
    }

    [UnityTest]
    public IEnumerator LoadUserLevelsTest()
    {
        var gameObject = new GameObject();
        var currentUser = gameObject.AddComponent<CurrentUser>();
        currentUser.SetUserId(userId);
        Assert.AreEqual(true, currentUser.userLevels.NickName == null);
        currentUser.GetUserLevels();
        yield return new WaitWhile(() => currentUser.userLevels.NickName == null);
        Assert.AreEqual(false, currentUser.userLevels.NickName == null);
        CurrentUser.instance = null;
    }

    [UnityTest]
    public IEnumerator WriteUserLevelsTest()
    {
        var gameObject = new GameObject();
        var currentUser = gameObject.AddComponent<CurrentUser>();
        currentUser.SetUserId(userId);
        currentUser.GetUserLevels();
        yield return new WaitWhile(() => currentUser.userLevels.NickName == null);
        var oldNickname = currentUser.userLevels.NickName;
        var newNickname = "123abc";
        // change nickname in user levels data
        currentUser.userLevels.NickName = newNickname;
        currentUser.WriteLevels();
        currentUser.userLevels.NickName = oldNickname;
        Assert.AreEqual(oldNickname, currentUser.userLevels.NickName);
        currentUser.GetUserLevels();
        yield return new WaitWhile(() => currentUser.userLevels.NickName != oldNickname);
        Assert.AreEqual(newNickname, currentUser.userLevels.NickName);
        // revert the change
        currentUser.userLevels.NickName = oldNickname;
        currentUser.WriteLevels();
        CurrentUser.instance = null;
    }

    [UnityTest]
    public IEnumerator WriteUserDataTest()
    {
        var gameObject = new GameObject();
        var currentUser = gameObject.AddComponent<CurrentUser>();
        currentUser.SetUserId(userId);
        currentUser.GetUserData();
        yield return new WaitWhile(() => currentUser.userData.Nickname == null);
        var oldNickname = currentUser.userData.Nickname;
        var newNickname = "123abc";
        // change nickname in user data
        currentUser.userData.Nickname = newNickname;
        currentUser.WriteUser();
        currentUser.userData.Nickname = oldNickname;
        Assert.AreEqual(oldNickname, currentUser.userData.Nickname);
        currentUser.GetUserData();
        yield return new WaitWhile(() => currentUser.userData.Nickname != oldNickname);
        Assert.AreEqual(newNickname, currentUser.userData.Nickname);
        // revert the change
        currentUser.userData.Nickname = oldNickname;
        currentUser.WriteUser();
        CurrentUser.instance = null;
    }

    //[UnityTest]
    //public IEnumerator SigningInTest()
    //{
    //    //menu controller
    //    var menuObject = new GameObject();
    //    var menuController = menuObject.AddComponent<MenuController>();
    //    menuController.loadingScreen = CreateScreen();
    //    menuController.startScreen = CreateScreen();
    //    menuController.currentScreen = menuController.startScreen;
    //    menuController.startScreen.SetActive(true);
    //    // current user
    //    var currentUserObject = new GameObject();
    //    var currentUser = currentUserObject.AddComponent<CurrentUser>();
    //    var gameObject = new GameObject();
    //    // email and password fields
    //    var emailObject = new GameObject();
    //    emailObject.name = "LoginField";
    //    var email = emailObject.AddComponent<TMP_InputField>();
    //    email.text = "janek@gmail.com";
    //    var passObject = new GameObject();
    //    passObject.name = "PasswordField";
    //    var pass = passObject.AddComponent<TMP_InputField>();
    //    pass.text = "123456";
    //    //auth manager
    //    var authManager = gameObject.AddComponent<AuthManager>();
    //    authManager.menuController = menuObject;
    //    authManager.currentUser = currentUserObject;
    //    yield return new WaitUntil(() => menuController.startScreen.activeSelf || menuController.loadingScreen.activeSelf);
    //    if (menuController.loadingScreen.activeSelf)
    //    {
    //        authManager.SignOutButton();
    //        yield return new WaitUntil(() => menuController.startScreen.activeSelf);
    //        Assert.AreEqual(true, menuController.startScreen.activeSelf);
    //    }
    //    //sign in
    //    yield return new WaitForSeconds(1);
    //    authManager.SignInButton();
    //    yield return new WaitUntil(() => menuController.loadingScreen.activeSelf);
    //    Assert.AreEqual(true, menuController.loadingScreen.activeSelf);
    //    CurrentUser.instance = null;
    //    AuthManager.instance = null;
    //}
}