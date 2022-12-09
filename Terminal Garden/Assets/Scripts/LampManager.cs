using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampManager : MonoBehaviour
{
    [SerializeField] int totalLamps;
    [SerializeField] string QuestName;

    public int lampsOn;

    private void Start()
    {
        GameManager.Instance.AddQuest(QuestName);
        lampsOn = 0;
    }

    public void LampOn(bool pink)
    {
        if (pink)
        {
            GameManager.Instance.PinkIndex++;
        }
        else
        {
            lampsOn++;
        }

        if (lampsOn >= totalLamps)
            GameManager.Instance.RemoveQuest(QuestName);
    }

    public void LampOff(bool pink)
    {
        if (pink)
        {
            GameManager.Instance.PinkIndex--;
        }
        else
        {
            lampsOn--;
        }
    }
}
