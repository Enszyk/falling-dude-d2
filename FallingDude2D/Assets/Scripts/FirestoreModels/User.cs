using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System;

[FirestoreData]
[Serializable]
public struct User
{
    [FirestoreProperty]
    public string Nickname { get; set; }

    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Surname { get; set; }

    [FirestoreProperty]
    public DateTime BirthDate { get; set; }

    //Settings
    [FirestoreProperty("MusicVolume")]
    public float MusicVolume { get; set; }

    [FirestoreProperty("HudVisibility")]
    public float HudVisibility { get; set; }

    [FirestoreProperty("IsTimerOn")]
    public bool IsTimerOn { get; set; }
}
