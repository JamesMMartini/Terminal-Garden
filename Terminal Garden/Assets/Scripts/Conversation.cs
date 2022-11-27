using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Dialog/Create New Conversation")]
public class Conversation : ScriptableObject
{
    [Multiline(20)] [SerializeField] string lines;

    public string[] Lines { get { return lines.Split(System.Environment.NewLine); } set { Lines = value; } }
}
