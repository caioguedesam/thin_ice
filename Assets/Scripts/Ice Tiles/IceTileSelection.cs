using System;
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
			EnterHover();
		}

		private void OnMouseOver()
		{
			if (!IsSelectable && _onHover)
			{
				ExitHover();
			}
			else if (IsSelectable && !_onHover)
			{
				EnterHover();
			}
		}

		private void OnMouseExit()
		{
			if (!_onHover) return;
			ExitHover();
		}

		private void OnMouseDown()
		{
			if (!_onHover) return;
			MouseClick();
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

		private void EnterHover()
		{
			_onHover = true;
			_onMouseEnteredTile.Invoke(gameObject);
		}

		private void ExitHover()
		{
			_onHover = false;
			_onMouseExitedTile.Invoke(gameObject);
		}

		private void MouseClick()
		{
			_onMouseDownOnTile.Invoke(gameObject);
		}
	}
}