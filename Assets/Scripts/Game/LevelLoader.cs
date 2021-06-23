using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Biweekly
{
	public sealed class LevelLoader : MonoBehaviour
	{
		private static LevelLoader _instance = null;

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				DontDestroyOnLoad(this);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void ReloadCurrentLevel()
		{
			int index = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(index);
		}

		private static void LoadNextLevel()
		{
			int index = SceneManager.GetActiveScene().buildIndex + 1;
			if (index >= SceneManager.sceneCount)
			{
				return;
			}
			SceneManager.LoadScene(index);
		}

		public void ExitCurrentLevel()
		{
			LoadNextLevel();
		}
	}
}
