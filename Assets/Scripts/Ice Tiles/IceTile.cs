using Common;
using UnityEngine;

namespace Biweekly
{
	public sealed class IceTile : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private Collider _tileCollider = null;

		[Header("Player Positioning Controls")]
		[SerializeField]
		private Vector3 _playerPositionOffsetFromTopCenter = Vector3.zero;
		
		[Header("Top/Bottom Raycast Controls")]
		[SerializeField]
		private float _topCastDistance = 0f;
		[SerializeField]
		private LayerMask _raycastLayers = 0;

		[Header("Breaking")]
		[SerializeField]
		private GameObjectUnityEvent _onBreak = null;
		private bool _isTop = false;

		public bool IsTop => _isTop;
		public Vector3 PlayerPositionOnTile
		{
			get
			{
				Vector3 pos = transform.position;
				pos.y += _tileCollider.bounds.size.y;
				pos += _playerPositionOffsetFromTopCenter;
				return pos;
			}
		}

		private void Start()
		{
			TopCheck();
		}

		private void Update()
		{
			// TODO: Remove top check from update and change to whenever player finishes movement.
			TopCheck();
		}

		public void TopCheck()
		{
			Vector3 castOrigin = transform.position;
			castOrigin.y += _tileCollider.bounds.size.y;

			Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hitInfo, _topCastDistance, _raycastLayers);
			_isTop = hitInfo.collider == null;
		}

		public void BreakCheck(GameObject tileObj)
		{
			if (tileObj != gameObject) return;
			Break();
		}

		private void Break()
		{
			_onBreak.Invoke(gameObject);
		}

		public void Kill()
		{
			Destroy(gameObject);
		}
	}
}
