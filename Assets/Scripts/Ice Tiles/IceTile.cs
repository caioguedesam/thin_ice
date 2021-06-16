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
		private bool _hasPlayer = false;
		
		[Header("Top/Bottom Raycast Controls")]
		[SerializeField]
		private float _topCastDistance = 0f;
		[SerializeField]
		private LayerMask _raycastLayers = 0;

		[Header("Breaking")]
		[SerializeField]
		private GameObjectUnityEvent _onBreak = null;
		private bool _isTop = false;

		[Header("Snowball Spawning")]
		[SerializeField]
		private bool _startsWithSnowball = false;
		[SerializeField]
		private SphereCollider _snowballPrefab = null;
		private bool _hasSnowball = false;

		public bool IsPlayerStandingOn => _hasPlayer;
		public bool IsTop => _isTop;
		public Vector3 PlayerPositionOnTile
		{
			get
			{
				Vector3 pos = transform.position;
				pos.y += _tileCollider.bounds.size.y;
				pos += _playerPositionOffsetFromTopCenter;
				if (_hasSnowball)
				{
					pos.y += _snowballPrefab.radius * 2f;
				}
				return pos;
			}
		}

		private void Start()
		{
			if (_startsWithSnowball)
			{
				SpawnSnowball();
			}
			TopCheck();
		}

		private void Update()
		{
			// TODO: Remove top check from update and change to whenever player finishes movement.
			TopCheck();
		}

		public void AddPlayer()
		{
			_hasPlayer = true;
		}

		public void RemovePlayer()
		{
			_hasPlayer = false;
		}

		public void RemoveSnowball()
		{
			_hasSnowball = false;
		}

		private void SpawnSnowball()
		{
			Instantiate(_snowballPrefab, PlayerPositionOnTile, Quaternion.identity, null);
			_hasSnowball = true;
		}

		public void TopCheck()
		{
			Vector3 castOrigin = transform.position;
			castOrigin.y += _tileCollider.bounds.extents.y;

			Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hitInfo, _topCastDistance, _raycastLayers);
			_isTop = hitInfo.collider == null;
		}

		public void Break()
		{
			_onBreak.Invoke(gameObject);
		}

		public void Kill()
		{
			Destroy(gameObject);
		}
	}
}
