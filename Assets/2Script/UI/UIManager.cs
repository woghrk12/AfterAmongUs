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

	[SerializeField] private TitleUI titleUI = null;
	[SerializeField] private InGameUI inGameUI = null;
	//[SerializeField] private GameObject loadingUI = null;
	[SerializeField] private FadeUI fadeUI = null;
	[SerializeField] private CommonUI commonUI = null;
	public TitleUI TitleUI { get { return uiList[(int)EUIList.TITLE].GetComponent<TitleUI>(); } }
	public FadeUI FadeUI { get { return uiList[(int)EUIList.FADE].GetComponent<FadeUI>(); } }
	public InGameUI InGameUI { get { return uiList[(int)EUIList.INGAME].GetComponent<InGameUI>(); } }

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
    }

    public void ActiveUI(EUIList p_uiIdx)
	{
		for (int i = 0; i < uiList.Count; i++)
		{ 
		
		}
		switch (p_uiIdx)
		{
			case EUIList.TITLE:
				break;
			case EUIList.FADE:
				break;
			case EUIList.INGAME:
				break;
			case EUIList.LOADING:
				break;
			default:
				break;
		}
	}

	public static void Alert(string p_text) => Instance.commonUI.Alert(p_text);
}

