using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    [SerializeField] GameObject buttonLvL1;
    [SerializeField] GameObject buttonLvL2;
    [SerializeField] GameObject buttonLvL3;
    [SerializeField] GameObject buttonAll;

    [SerializeField] GameObject rankingLvL1;
    [SerializeField] GameObject scrollLvL1;
    [SerializeField] GameObject rankingLvL2;
    [SerializeField] GameObject scrollLvL2;
    [SerializeField] GameObject rankingLvL3;
    [SerializeField] GameObject scrollLvL3;
    [SerializeField] GameObject rankingAll;
    [SerializeField] GameObject scrollAll;

    [SerializeField] GameObject row;

    private FirebaseFirestore firestore;

    private List<Levels> ranking = new List<Levels>();

    private List<(string, int)> listLvL1 = new List<(string, int)>();
    private List<(string, int)> listLvL2 = new List<(string, int)>();
    private List<(string, int)> listLvL3 = new List<(string, int)>();
    private List<(string, int)> listLvLAll = new List<(string, int)>();

    private bool rankingCreated = false;

    private GameObject currentRanking;


    public void GoToLvL1()
    {
        currentRanking.SetActive(false);
        rankingLvL1.SetActive(true);
        currentRanking = rankingLvL1;
        buttonLvL1.GetComponent<Button>().interactable = false;

        buttonLvL3.GetComponent<Button>().interactable = true;
        buttonLvL2.GetComponent<Button>().interactable = true;
        buttonAll.GetComponent<Button>().interactable = true;
    }

    public void GoToLvL2()
    {
        currentRanking.SetActive(false);
        rankingLvL2.SetActive(true);
        currentRanking = rankingLvL2;
        buttonLvL2.GetComponent<Button>().interactable = false;

        buttonLvL1.GetComponent<Button>().interactable = true;
        buttonLvL3.GetComponent<Button>().interactable = true;
        buttonAll.GetComponent<Button>().interactable = true;
    }

    public void GoToLvL3()
    {
        currentRanking.SetActive(false);
        rankingLvL3.SetActive(true);
        currentRanking = rankingLvL3;
        buttonLvL3.GetComponent<Button>().interactable = false;

        buttonLvL1.GetComponent<Button>().interactable = true;
        buttonLvL2.GetComponent<Button>().interactable = true;
        buttonAll.GetComponent<Button>().interactable = true;
    }

    public void GoToAll()
    {
        currentRanking.SetActive(false);
        rankingAll.SetActive(true);
        currentRanking = rankingAll;
        buttonAll.GetComponent<Button>().interactable = false;

        buttonLvL1.GetComponent<Button>().interactable = true;
        buttonLvL2.GetComponent<Button>().interactable = true;
        buttonLvL3.GetComponent<Button>().interactable = true;
    }

    private void Awake()
    {
        firestore = FirebaseFirestore.DefaultInstance;

        GetRankings();
    }

    private void OnEnable()
    {
        rankingLvL1.SetActive(true);
        currentRanking = rankingLvL1;
        buttonLvL1.GetComponent<Button>().interactable = false;

        rankingLvL2.SetActive(false);
        buttonLvL2.GetComponent<Button>().interactable = true;

        rankingLvL3.SetActive(false);
        buttonLvL3.GetComponent<Button>().interactable = true;

        rankingAll.SetActive(false);
        buttonAll.GetComponent<Button>().interactable = true;
    }

    private void InitializeRankings()
    {
        if (ranking.Count == 0 || rankingCreated)
            return;

        for (int i = 0; i < ranking.Count; i++)
        {
            Levels levels = ranking[i];

            int lvlsFinished = 0;
            int timeSum = 0;

            if ((bool)levels.Level1["Finished"])
            {
                lvlsFinished += 1;
                var time = (int)System.Convert.ChangeType(levels.Level1["FinishTime"], typeof(int));
                timeSum += time;
                listLvL1.Add((levels.NickName, time));
            }

            if ((bool)levels.Level2["Finished"])
            {
                lvlsFinished += 1;
                var time = (int)System.Convert.ChangeType(levels.Level2["FinishTime"], typeof(int));
                timeSum += time;
                listLvL2.Add((levels.NickName, time));
            }

            if ((bool)levels.Level3["Finished"])
            {
                lvlsFinished += 1;
                var time = (int)System.Convert.ChangeType(levels.Level3["FinishTime"], typeof(int));
                timeSum += time;
                listLvL3.Add((levels.NickName, time));
            }

            if (lvlsFinished == 3)
            {
                listLvLAll.Add((levels.NickName, timeSum));
            }
        }

        listLvL1.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        listLvL2.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        listLvL3.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        listLvLAll.Sort((x, y) => x.Item2.CompareTo(y.Item2));

        DisplayRankings();
        rankingCreated = true;
    }

    private void DisplayRankings()
    {
        string name = "";
        string time = "";
        for(int i=0; i < listLvL1.Count; i++)
        {
            name = $"{i + 1}\t{listLvL1[i].Item1}";
            time = FormatTime(listLvL1[i].Item2);
            AddToRanking(scrollLvL1, name, time);
        }

        for (int i = 0; i < listLvL2.Count; i++)
        {
            name = $"{i + 1}\t{listLvL2[i].Item1}";
            time = FormatTime(listLvL2[i].Item2);
            AddToRanking(scrollLvL2, name, time);
        }

        for (int i = 0; i < listLvL3.Count; i++)
        {
            name = $"{i + 1}\t{listLvL3[i].Item1}";
            time = FormatTime(listLvL3[i].Item2);
            AddToRanking(scrollLvL3, name, time);
        }

        for (int i = 0; i < listLvLAll.Count; i++)
        {
            name = $"{i + 1}\t{listLvLAll[i].Item1}";
            time = FormatTime(listLvLAll[i].Item2);
            AddToRanking(scrollAll, name, time);
        }
    }

    private string FormatTime(int timeSeconds)
    {
        int hours = timeSeconds / 3600;
        int minutes = (timeSeconds % 3600) / 60;
        int seconds = (timeSeconds % 3600) % 60;

        var hoursS = $"{hours}";
        if (hours < 10)
            hoursS = "0" + hoursS;
        var minutesS = $"{minutes}";
        if (minutes < 10)
            minutesS = "0" + minutesS;
        var secondsS = $"{seconds}";
        if (seconds < 10)
            secondsS = "0" + secondsS;
        return $"{hoursS}:{minutesS}:{secondsS}";
    }

    private void AddToRanking(GameObject ranking, string name, string value)
    {
        var newRow = Instantiate(row, ranking.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);

        GameObject nameObject = GetChildWithName(newRow, "Name");
        GameObject valueObject = GetChildWithName(newRow, "Time");

        nameObject.GetComponent<TextMeshProUGUI>().text = name;
        valueObject.GetComponent<TextMeshProUGUI>().text = value;
    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }


    private void GetRankings()
    {
        firestore.Collection("levels").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot capitalQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
            {
                var levels = documentSnapshot.ConvertTo<Levels>();
                ranking.Add(levels);
            }
            rankingCreated = false;
            InitializeRankings();
        });
    }
}
