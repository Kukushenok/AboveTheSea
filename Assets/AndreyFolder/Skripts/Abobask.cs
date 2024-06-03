using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Abobask : MonoBehaviour
{
    public float force, maxforce;
    public GameObject sphere;
    public float distance_to_player, distance_between_nods;
    public LayerMask collMask;
    public GameObject player;
    public List<GameObject> objlist = new List<GameObject>();
    List<Vector3> ropePositions = new List<Vector3>();
    public bool attract = false;
    LineRenderer rope;
 //   [HideInInspector]
    public GameObject attractionobj1, attractionobj2;
    InputSkript input;
    void Update()
    {
        if(attract && attractionobj2 != null){
            
            Vector3 direction1, direction2;
            float dist1, dist2;
            dist1 = Vector3.Distance(objlist[1].transform.position, objlist[0].transform.position);
            dist2 = Vector3.Distance(objlist[objlist.Count - 2].transform.position, objlist[objlist.Count-1].transform.position);
            direction1 = objlist[1].transform.position - objlist[0].transform.position;
            direction2 = objlist[objlist.Count - 2].transform.position - objlist[objlist.Count-1].transform.position;
            float currforce1 = Math.Min(dist1 * Time.deltaTime * 1000 * force, maxforce * Time.deltaTime * 1000);
            float currforce2 = Math.Min(dist2 *Time.deltaTime * 1000 *force,  maxforce * Time.deltaTime * 1000);
            attractionobj1.GetComponent<Rigidbody>().AddForceAtPosition(direction1.normalized * currforce1, objlist[0].transform.position);
            attractionobj2.GetComponent<Rigidbody>().AddForceAtPosition(direction2.normalized * currforce2, objlist[objlist.Count-1].transform.position);
      //    attractionobj.GetComponent<Rigidbody>().AddForceAtPosition(direction.normalized * 10, attractionobj.transform.position);
            if(objlist.Count == 2 && Vector3.Distance(objlist[1].transform.position, objlist[0].transform.position) <= distance_between_nods){
                attract = false;
                Destroy(objlist[0]);
                Destroy(objlist[1]);
                Destroy(gameObject);
            }
            if(input.doublem && attract && (Vector3.Distance(player.transform.position, objlist[objlist.Count - 1].transform.position) <= distance_to_player || Vector3.Distance(player.transform.position, objlist[0].transform.position) <= distance_to_player)){
                input.doublem = false;
                attract = false;
                for(int i = 0; i < objlist.Count; ++i)
                    Destroy(objlist[i]);
                Destroy(gameObject);

            }
             
        }
        if(attract){
            UpdateRopePositions();
        LastSegmentGoToinstposPos();
        DetectCollisionEnter();
        if (objlist.Count > 2) DetectCollisionExits();
        }
    }
    private void DetectCollisionEnter()
    {
        for(int i = 0; i < objlist.Count - 1; ++i){
            RaycastHit hit;
            if (Physics.Linecast(objlist[i].transform.position, objlist[i+1].transform.position, out hit, collMask))
            {
             bool b = true;
             for(int j = 0; j < objlist.Count; ++j){
                 if(Vector3.Distance(hit.point, objlist[j].transform.position) < 0.1f){
                        b = false;
                        break;
                 }
                 
             }
             if(b){
                GameObject newobj = Instantiate(sphere, hit.point, Quaternion.identity);
                newobj.transform.parent = hit.collider.transform;
                objlist.Insert(i+1, newobj);
              }
            }
        }
       
    }
    private void UpdateRopePositions()
    {
        ropePositions.Clear();
        for(int i = 0; i < objlist.Count; ++i)
            ropePositions.Add(objlist[i].transform.position);
        rope.positionCount = ropePositions.Count;
        rope.SetPositions(ropePositions.ToArray());
    }
    private void LastSegmentGoToinstposPos() => rope.SetPosition(rope.positionCount - 1, objlist[objlist.Count-1].transform.position);
     private void DetectCollisionExits()
    {
        RaycastHit hit;
        if (!Physics.Linecast(objlist[objlist.Count - 3].transform.position,objlist[objlist.Count - 1].transform.position,  out hit, collMask))
        {
            if(Math.Abs(Vector3.Distance(objlist[objlist.Count - 2].transform.position, objlist[objlist.Count - 1].transform.position) + 
                        Vector3.Distance(objlist[objlist.Count - 2].transform.position, objlist[objlist.Count - 3].transform.position) - 
                        Vector3.Distance(objlist[objlist.Count - 3].transform.position, objlist[objlist.Count - 1].transform.position))  < 0.1f){
            GameObject tmp = objlist[objlist.Count - 2];
            objlist.RemoveAt(objlist.Count - 2);
            Destroy(tmp);
                        }
        }
        if(objlist.Count > 2){
            RaycastHit hit1;
        if (!Physics.Linecast(objlist[0].transform.position,objlist[2].transform.position,  out hit1, collMask))
        {
            if(Math.Abs(Vector3.Distance(objlist[0].transform.position, objlist[1].transform.position) + 
                        Vector3.Distance(objlist[1].transform.position, objlist[2].transform.position) - 
                        Vector3.Distance(objlist[0].transform.position, objlist[2].transform.position))  < 0.1f){
            GameObject tmp = objlist[1];
            objlist.RemoveAt(1);
            Destroy(tmp);
                        }
        }
        }
        
        if(objlist.Count > 2){
            if(Vector3.Distance(objlist[0].transform.position, objlist[1].transform.position) < distance_between_nods){
            GameObject tmp = objlist[1];
            objlist.RemoveAt(1);
            Destroy(tmp);
            ropePositions.RemoveAt(1);
        }
        }
        if(objlist.Count > 2){
if(Vector3.Distance(objlist[objlist.Count - 1].transform.position, objlist[objlist.Count - 2].transform.position) < distance_between_nods){
            GameObject tmp = objlist[objlist.Count - 2];
            objlist.RemoveAt(objlist.Count - 2);
            Destroy(tmp);
        }
        }
        
    }
    public void startfunc(){
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputSkript>();
        attract = true;
        rope = GetComponent<LineRenderer>();
        if(objlist[0].transform.parent == null){
            attractionobj1 = objlist[0];
        }
        else{
            if(objlist[0].transform.parent.tag == "physobj"){
                 attractionobj1 = objlist[0].transform.parent.gameObject;
            }
            else{
                attractionobj1 = objlist[0];
            }
        }
       
        attractionobj2 = null;
    }
}
