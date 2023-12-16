using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeExtentionSkript : MonoBehaviour
{
    public GameObject rope;
    bool end = false;
    GameObject child;
    public void extent(int x, int max)
    {
        if (x <= max)
        {
            child = Instantiate(rope, transform.position + (transform.forward / 10), transform.rotation);
            child.GetComponent<CharacterJoint>().connectedBody = gameObject.GetComponent<Rigidbody>();
            child.GetComponent<RopeExtentionSkript>().extent(x + 1, max);
        }
        else
            end = true;
    }
    public void delete()
    {
        if (end)
        {
            if(child != null)
            {
                CharacterJoint cj = child.GetComponent<CharacterJoint>();
                Destroy(cj);
            }
            Destroy(gameObject);
        }
        else
            child.GetComponent<RopeExtentionSkript>().delete();
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(end && other.gameObject.tag == "physobj")
        {
            child = other.gameObject;
            CharacterJoint cj = other.gameObject.AddComponent(typeof(CharacterJoint)) as CharacterJoint;
            cj.connectedBody = gameObject.GetComponent<Rigidbody>();
    //        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RopeShootSkript>().physobj = child;
        }
    }
}
