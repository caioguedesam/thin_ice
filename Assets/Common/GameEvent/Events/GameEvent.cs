using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	[CreateAssetMenu(fileName = CreateMenus.gameEventFile, menuName = CreateMenus.gameEventMenu,
		order = CreateMenus.gameEventOrder)]
	public sealed class GameEvent : BaseEvent
	{
		#region Private Attributes

		private List<BaseEventListener> _listeners = new List<BaseEventListener>();

		#endregion

		#region Public Properties

		public List<BaseEventListener> Listeners => _listeners;

		#endregion

		#region Public Methods

		public void Raise()
		{
			OnRaiseDebug();
			for (int i = _listeners.Count - 1; i >= 0; i--)
			{
				_listeners[i].OnEventRaised();
			}
		}

		public void AddListener(BaseEventListener listener)
		{
			_listeners.Add(listener);
		}

		public void RemoveListener(BaseEventListener listener)
		{
			_listeners.Remove(listener);
		}

		#endregion
	}
}