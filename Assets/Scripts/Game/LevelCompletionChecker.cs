using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Biweekly
{
	public sealed class LevelCompletionChecker : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent _onLevelComplete = null;
		[SerializeField]
		private UnityEvent _onLevelFinished = null;
		[SerializeField]
		private UnityEvent _onGameFinished = null;

		private int _levelTiles = 0;
		private bool _ended = false;
		private bool _isLastScene = false;

		private void Awake()
		{
			_levelTiles = GetComponentsInChildren<IceTile>().Length;
		}

		private void Start()
		{
			_isLastScene = SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1;
		}

		public void OnBrokeTile()
		{
			_levelTiles--;
		}

		public void LevelCompletionCheck()
		{
			if (_ended || _levelTiles > 1) return;
			
			_ended = true;
			_onLevelComplete.Invoke();
		}

		public void FinishLevel()
		{
			if(_isLastScene)
				_onGameFinished.Invoke();
			else
				_onLevelFinished.Invoke();
		}
	}
}