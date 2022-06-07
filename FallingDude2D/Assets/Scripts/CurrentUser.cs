using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.IO;
using System;

public class CurrentUser : MonoBehaviour
{
    public static CurrentUser instance;

    public static string nickName = "";

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

    private static User GetDefaultUser()
    {
        return new User()
        {
            Nickname = "DefaultUser",
            Name = "-----",
            Surname = "-----",
            BirthDate = DateTime.Today,
            MusicVolume = 100,
            IsTimerOn = true,
            HudVisibility = 100
        };
    }

    private static Levels GetDefaultLevels()
    {
        Dictionary<string, object> level1 = new Dictionary<string, object>
        {
            {"FinishTime" , 0 },
            {"Finished" , false },
            {"Started" , true },
            {"Time" , 0 }
        };
        Dictionary<string, object> level2 = new Dictionary<string, object>
        {
            {"FinishTime" , 0 },
            {"Finished" , false },
            {"Started" , false },
            {"Time" , 0 }
        };
        Dictionary<string, object> level3 = new Dictionary<string, object>
        {
            {"FinishTime" , 0 },
            {"Finished" , false },
            {"Started" , false },
            {"Time" , 0 }
        };
        return new Levels() { Level1 = level1, Level2 = level2, Level3 = level3, NickName=String.Empty};
    }

    public void LoadOfflineSave()
    {
        userId = string.Empty;
        try
        {
            userLevels = ReadObjectFromFile<Levels>(Application.persistentDataPath + "/offlineLevels.tmp");
            userData = ReadObjectFromFile<User>(Application.persistentDataPath + "/offlineUser.tmp");
            Settings.hudVisibility = userData.HudVisibility;
            Settings.isTimerOn = userData.IsTimerOn;
            Settings.musicVolume = userData.MusicVolume;
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            userLevels = GetDefaultLevels();
            userData = GetDefaultUser();
        }
    }

    private T ReadObjectFromFile<T>(string path)
    {
        using (Stream stream = File.Open(path, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
    }

    private void WriteObjectToFile<T>(T objectToWrite, string path)
    {
        using (Stream stream = File.Open(path, FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
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
        birthDateTime = birthDateTime.AddHours(12);
        Debug.Log(birthDateTime);
        userData.BirthDate = birthDateTime;
    }

    private void startMusic()
    {
        GameObject backgroundMusic = GameObject.Find("BackgroundMusic");
        backgroundMusic.GetComponent<AudioSource>().Play();
        backgroundMusic.GetComponent<AudioSource>().volume = Settings.musicVolume;
    }

    public void WriteUser()
    {
        if (userId != string.Empty)
        {
            firestore.Document("users/" + userId).SetAsync(userData);
        }
        else
            WriteObjectToFile(userData, Application.persistentDataPath + "/offlineUser.tmp");
    }

    public void WriteLevels()
    {
        if (userId != string.Empty)
            firestore.Document("levels/" + userId).SetAsync(userLevels);
        else
            WriteObjectToFile(userLevels, Application.persistentDataPath + "/offlineLevels.tmp");
    }

    public void GetUserLevels()
    {
        firestore.Document("levels/" + userId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if(task.Exception == null && userId != string.Empty)
            {
                userLevels = task.Result.ConvertTo<Levels>();
                Debug.Log("levels loaded");
                GameObject.Find("MenuController").GetComponent<MenuController>().GoToMenu();
            }
            else
            {
                Debug.Log(task.Exception);
            }

        });
    } 

    public void GetUserData()
    {
        firestore.Document("users/" + userId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception == null && userId != string.Empty)
            {
                userData = task.Result.ConvertTo<User>();
                userData.BirthDate.ToLocalTime();
                nickName = userData.Nickname;
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
            }
        });
    }
}
