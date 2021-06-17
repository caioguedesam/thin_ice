using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Biweekly
{
	public sealed class IceTileCracking : MonoBehaviour
	{
		// References
		private Collider _thisCollider = null;
		private List<Collider> _pieceColliders = new List<Collider>();
		private List<Rigidbody> _pieceBodies = new List<Rigidbody>();
		
		[Header("Crack Controls")]
		[SerializeField]
		private Vector3 _forceOffsetFromCenter = Vector3.zero;
		private Vector3 _forceCenter = Vector3.zero;
		[SerializeField, Min(0f)]
		private float _horizontalForce = 0f;
		[SerializeField, Min(0f)]
		private float _verticalForce = 0f;
		[SerializeField, Min(0f)]
		private float _timeToDie = 0f;
		private bool _hasCracked = false;

		[Header("Events")]
		[SerializeField]
		private UnityEvent _onCrack = null;
		[SerializeField]
		private UnityEvent _onDeath = null;

		private void Awake()
		{
			_thisCollider = GetComponent<Collider>();
			_thisCollider.enabled = true;
			foreach (Transform child in transform)
			{
				Collider coll = child.GetComponent<Collider>();
				Rigidbody body = child.GetComponent<Rigidbody>();
				_pieceColliders.Add(coll);
				_pieceBodies.Add(body);
				coll.enabled = false;
			}

			_forceCenter = transform.position + _forceOffsetFromCenter;
		}

		public void Crack()
		{
			_thisCollider.enabled = false;
			for (int i = _pieceColliders.Count - 1; i >= 0; i--)
			{
				Collider coll = _pieceColliders[i];
				coll.enabled = true;

				Vector3 dir = GetDirectionFromCenter(coll.transform.position);
				dir = new Vector3(dir.x * _horizontalForce, dir.y * _verticalForce, dir.z * _horizontalForce);
				_pieceBodies[i].AddForce(dir, ForceMode.Impulse);
			}
			
			_onCrack.Invoke();
			StartCoroutine(Die());
		}

		private Vector3 GetDirectionFromCenter(Vector3 v)
		{
			return (v - _forceCenter).normalized;
		}

		private IEnumerator Die()
		{
			if (_hasCracked) yield break;
			_hasCracked = true;
			
			yield return new WaitForSeconds(_timeToDie);
			_onDeath.Invoke();
		}
	}
}