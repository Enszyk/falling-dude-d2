using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;

    [SerializeField]
    public GameObject menuController;
    [SerializeField]
    public GameObject currentUser;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Login")]
    public TMP_InputField email;
    public TMP_InputField password;
    public string message = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            instance.menuController = GameObject.Find("MenuController");
            Destroy(gameObject);
        }
    }

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
            menuController.GetComponent<MenuController>().ShowLoadingScreen();
            MenuController.offline = false;
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
        email = GameObject.Find("LoginField").GetComponent<TMP_InputField>();
        password = GameObject.Find("PasswordField").GetComponent<TMP_InputField>();
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

            message = "Brak internetu!";

            switch(errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Brakuje Email'a";
                    break;
                case AuthError.MissingPassword:
                    message = "Brakuje Hasła";
                    break;
                case AuthError.WrongPassword:
                    message = "Złe Hasło";
                    break;
                case AuthError.InvalidEmail:
                    message = "Zły Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Konto nie istnieje";
                    break;
            }
            menuController.GetComponent<MenuController>().ShowError(message);
        }
        else
        {
            user = LoginTask.Result;
            Debug.Log("User signed in successfully");
            currentUser.GetComponent<CurrentUser>().SetUserId(user.UserId);
            menuController.GetComponent<MenuController>().ShowLoadingScreen();
            MenuController.offline = false;
        }
    }

}
