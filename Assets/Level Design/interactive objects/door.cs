using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class door : MonoBehaviour
{
    private bool _open = false;
    [SerializeField] private bool _in = false;
    private Animator _animator;

    public void Open() {
        _animator.SetBool("in", true);
        _animator.SetBool("open", !_open);
        _open = !_open;
    }

    // это временное решение
    private void OnMouseDown()
    {
        Open();
    }
}
