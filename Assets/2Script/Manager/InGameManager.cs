using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
	public static InGameManager instance;

	[SerializeField] private List<Region> enemySpawnRegions;

	[SerializeField] private ControlSlider progress;

	[SerializeField] private GamePlayer player;
	private static Region playerRegion;

	[SerializeField] private Image screen;
	[SerializeField] private GameObject pointLight;
	[SerializeField] private GameObject globalLight;

	[SerializeField] private Text timerText;

	private void Awake()
	{
		instance = this;

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GamePlayer>();
	}

	private void Start()
	{
		player.SetColor(GameManager.playerColor);
		//StartCoroutine(WaveStart(5f));
		player.CanMove = true;
	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.F1))
			StartCoroutine(ChangeScreen(true));
		if (Input.GetKeyDown(KeyCode.F2))
			StartCoroutine(ChangeScreen(false));
		if (Input.GetKeyDown(KeyCode.F3))
			ItemManager.SpawnItems(1);	
    }

	public static void SetPlayerRegion(Region p_region)
	{
		playerRegion = p_region;
	}

	public static Region GetPlayerRegion()
	{
		return playerRegion;
	}

	private IEnumerator SpawnEnemy(int p_num)
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < p_num; i++)
		{
			var t_regions = enemySpawnRegions;
			t_regions.Remove(playerRegion);
			var t_spawnRegion = t_regions[Random.Range(0, t_regions.Count)];
			var t_enemy = ObjectPooling.SpawnObject("EnemyNormal", Vector3.zero, Quaternion.identity).GetComponent<EnemyNormal>();
			t_enemy.SetEnemy(t_spawnRegion);
		}
	}

	private IEnumerator SpawnPortal(int p_num)
	{
		yield return new WaitForSeconds(1f);

		for (int i = 0; i < p_num; i++)
		{
			var t_regions = enemySpawnRegions;
			t_regions.Remove(playerRegion);
			var t_spawnRegion = t_regions[Random.Range(0, t_regions.Count)];
			t_regions.Remove(t_spawnRegion);
			var t_portal = ObjectPooling.SpawnObject("EnemyPortal", Vector3.zero, Quaternion.identity).GetComponent<EnemyPortal>();
			t_portal.SetEnemy(t_spawnRegion);

			t_portal.miniMapObject = MiniMapManager.SpawnObject(t_portal.transform.position, EMiniMapObject.ENEMYPORTAL);
		}
	}

	private void ChangeLight(bool p_isStart)
	{
		globalLight.SetActive(!p_isStart);
		pointLight.SetActive(p_isStart);
	}

	public static void TurnOnGlobalLight() => instance.ChangeLight(false);
	public static void TurnOnPointLight() => instance.ChangeLight(true);

	private IEnumerator ChangeScreen(bool p_isFadeIn)
	{
		float t_timer = 0f;
		float t_totalTime = 1f;
		Color t_color = screen.color;

		while (t_timer <= 1f)
		{
			t_color.a = Mathf.Lerp(p_isFadeIn ? 1f : 0f, p_isFadeIn ? 0f : 1f, t_timer / t_totalTime);
			screen.color = t_color;
			t_timer += Time.deltaTime;
			yield return null;
		}
	}

	public static void FadeIn() => instance.StartCoroutine(instance.ChangeScreen(true));
	public static void FadeOut() => instance.StartCoroutine(instance.ChangeScreen(false));

	private IEnumerator TimeCheck(float p_time)
	{
		timerText.gameObject.SetActive(true);

		var t_time = p_time;
		var t_interval = 1f;

		while (t_time > 10f)
		{
			t_time -= t_interval;
			timerText.text = t_time.ToString();

			yield return new WaitForSeconds(t_interval);
		}

		while (t_time > 0f)
		{
			t_time -= t_interval;

			timerText.text = t_time.ToString();
			var t_colorValue = Mathf.Lerp(0f, 1f, t_time / 10f);

			timerText.color = new Color(1f, t_colorValue, t_colorValue);

			yield return new WaitForSeconds(t_interval);
		}

		timerText.gameObject.SetActive(false);
	}
}
