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

	[SerializeField] private List<GameObject> uiList = null;

	private TitleUI titleUI = null; 
	private InGameUI inGameUI = null;
	private LoadingUI loadingUI = null;
	private FadeUI fadeUI = null;
	private CommonUI commonUI = null;

	public TitleUI TitleUI { get { return titleUI; } }
	public InGameUI InGameUI { get { return inGameUI; } }
	public LoadingUI LoadingUI { get { return loadingUI; } }
	public FadeUI FadeUI { get { return fadeUI; } }
	public CommonUI CommonUI { get { return commonUI; } }
	public JoyStick Joystick { get { return TitleUI.JoyStick; } }
	public Button UseButton { get { return TitleUI.UseButton; } }

	private void Awake()
	{
		var t_objs = FindObjectsOfType<UIManager>();

		if (t_objs.Length != 1)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		titleUI = uiList[(int)EUIList.TITLE].GetComponent<TitleUI>();
		inGameUI = uiList[(int)EUIList.INGAME].GetComponent<InGameUI>();
		fadeUI = uiList[(int)EUIList.FADE].GetComponent<FadeUI>();
		loadingUI = uiList[(int)EUIList.LOADING].GetComponent<LoadingUI>();
		commonUI = uiList[(int)EUIList.COMMON].GetComponent<CommonUI>();
	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.F1))
		{
			inGameUI.gameObject.SetActive(false);
			titleUI.gameObject.SetActive(true);
			titleUI.InitUI();
		}
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			titleUI.gameObject.SetActive(false);
			inGameUI.gameObject.SetActive(true);
			inGameUI.InitUI();
		}
		else if (Input.GetKeyDown(KeyCode.F3))
			LoadingManager.LoadScene(EScene.INGAME);
    }

    public void ActiveUI(EUIList p_uiIdx)
	{
		var t_cnt = (int)EUIList.COMMON;
		var t_idx = (int)p_uiIdx;

		for (int i = 0; i < t_cnt; i++)
		{
			uiList[i].SetActive(t_idx == i);				
		}
	}

	public static void Alert(string p_text) => Instance.commonUI.Alert(p_text);
}

