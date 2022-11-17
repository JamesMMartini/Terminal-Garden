using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New TextFile", menuName = "TextFile Data")]
public class TextFile_SO : ScriptableObject
{
    [TextArea(10, 200)]
    public string text;
}
