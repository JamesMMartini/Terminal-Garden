using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File : MonoBehaviour
{
    public string FileName;
    public string FileType;

    public virtual string Open()
    {
        return "EMPTY FILE";
    }
}
