using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System;

[FirestoreData]
[Serializable]
public struct Levels
{
    [FirestoreProperty("NickName")]
    public string NickName { get; set; }

    [FirestoreProperty("Level1")]
    public Dictionary<string, object> Level1 { get; set; }

    [FirestoreProperty("Level2")]
    public Dictionary<string, object> Level2 { get; set; }

    [FirestoreProperty("Level3")]
    public Dictionary<string, object> Level3 { get; set; }
}