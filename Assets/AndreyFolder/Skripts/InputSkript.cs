using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSkript : MonoBehaviour
{
    [HideInInspector]
    public float mouseX, mouseY, horizontal, vertical;
    [HideInInspector]
    public bool jump, m1_d,m2_d;
    void Update()
    {
     //   e_1 = Input.GetButtonDown("E_1");
     //   true_jump = Input.GetButtonDown("jump_up");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    //    joystickX = Input.GetAxis("Joystick X");
    //    joystickY = Input.GetAxis("Joystick Y");
        jump = Input.GetButtonDown("Jump");
     //   jump_up = Input.GetButtonUp("Jump");
     //   jump_long = Input.GetButton("Jump");
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
     //   s2 = Input.GetButton("Fire2");
        m2_d = Input.GetButtonDown("Fire2");
       // shift = Input.GetButton("Shift");
      //  caps = Input.GetButtonDown("caps");
        m1_d = Input.GetButtonDown("Fire1");
        //s1_l = Input.GetButton("Fire1");
        //s1_u = Input.GetButtonUp("Fire1");
        //knive = Input.GetButtonDown("Knive");
        //follow = Input.GetButtonDown("Fire3");
        //float R2 = Input.GetAxis("R2") * (-10);
        //bool R2_D = false;
        //if (R2 > 0.8f && !input_r2)
        //{
        //    input_r2 = true;
        //    R2_D = true;
        //}
        //if (R2 <= -0.9f && input_r2)
        //    input_r2 = false;
        //crossbow_d = (Input.GetButtonDown("Crossbow") || R2_D);
        //crossbow_u = Input.GetButtonUp("Crossbow");
        //bomb = Input.GetKey(KeyCode.LeftAlt);
        //ctrl = (Input.GetButtonDown("Ctrl") || Input.GetKeyDown(KeyCode.LeftControl));
    }
}
