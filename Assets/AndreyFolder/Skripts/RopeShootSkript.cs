using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeShootSkript : MonoBehaviour
{
    public GameObject obj;
    public Transform instpos, orig;
    InputSkript input;
    GameObject child;
    bool shoot = false;
    Vector3 pos;
    public float back_speed;
    bool back;
    void Start()
    {
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputSkript>();
    }
    void Update()
    {
        if (input.m1_d && shoot)
            back = !back;
        if(back)
        {
            instpos.position = instpos.position - instpos.forward * back_speed * Time.deltaTime;
        }
        if (input.m1_d && !shoot)
        {
            child = Instantiate(obj, instpos.position, transform.rotation);
        //    newobj.GetComponent<Rigidbody>().AddForce(newobj.transform.forward * 500);
            child.GetComponent<CharacterJoint>().connectedBody = instpos.GetComponent<Rigidbody>();
            child.GetComponent<RopeExtentionSkript>().extent(0, 30);
            shoot = true;
        }
        if (input.m2_d && shoot)
        {
            back = false;
            instpos.position = orig.position;
            child.GetComponent<RopeExtentionSkript>().delete();
            shoot = false;
        }
      
    }
}
