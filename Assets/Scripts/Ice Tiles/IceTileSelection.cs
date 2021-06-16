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
		private bool _onPlayerRange = false;
		private bool IsSelectable => _onPlayerRange && _iceTile.IsTop && !_iceTile.IsPlayerStandingOn;

		private void Awake()
		{
			_iceTile = GetComponent<IceTile>();
		}

		private void OnMouseEnter()
		{
			if (!IsSelectable) return;
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
			if (!IsSelectable) return;
			_onMouseDownOnTile.Invoke(gameObject);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Player Neighbor Detection")) return;

			_onPlayerRange = true;
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("Player Neighbor Detection")) return;

			_onPlayerRange = false;
		}
	}
}