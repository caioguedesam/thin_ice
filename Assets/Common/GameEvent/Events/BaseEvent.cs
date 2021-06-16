using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	public abstract class BaseEvent : ScriptableObject
	{
		#region Protected Attributes

#pragma warning disable CS0414
		[SerializeField, Multiline]
		private string _description = null;
		[SerializeField]
		protected bool _debug = false;
#pragma warning restore CS0414

		#endregion

		#region Protected Methods

		protected void OnRaiseDebug()
		{
			if (_debug)
			{
				Debug.Log($"Raising {name}...");
			}
		}

		#endregion
	}

	[Serializable]
	public abstract class BaseEvent<T> : BaseEvent
	{
		#region Private Attributes

		private List<IEventListener<T>> _listeners = new List<IEventListener<T>>();

		#endregion

		#region Public Properties

		public List<IEventListener<T>> Listeners => _listeners;

		#endregion

		#region Public Methods

		public virtual void Raise(T attr)
		{
			OnRaiseDebug();
			for (int i = _listeners.Count - 1; i >= 0; i--)
			{
				_listeners[i].OnEventRaised(attr);
			}
		}

		public virtual void AddListener(IEventListener<T> listener)
		{
			_listeners.Add(listener);
		}

		public virtual void RemoveListener(IEventListener<T> listener)
		{
			_listeners.Remove(listener);
		}

		#endregion
	}
}
