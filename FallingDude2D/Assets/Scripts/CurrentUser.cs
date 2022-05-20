using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class CurrentUser : MonoBehaviour
{
    private static CurrentUser instance;

    public User userData;
    public Levels userLevels;
    public string userId;

    private FirebaseFirestore firestore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
            Destroy(gameObject);

        firestore = FirebaseFirestore.DefaultInstance;
    }

    public void SetUserId(string userId)
    {
        this.userId = userId;
        GetUserData();
    }

    public void SetBirthDate(string birthDate)
    {
        string[] dateFields = birthDate.Split('-');

        System.Int32.TryParse(dateFields[0], out int day);
        System.Int32.TryParse(dateFields[1], out int month);
        System.Int32.TryParse(dateFields[2], out int year);

        var birthDateTime = new System.DateTime(year, month, day);
        userData.BirthDate = Timestamp.FromDateTime(birthDateTime);
    }

    private void startMusic()
    {
        GameObject backgroundMusic = GameObject.Find("BackgroundMusic");
        backgroundMusic.GetComponent<AudioSource>().Play();
        backgroundMusic.GetComponent<AudioSource>().volume = Settings.musicVolume;
    }

    public void WriteUser()
    {
        firestore.Document("users/" + userId).SetAsync(userData);
    }

    public void WriteLevels()
    {
        firestore.Document("levels/" + userId).SetAsync(userLevels);
    }

    public void GetUserLevels()
    {
        firestore.Document("levels/" + userId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.Exception == null)
            {
                userLevels = task.Result.ConvertTo<Levels>();
                Debug.Log("levels loaded");
                GameObject.Find("MenuController").GetComponent<MenuController>().GoToMenu();
            }
            else
            {
                Debug.Log(task.Exception);
                userLevels = new Levels();
            }

        });
    } 

    public void GetUserData()
    {
        firestore.Document("users/" + userId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception == null)
            {
                userData = task.Result.ConvertTo<User>();
                GetUserLevels();
                Settings.hudVisibility = userData.HudVisibility;
                Settings.isTimerOn = userData.IsTimerOn;
                Settings.musicVolume = userData.MusicVolume;
                startMusic();
                Debug.Log("user loaded");
            }
            else
            {
                Debug.Log(task.Exception);
                userData= new User();
            }
        });
    }
}
