using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private const string START_ANIM_KEY = "start";
    private const string END_ANIM_KEY = "end";
    public static TransitionManager singleton { get; private set; }
    public enum TransitionState {Start, Middle, End};
    public delegate IEnumerator TransitionCallback(TransitionState state);
    [SerializeField] private float transitionTime;
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas renderCanvas;
    public static bool isTransferring { get { return singleton.renderCanvas.enabled; } }
    // Start is called before the first frame update
    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
    }
    public static void MakeTransition(TransitionCallback callback)
    {
        singleton.StartCoroutine(singleton.TransitionCoroutine(callback));
    }
    private IEnumerator TransitionCoroutine(TransitionCallback callback)
    {
        renderCanvas.enabled = true;
        yield return callback(TransitionState.Start);
        animator.Play(START_ANIM_KEY);
        yield return new WaitForSecondsRealtime(transitionTime);
        yield return callback(TransitionState.Middle);
        animator.Play(END_ANIM_KEY);
        yield return new WaitForSecondsRealtime(transitionTime);
        yield return callback(TransitionState.End);
        renderCanvas.enabled = false;
    }
}
