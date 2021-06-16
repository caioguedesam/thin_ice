using Common;
using UnityEngine;

namespace Biweekly
{
	public sealed class IceTileSelection : MonoBehaviour
	{
		// References
		private IceTile _iceTile = null;
		
		[Header("Events")]
		[SerializeField]
		private GameObjectUnityEvent _onMouseEnteredTile = null;
		[SerializeField]
		private GameObjectUnityEvent _onMouseExitedTile = null;
		[SerializeField]
		private GameObjectUnityEvent _onMouseDownOnTile = null;
		
		private bool _onHover = false;

		private void Awake()
		{
			_iceTile = GetComponent<IceTile>();
		}

		private void OnMouseEnter()
		{
			if (!_iceTile.IsTop) return;
			_onHover = true;
			_onMouseEnteredTile.Invoke(gameObject);
		}

		private void OnMouseExit()
		{
			if (!_onHover) return;
			_onHover = false;
			_onMouseExitedTile.Invoke(gameObject);
		}

		private void OnMouseDown()
		{
			_onMouseDownOnTile.Invoke(gameObject);
		}
	}
}