using UnityEngine;
using UnityEngine.Events;

namespace Biweekly
{
	public sealed class LevelCompletionChecker : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent _onLevelComplete = null;
		
		private int _levelTiles = 0;
		private bool _ended = false;

		private void Awake()
		{
			_levelTiles = GetComponentsInChildren<IceTile>().Length;
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
	}
}