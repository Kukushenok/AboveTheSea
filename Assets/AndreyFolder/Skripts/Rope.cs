using System.Collections.Generic;
using UnityEngine;
using System;

public class Rope : MonoBehaviour
{
    public GameObject hook;
    public GameObject sphere;
    public Transform instpos;
    public GameObject aboba;
    public LineRenderer rope;
    public Abobask ropesk;
    public LayerMask collMask,mask;

    public List<Vector3> ropePositions { get; set; } = new List<Vector3>();
    public List<GameObject> objlist = new List<GameObject>();
    bool shoot = false;
    InputSkript input;
    bool attract = false;
    public GameObject attractionobj;
     void Start()
    {
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputSkript>();
    }
    private void Update()
    {
        
        if(input.m1 && !input.doublem && shoot && objlist[0].transform.parent != null){
            attract = true;
            attractionobj = objlist[0].transform.parent.gameObject;
        }
        if(input.m1_u && attract){
            attract = false;
        }
        if(attract){
            Vector3 direction;
            if(objlist.Count > 1){
                direction = objlist[1].transform.position -  objlist[0].transform.position;
            }
            else{
                direction = instpos.position - objlist[0].transform.position;
            }
            attractionobj.GetComponent<Rigidbody>().AddForceAtPosition(direction.normalized * 10, objlist[0].transform.position);
      //    attractionobj.GetComponent<Rigidbody>().AddForceAtPosition(direction.normalized * 10, attractionobj.transform.position);
            if(objlist.Count == 1 && Vector3.Distance(instpos.position, objlist[0].transform.position) <= 0.1f){
                attract = false;
            }
        }
        if(input.doublem && shoot)
        {
            Destroy(rope.gameObject);
            shoot = false;
            ropePositions.Clear();
            for(int i = 0; i < objlist.Count; ++i)
                Destroy(objlist[i]);
            objlist.Clear();
        }
        else{
             if(input.m1_d && !shoot){
           // RaycastHit hit;
         //   if (Physics.Raycast(instpos.position, instpos.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, collMask)){
                shoot = true;
                GameObject newobj = Instantiate(aboba, transform.position, Quaternion.identity);
                ropesk = newobj.GetComponent<Abobask>();
                rope = newobj.GetComponent<LineRenderer>();
                Vector3 direction = instpos.forward;
                GameObject newhook = Instantiate(hook, instpos.position, Quaternion.LookRotation(direction));
                newhook.GetComponent<Hooksk>().dir = direction;
                newhook.GetComponent<Hooksk>().second = false;
                 newhook.GetComponent<Hooksk>().ropesk = ropesk;
                  newhook.GetComponent<Hooksk>().func();
                objlist.Add(newhook);
        //        ropePositions.Add(hit.point);
           //     ropePositions.Add(instpos.position); //Always the last pos must be the instpos
         //   }
             }
        }
       
        if(input.m2_d && shoot){
          //  RaycastHit hit;
        //    if (Physics.Raycast(instpos.position, instpos.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, collMask)){
                shoot = false;
                ropePositions.Clear();
                Vector3 direction = instpos.forward;
                GameObject newhook = Instantiate(hook, instpos.position, Quaternion.LookRotation(direction));
                newhook.GetComponent<Hooksk>().dir = direction;
                newhook.GetComponent<Hooksk>().second = true;
                newhook.GetComponent<Hooksk>().ropesk = ropesk;
                newhook.GetComponent<Hooksk>().func();
                objlist.Add(newhook);
                for(int i = 0; i < objlist.Count; ++i)
                    ropesk.objlist.Add(objlist[i]);
                objlist.Clear();
                ropesk.player = gameObject;
                ropesk.startfunc();
      //      }
        }
        if(shoot){
            UpdateRopePositions();
        LastSegmentGoToinstposPos();

        DetectCollisionEnter();
        if (objlist.Count > 1) DetectCollisionExits();
        }        
    }

    private void DetectCollisionEnter()
    {
        RaycastHit hit;
        if (Physics.Linecast(instpos.position, rope.GetPosition(ropePositions.Count - 2), out hit, collMask))
        {
            bool b = true;
            for(int i = 0; i < ropePositions.Count; ++i){
                if(Vector3.Distance(hit.point, ropePositions[i]) < 0.1f){
                    b = false;
                    break;
                }
            }
            if(b){
                ropePositions.RemoveAt(ropePositions.Count - 1);
                AddPosToRope(hit.point, hit.collider.gameObject);
            }
        }
    }

    private void DetectCollisionExits()
    {
        RaycastHit hit;
        Debug.DrawRay(objlist[objlist.Count - 2].transform.position, instpos.position - objlist[objlist.Count - 2].transform.position, Color.green);
        if (!Physics.Linecast(objlist[objlist.Count - 2].transform.position,instpos.position,  out hit, collMask))
        {
            if(Math.Abs(Vector3.Distance(objlist[objlist.Count - 2].transform.position, objlist[objlist.Count - 1].transform.position) + 
        Vector3.Distance(objlist[objlist.Count - 1].transform.position, instpos.position) - 
        Vector3.Distance(objlist[objlist.Count - 2].transform.position, instpos.position))  < 0.1f){
            GameObject tmp = objlist[objlist.Count - 1];
            objlist.RemoveAt(objlist.Count - 1);
            Destroy(tmp);
            ropePositions.RemoveAt(ropePositions.Count - 2);
        }
        }
        if(attract && objlist.Count > 1){
            if(Vector3.Distance(objlist[0].transform.position, objlist[1].transform.position) < 0.1f){
                GameObject tmp = objlist[1];
            objlist.RemoveAt(1);
            Destroy(tmp);
            ropePositions.RemoveAt(1);
            }
        }
        
        
    }

    private void AddPosToRope(Vector3 _pos, GameObject obj)
    {
         GameObject newobj;
        if(obj != null){
            newobj = Instantiate(sphere, _pos, Quaternion.identity);
            newobj.transform.parent = obj.transform;
        }
        else{
            newobj = Instantiate(sphere, _pos, Quaternion.identity);
        }
        objlist.Add(newobj);
        ropePositions.Add(_pos);
        ropePositions.Add(instpos.position); //Always the last pos must be the instpos
    }

    private void UpdateRopePositions()
    {
        ropePositions.Clear();
        for(int i = 0; i < objlist.Count; ++i)
            ropePositions.Add(objlist[i].transform.position);
        ropePositions.Add(instpos.position);
        rope.positionCount = ropePositions.Count;
        rope.SetPositions(ropePositions.ToArray());
    }
    private void LastSegmentGoToinstposPos() => rope.SetPosition(rope.positionCount - 1, instpos.position);
}