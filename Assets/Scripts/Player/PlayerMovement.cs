using UnityEngine;

namespace Biweekly
{
	public sealed class PlayerMovement : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private PlayerNeighborDetection _neighborDetection = null;
		private Rigidbody _body = null;

		[Header("Initial Positioning")]
		[SerializeField]
		private float _downCastDistance = 0f;
		[SerializeField]
		private LayerMask _downCastLayerMask = 0;

		[SerializeField]
		private IceTile currentTile = null;

		private void Awake()
		{
			_body = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			MoveToInitialPosition();
		}

		private void MoveToInitialPosition()
		{
			Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, _downCastDistance,
				_downCastLayerMask);
			if (hitInfo.collider == null)
			{
				Debug.LogError("ERROR: Couldn't find initial tile to position player on at Start.");
				return;
			}

			IceTile tile = hitInfo.collider.GetComponentInParent<IceTile>();
			if (tile == null)
			{
				Debug.LogError($"ERROR: No IceTile component on {hitInfo.transform.parent.name}");
				return;
			}
			MoveTo(tile.PlayerPositionOnTile);
			currentTile = tile;
		}

		private void MoveTo(Vector3 position)
		{
			// TODO: Make animated/smooth movement
			_body.MovePosition(position);
		}
	}
}