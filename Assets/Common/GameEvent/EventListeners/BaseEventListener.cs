using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Common
{
	public abstract class BaseEventListener : MonoBehaviour
	{
		#region Protected Attributes

		protected bool _registered = false;

		#endregion

		#region MonoBehaviour Callbacks

		protected void OnEnable()
		{
			Register();
		}

		protected void OnDisable()
		{
			Unregister();
		}

		#endregion

		#region Public Methods

		public abstract void OnEventRaised();

		#endregion

		#region Protected Methods

		protected abstract void Register();
		protected abstract void Unregister();

		#endregion
	}

	/// <summary>
	/// Interface for event listener for event with generic type.
	/// </summary>
	public interface IEventListener<T>
	{
		void OnEventRaised(T attr);
		Object GetObject();
	}

	[Serializable]
	public abstract class BaseEventListener<T, TEvent, TUnityEvent> : BaseEventListener,
		IEventListener<T> where TEvent : BaseEvent<T> where TUnityEvent : UnityEvent<T>
	{
		#region Protected Attributes

		[SerializeField]
		protected TEvent _event = null;
		[SerializeField]
		protected TUnityEvent _response = null;

		#endregion

		#region BaseEventListener Implementation

		public override void OnEventRaised()
		{
			Debug.LogError("ERROR: Typed event listener listened to event without parameter.");
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

		#region IEventListener<T> Implementation

		public Object GetObject()
		{
			return this;
		}

		public void OnEventRaised(T attr)
		{
			_response.Invoke(attr);
		}

		#endregion
	}
}