using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);

        Vector3 newRot = transform.rotation.eulerAngles;
        newRot.x = 0f;
        newRot.z = 0f;

        transform.rotation = Quaternion.Euler(newRot);
    }
}
