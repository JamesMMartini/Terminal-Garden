using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool walking;
    public float rotation;
    public float _verticalVelocity;

    [Header("Movement Fields")]
    [SerializeField] CharacterController character;
    [SerializeField] float speed;
    [SerializeField] float gravity;
    [SerializeField] float _terminalVelocity;

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
            Vector3 moveVec = new Vector3(0f, 0f, 0f);
            if (walking)
            {
                moveVec = transform.forward * speed;
            }

            character.Move(moveVec + new Vector3(0f, _verticalVelocity, 0f));

            if (rotation != 0f)
            {
                transform.Rotate(new Vector3(0f, rotation, 0f));
                rotation = 0f;
            }

            if (!character.isGrounded)
            {
                // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
                if (_verticalVelocity > _terminalVelocity)
                {
                    _verticalVelocity += gravity * GameManager.Instance.timeStep;
                }
            }
            else
            {
                _verticalVelocity = 0f;
            }
        }
        Debug.Log(character.isGrounded);
    }

    public void LookAt(GameObject target)
    {
        Vector3 targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        transform.LookAt(targetPos);
    }
}
