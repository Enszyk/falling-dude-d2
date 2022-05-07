using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateCloud : MonoBehaviour
{
    [SerializeField] private float coolDown;
    [SerializeField] private Transform[] firePoint;
    [SerializeField] private GameObject[] clouds;
    private float cooldownTimer = Mathf.Infinity;
    private List<int> lastTwoIndexes = new List<int>();

    private void Update()
    {
        if (cooldownTimer > coolDown)
            CreateCloud();

        cooldownTimer += Time.deltaTime;
    }

    private void CreateCloud()
    {
        var rnd = new System.Random();
        GameObject cld= Instantiate(clouds[rnd.Next(0, clouds.Length)]);
        cooldownTimer = 0;

        cld.transform.position = firePoint[RandomIndex(firePoint.Length)].position;
        cld.GetComponent<Cloud>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int RandomIndex(int size)
    {
        var rnd = new System.Random();
        int newIndex;
        do
        {
            newIndex = rnd.Next(0, size);
        } while (lastTwoIndexes.Contains(newIndex));

        lastTwoIndexes.Add(newIndex);
        if (lastTwoIndexes.Count == 3)
            lastTwoIndexes.RemoveAt(0);
        return newIndex;
    }
}
