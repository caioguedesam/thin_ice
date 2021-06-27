using UnityEngine;
using UnityEngine.Events;

namespace Common
{
	public sealed class GameEventListener : BaseEventListener
	{
		#region Private Attributes

		[SerializeField]
		private GameEvent _event = null;
		[SerializeField]
		private UnityEvent _response = null;

		#endregion

		#region Public Properties

		public GameEvent Event => _event;

		#endregion

		#region BaseEventListener Implementation

		public override void OnEventRaised()
		{
			_response.Invoke();
		}

		protected override void Register()
		{
			if (_registered) return;
			_event.AddListener(this);
			_registered = true;
		}

		protected override void Unregister()
		{
			if (!_registered) return;
			_event.RemoveListener(this);
			_registered = false;
		}

		#endregion
	}
}