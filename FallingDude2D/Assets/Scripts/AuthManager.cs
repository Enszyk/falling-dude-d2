using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menuController;
    [SerializeField]
    private GameObject currentUser;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Login")]
    public TMP_InputField email;
    public TMP_InputField password;
    public string message = "";

    private void Start()
    {
        StartCoroutine(CheckAndFixDependencies());
    }

    private IEnumerator CheckAndFixDependencies()
    {
        var task = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);

        dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.Log("Could not resolove all Firebase dependecies: " + dependencyStatus);
        }
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        if(user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);

            currentUser.GetComponent<CurrentUser>().SetUserId(user.UserId);
            menuController.GetComponent<MenuController>().GoToMenu();
        }
        else
        {
            menuController.GetComponent<MenuController>().GoToStartScreen();
        }
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed Out");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log($"Signed In: {user.Email}");
            }
        }
    }

    public void SignInButton()
    {
        StartCoroutine(SignIn());
    }

    public void SignOutButton()
    {
        auth.SignOut();
        menuController.GetComponent<MenuController>().GoToStartScreen();
    }

    private IEnumerator SignIn()
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email.text, password.text);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if(LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            message = "Login Failed!";

            switch(errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            menuController.GetComponent<MenuController>().ShowError(message);
        }
        else
        {
            user = LoginTask.Result;
            Debug.Log("User signed in successfully");
            currentUser.GetComponent<CurrentUser>().SetUserId(user.UserId);
            menuController.GetComponent<MenuController>().GoToMenu();
        }
    }

}
