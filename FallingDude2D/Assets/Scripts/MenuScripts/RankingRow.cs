using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingRow : MonoBehaviour
{
    private void OnEnable()
    {
        var nickName = GetChildWithName("Name").GetComponent<TextMeshProUGUI>();
        var time = GetChildWithName("Time").GetComponent<TextMeshProUGUI>();

        if (nickName.text.Length == 0)
            return;

        if (CurrentUser.nickName == nickName.text.Split('\t')[1])
        {
            nickName.color = new Color32(255, 215, 0, 255);
            time.color = new Color32(255, 215, 0, 255);
        }
        else
        {
            nickName.color = new Color32(255, 255, 255, 255);
            time.color = new Color32(255, 255, 255, 255);
        }
    }

    GameObject GetChildWithName(string name)
    {
        Transform trans = transform;
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
}
