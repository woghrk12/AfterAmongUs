using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private static UIManager instance = null;
	public static UIManager Instance
	{
		get
		{
			if (instance == null)
			{
				var t_obj = FindObjectOfType<UIManager>();

				if (t_obj != null)
				{
					instance = t_obj;
				}
				else
				{
					var t_newObj = new GameObject().AddComponent<UIManager>();
					instance = t_newObj;
				}
			}
			return instance;
		}
	}

	[SerializeField] private List<UIGroup> uiList = null;

	public TitleUIGroup TitleUI { private set; get; }
	public InGameUIGroup InGameUI { private set; get; }
	public LoadingUIGroup LoadingUI { private set; get; }
	public FadeUIGroup FadeUI { private set; get; }
	public CommonUIGroup CommonUI { private set; get; }
	public JoyStick Joystick { set; get; }
	public Button UseButton { set; get; }

	private void Awake()
	{
		var t_objs = FindObjectsOfType<UIManager>();

		if (t_objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		TitleUI = (TitleUIGroup)uiList[(int)EUIList.TITLE];
		InGameUI = (InGameUIGroup)uiList[(int)EUIList.INGAME];
		LoadingUI = (LoadingUIGroup)uiList[(int)EUIList.LOADING];
		FadeUI = (FadeUIGroup)uiList[(int)EUIList.FADE];
		CommonUI = (CommonUIGroup)uiList[(int)EUIList.COMMON];
	}

    public void ActiveUI(EUIList p_uiIdx)
	{
		var t_cnt = (int)EUIList.COMMON;
		var t_idx = (int)p_uiIdx;

		for (int i = 0; i < t_cnt; i++) uiList[i].gameObject.SetActive(i == t_idx);
	}

	public static void Alert(string p_text) => Instance.CommonUI.Alert(p_text);
}

