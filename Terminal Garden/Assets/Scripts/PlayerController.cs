using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool walking;
    public float rotation;

    [Header("Movement Fields")]
    [SerializeField] CharacterController character;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        walking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.RunUpdate) // It is time for the objects to update
        {
            if (walking)
            {
                character.Move(transform.forward * speed);
            }

            if (rotation != 0f)
            {
                transform.Rotate(new Vector3(0f, rotation, 0f));
                rotation = 0f;
            }
        }
    }

    public void LookAt(GameObject target)
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        transform.LookAt(targetPos);
    }
}
