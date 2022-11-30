using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static EScene nextScene;
    private LoadingUI loadingUI = null;

    private void Start()
    {
        loadingUI = UIManager.Instance.LoadingUI;
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(EScene p_scene)
    {
        nextScene = p_scene;
        SceneManager.LoadScene((int)EScene.LOADING);
    }

    private IEnumerator LoadScene()
    {
        var t_op = SceneManager.LoadSceneAsync((int)nextScene);
        t_op.allowSceneActivation = false;

        var t_timer = 0f;

        while (!t_op.isDone)
        {
            yield return null;

            if (t_op.progress < 0.9f) loadingUI.SetProgress(t_op.progress);
            else
            {
                t_timer += Time.unscaledDeltaTime;
                loadingUI.SetProgress(Mathf.Lerp(0.9f, 1f, t_timer));
                if (t_timer >= 1.0f)
                    t_op.allowSceneActivation = true;
            }
        }
    }
}
