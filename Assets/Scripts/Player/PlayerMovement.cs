using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

		[Header("Jump Controls")]
		[SerializeField]
		private float _jumpHeight = 0f;
		[SerializeField]
		private float _jumpTime = 0f;
		[SerializeField]
		private float _fallTime = 0f;
		private float _gravity = 0f;
		private bool _onJump = false;

		[Header("Final Movement Controls")]
		[SerializeField]
		private string _killTag = "";
		[SerializeField, Min(0f)]
		private float _finalPositionYOffset = 0f;
		private bool _madeFinalJump = false;
		
		[Header("Events")]
		[SerializeField]
		private UnityEvent _onJumpStart = null;
		[SerializeField]
		private UnityEvent _onJumpEnd = null;
		[SerializeField]
		private UnityEvent _onDeath = null;
		
		private IceTile _currentTile = null;
		public IceTile CurrentTile => _currentTile;

		private bool IsFalling => _body.velocity.y < 0f;

		private void Awake()
		{
			_body = GetComponent<Rigidbody>();
			_body.useGravity = false;
		}

		private void Start()
		{
			MoveToInitialPosition();
		}

		private void Update()
		{
			if (_onJump || _madeFinalJump)
				ApplyGravity();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(_killTag))
			{
				_onDeath.Invoke();
				gameObject.SetActive(false);
			}
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
			
			MoveTo(tile, true);
		}

		public void MoveCheck(GameObject tileObj)
		{
			IceTile tile = tileObj.GetComponent<IceTile>();
			if (tile == null || tile == _currentTile || !_neighborDetection.IsNeighbor(tile)) return;
			
			MoveTo(tile);
		}

		public void FinalMove()
		{
			MoveTo(null, finalMove: true);
		}

		private void MoveTo(IceTile tile, bool initialMove = false, bool finalMove = false)
		{
			// Don't move when mid jump.
			if (_onJump) return;
			
			if(_currentTile != null)
				_currentTile.RemovePlayer();
			
			IceTile lastTile = _currentTile;
			if (finalMove)
			{
				_currentTile = null;
				Vector3 finalPos = transform.position + _finalPositionYOffset * Vector3.down;
				StartCoroutine(JumpRoutine(finalPos));
				_madeFinalJump = true;
			}
			else
			{
				_currentTile = tile;
				_currentTile.AddPlayer();
				StartCoroutine(JumpRoutine(_currentTile.PlayerPositionOnTile));
			}

			if (!initialMove)
			{
				if(lastTile != null)
					lastTile.Break();
				if(tile != null)
					transform.forward = new Vector3(tile.PlayerPositionOnTile.x - transform.position.x, 0f, tile.PlayerPositionOnTile.z - transform.position.z);
			}
		}

		private IEnumerator JumpRoutine(Vector3 finalPos)
		{
			_onJump = true;
			_onJumpStart.Invoke();
			// Horizontal Movement on Jump
			Vector3 horizontalStart = new Vector3(transform.position.x, 0f, transform.position.z);
			Vector3 horizontalEnd = new Vector3(finalPos.x, 0f, finalPos.z);
			Vector3 horizontalDirection = horizontalEnd - horizontalStart;
			float horizontalDistance = horizontalDirection.magnitude;
			horizontalDirection.Normalize();
			float velX = horizontalDirection.x * (horizontalDistance / (_jumpTime * 2f));
			float velZ = horizontalDirection.z * (horizontalDistance / (_jumpTime * 2f));

			// Jump Up
			float heightUp = GetJumpUpHeight(transform.position.y, finalPos.y);
			float velY = 2f * heightUp / _jumpTime;
			SetGravity(heightUp, _jumpTime);
			_body.velocity = new Vector3(velX, velY, velZ);
			
			// Wait for Fall
			yield return new WaitUntil(() => IsFalling);
			
			// Fall
			velX = horizontalDirection.x * (horizontalDistance / (_fallTime * 2f));
			velZ = horizontalDirection.z * (horizontalDistance / (_fallTime * 2f));
			_body.velocity = new Vector3(velX, _body.velocity.y, velZ);
			float heightDown = Mathf.Abs(transform.position.y - finalPos.y);
			SetGravity(heightDown, _fallTime);
			yield return new WaitForSeconds(_fallTime);

			_onJump = false;
			_onJumpEnd.Invoke();
		}

		private void SetGravity(float dist, float time)
		{
			_gravity = (-2f * dist) / Mathf.Pow(time, 2);
		}

		private void ApplyGravity()
		{
			_body.velocity += new Vector3(0f, _gravity * Time.deltaTime, 0f);
		}

		private float GetJumpUpHeight(float startY, float endY)
		{
			if (startY < endY) return _jumpHeight + (endY - startY);
			return _jumpHeight;
		}

		public void ToggleConstraints(bool value)
		{
			_body.constraints =
				!value
					? RigidbodyConstraints.FreezeRotation
					: RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		}
	}
}