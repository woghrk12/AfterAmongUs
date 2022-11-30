using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static EScene nextScene;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(EScene p_scene)
    {
        nextScene = p_scene;
        SceneManager.LoadScene((int)EScene.LOADING);
    }

    private IEnumerator LoadScene()
    {
        var t_uiManager = (LoadingUIGroup)UIManager.Instance.ActiveUI(EUIList.LOADING);
        var t_op = SceneManager.LoadSceneAsync((int)nextScene);
        var t_timer = 0f;
        
        t_op.allowSceneActivation = false;

        while (!t_op.isDone)
        {
            yield return null;

            if (t_op.progress < 0.9f) t_uiManager.SetProgress(t_op.progress);
            else
            {
                t_timer += Time.unscaledDeltaTime;
                t_uiManager.SetProgress(Mathf.Lerp(0.9f, 1f, t_timer));
                if (t_timer >= 1.0f)
                    t_op.allowSceneActivation = true;
            }
        }
    }
}
