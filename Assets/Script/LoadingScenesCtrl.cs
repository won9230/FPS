using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingScenesCtrl : MonoBehaviour
{
	static string nextScene;
	[SerializeField] Image progressBar;
	private void Start()
	{
		StartCoroutine(LoadSceneProcess());
	}
	public static void LoadScene(string sceneName)
	{
		nextScene = sceneName;
		PhotonNetwork.LoadLevel("LoadingScene");
	}
	IEnumerator LoadSceneProcess()
	{
		AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
		op.allowSceneActivation = false;

		float timer = 0f;
		while (!op.isDone)
		{
			yield return null;

			if (op.progress < 0.9f)
			{
				progressBar.fillAmount = op.progress;
			}
			else
			{
				timer += Time.unscaledDeltaTime;
				progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
				if (progressBar.fillAmount >= 1f)
				{
					op.allowSceneActivation = true;
					yield break;
				}
			}
		}
	}
}