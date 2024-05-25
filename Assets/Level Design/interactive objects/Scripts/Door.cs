using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string ANIMATION_OPEN_IN_NAME;
    [SerializeField] private string ANIMATION_OPEN_OUT_NAME;
    [SerializeField] private string ANIMATION_CLOSE_IN_NAME;
    [SerializeField] private string ANIMATION_CLOSE_OUT_NAME;
    [SerializeField] private ButtonObject[] _buttonObjects;

    private bool _open = false;
    private enum Direction {none, outd, ind };
    private Direction direction = Direction.outd;

    
    private Animator _animator;

    private void Awake()
    {
        foreach (ButtonObject btnobj in _buttonObjects)
        {
            btnobj.Interacted += OnInteract;
        }
        _animator = GetComponent<Animator>();
    }


    

    public void OnInteract(Vector3 interactorPosition) {
        if(transform.InverseTransformPoint(interactorPosition).x < 0 && _open==false) { direction = Direction.outd; }
        else if (transform.InverseTransformPoint(interactorPosition).x >= 0 && _open == false) { direction = Direction.ind; }

        if (_open==true && direction == Direction.outd && ANIMATION_CLOSE_OUT_NAME != "") { _animator.Play(ANIMATION_CLOSE_OUT_NAME, 0, 0.0f); _open = false; Debug.Log("close out"); } //todo
        else if (_open==true && direction == Direction.ind && ANIMATION_CLOSE_IN_NAME != "") { _animator.Play(ANIMATION_CLOSE_IN_NAME, 0, 0.0f); _open = false; } //todo
        else if (_open==false && direction == Direction.outd && ANIMATION_OPEN_OUT_NAME != "") { _animator.Play(ANIMATION_OPEN_OUT_NAME, 0, 0.0f); _open = true; Debug.Log("open out"); } //todo
        else if (_open==false && direction == Direction.ind && ANIMATION_OPEN_IN_NAME != "") { _animator.Play(ANIMATION_OPEN_IN_NAME, 0, 0.0f); _open = true; } //todo
    }
    
}
