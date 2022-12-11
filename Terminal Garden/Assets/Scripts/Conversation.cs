using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Dialog/Create New Conversation")]
public class Conversation : ScriptableObject
{
    [SerializeField] string[] lines;

    public string[] Lines { get { return lines; } set { lines = value; } }
}
