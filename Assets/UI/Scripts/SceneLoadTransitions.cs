using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class SceneLoadTransitions
{
    public const int SCENE_MAINMENU = 0;
    public const int SCENE_GAMEPLAY = 1;
    private static int currentSceneLoadIndex = -1;
    public static void LoadScene(int sceneIdx)
    {
        if (TransitionManager.singleton == null)
        {
            LoadSceneImmediate(sceneIdx);
            return;
        }
        if (currentSceneLoadIndex > 0) return;
        currentSceneLoadIndex = sceneIdx;
        TransitionManager.MakeTransition(SceneTransitionEnumerator);
    }
    public static void LoadSceneImmediate(int sceneIdx)
    {
        SceneManager.LoadScene(sceneIdx);
    }
    private static IEnumerator SceneTransitionEnumerator(TransitionManager.TransitionState state)
    {
        if (state == TransitionManager.TransitionState.Start)
        {
            //Time.timeScale = 0;
        }
        else if (state == TransitionManager.TransitionState.Middle)
        {
            AsyncOperation loading = SceneManager.LoadSceneAsync(currentSceneLoadIndex);
            while (!loading.isDone)
            {
                yield return null;
            }
            currentSceneLoadIndex = -1;
        }
        else
        {
            //Time.timeScale = 1;
        }
    }
}
