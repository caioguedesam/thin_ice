using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Behaviors
{
	public sealed class SimpleDelay : MonoBehaviour
	{
		[SerializeField]
		private float _delayTime = 0f;
		[SerializeField]
		private UnityEvent _onStart = null;
		[SerializeField]
		private UnityEvent _onEnd = null;

		private bool _onDelay = false;

		public void StartDelay()
		{
			if (_onDelay) return;
			StartCoroutine(DelayRoutine());
		}

		private IEnumerator DelayRoutine()
		{
			_onDelay = true;
			_onStart.Invoke();
			
			yield return new WaitForSeconds(_delayTime);
			
			_onEnd.Invoke();
			_onDelay = false;
		}
	}
}