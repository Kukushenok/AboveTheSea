using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gg_walk : MonoBehaviour
{
    public float speed;
    InputSkript input;
    CharacterController control;
    Transform targetT;
    void Start()
    {
        targetT = transform.GetChild(0).GetComponent<Transform>();
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputSkript>();
        control = GetComponent<CharacterController>();
    }
    void Update()
    {
        float moveX = input.horizontal;
        float moveZ = input.vertical;
        Vector3 move_dir = Vector3.zero;
        move_dir.x = moveX;
        move_dir.z = moveZ;
        move_dir = speed * move_dir / move_dir.magnitude;
        move_dir.y = -10;
        control.Move(targetT.TransformDirection(move_dir * Time.deltaTime));
    }
}