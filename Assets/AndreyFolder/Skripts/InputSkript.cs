using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
public class InputSkript : MonoBehaviour
{
    [HideInInspector]
    public bool m1_d,m2_d,m1_u,m2,m2_u, doublem, m1,e, j,r;
    bool timegoes = false;
    double timer;
    double dtime = 0.2;

    public InputActionReference leftclickref, rightclickref, interact, tsks, reset;
    void Start(){
        leftclickref.action.performed += left;
        rightclickref.action.performed += right;
        leftclickref.action.started += left_d;
        rightclickref.action.started += right_d;
        leftclickref.action.canceled += left_u;
        rightclickref.action.canceled += right_u;
        interact.action.started += e_d;
        interact.action.canceled += e_u;
        tsks.action.started += j_d;
        tsks.action.canceled += j_u;
        reset.action.started += r_d;
        reset.action.canceled += r_u;
    }
     void r_d(InputAction.CallbackContext context){
        r = true;
    }
    void r_u(InputAction.CallbackContext context){
        r = false;
    }
    void j_d(InputAction.CallbackContext context){
        j = true;
    }
    void j_u(InputAction.CallbackContext context){
        j = false;
    }
    void e_d(InputAction.CallbackContext context){
        e = true;
    }
    void e_u(InputAction.CallbackContext context){
        e = false;
    }
    void left_d(InputAction.CallbackContext context){
        doublem = false;
        m1_d = true;
            m1 = true;
            m1_u = false;
            if(timegoes){
                if(Time.time < timer){
                    doublem = true;
                }
                timegoes = false;
            }
            if(!timegoes){
                timegoes = true;
                timer = Time.time + dtime;
            }
    }
    void left_u(InputAction.CallbackContext context){
         m1_d = false;
            m1 = false;
            m1_u = true;
    }
    void right_d(InputAction.CallbackContext context){
         m2_d = true;
            m2 = true;
            m2_u = false;
    }
    void right_u(InputAction.CallbackContext context){
         m2_d = false;
            m2 = true;
            m2_u = true;
    }
    void left(InputAction.CallbackContext context)
    {
        m1 = true;
            m1_u = false;
    }
    void right(InputAction.CallbackContext context)
    {
            m2 = true;
            m2_u = false;
    }
}