using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugManager : MonoBehaviour
{
    [SerializeField] GameObject idol;
    [SerializeField] GameObject[] brokenObjects;
    [SerializeField] GameObject mrRaccoon;
    [SerializeField] Conversation success;
    [SerializeField] GameObject blood;
    [SerializeField] GameObject bugObject;
    [SerializeField] RuntimeAnimatorController eyesAnimation;
    [SerializeField] GameObject bugs;
    [SerializeField] Material redMat;
    [SerializeField] Material defaultMat;
    [SerializeField] int StartingQuests;
    [SerializeField] string QuestName;
    [SerializeField] string newQuestName;

    int activeQuests;
    bool started;
    bool ended;

    // Start is called before the first frame update
    void Start()
    {
        activeQuests = brokenObjects.Length + 1;
        started = false;
        ended = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.questList.Count <= StartingQuests - 3 && !started)
        {
            BeginQuest();
        }
        else if (activeQuests <= 0 && started && !ended)
        {
            EndQuest();
        }
    }

    void BeginQuest()
    {
        GameManager.Instance.AddQuest(QuestName);

        started = true;

        bugs.SetActive(true);

        GameObject bug;
        TryDelete newDelete;
        
        foreach (GameObject broken in brokenObjects)
        {
            broken.GetComponent<SpriteRenderer>().material = redMat;

            bug = GameObject.Instantiate(bugObject);
            bug.transform.SetParent(broken.transform);
            bug.transform.localPosition = Vector3.zero;

            newDelete = broken.AddComponent(typeof(TryDelete)) as TryDelete;
            newDelete.deletable = bug;
            newDelete.bugQuest = this;
            newDelete.MrRaccoon = false;
        }

        mrRaccoon.GetComponent<SpriteRenderer>().material = redMat;

        bug = Instantiate(bugObject);
        bug.transform.SetParent(mrRaccoon.transform);
        bug.transform.localPosition = Vector3.zero;

        newDelete = mrRaccoon.AddComponent(typeof(TryDelete)) as TryDelete;
        newDelete.deletable = bug;
        newDelete.bugQuest = this;
        newDelete.MrRaccoon = true;

        DebugRaccoon newDebug = mrRaccoon.AddComponent<DebugRaccoon>();
        newDebug.bugQuest = this;
    }

    void EndQuest()
    {
        ended = true;

        bugs.SetActive(false);

        GameManager.Instance.RemoveQuest(QuestName);

        idol.GetComponent<Blaspheme>().enabled = true;

        GameManager.Instance.AddQuest(newQuestName);
    }

    public void DeleteBug(GameObject fixedObj)
    {
        fixedObj.GetComponent<SpriteRenderer>().material = defaultMat;

        fixedObj.GetComponent<TryDelete>().enabled = false;

        activeQuests--;
    }

    public void DeleteMrRaccoon()
    {
        activeQuests--;

        GameManager.Instance.PinkIndex++;

        mrRaccoon.GetComponent<TryDelete>().enabled = false;
        mrRaccoon.GetComponent<DebugRaccoon>().enabled = false;

        bugs.GetComponent<Animator>().runtimeAnimatorController = eyesAnimation;

        //GameManager.Instance.RemoveQuest(QuestName);

        //GameManager.Instance.AddQuest(newQuestName);
    }

    public void DebugMrRaccoon()
    {
        activeQuests--;

        mrRaccoon.GetComponent<SpriteRenderer>().material = defaultMat;

        mrRaccoon.GetComponent<TryDelete>().enabled = false;
        mrRaccoon.GetComponent<DebugRaccoon>().enabled = false;

        GameManager.Instance.DialogueManager.StartConversation(success.Lines);

        blood.GetComponent<Collect>().Open();
    }
}
