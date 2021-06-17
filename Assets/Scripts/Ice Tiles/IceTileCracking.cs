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
		private List<Material> _pieceMats = new List<Material>();
		
		[Header("Crack Controls")]
		[SerializeField]
		private Vector3 _forceOffsetFromCenter = Vector3.zero;
		private Vector3 _forceCenter = Vector3.zero;
		[SerializeField, Min(0f)]
		private float _horizontalForce = 0f;
		[SerializeField, Min(0f)]
		private float _verticalForce = 0f;
		[SerializeField, Min(0f)]
		private float _fadeTime = 1.5f;
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
				Material mat = child.GetComponent<MeshRenderer>().material;
				_pieceColliders.Add(coll);
				_pieceBodies.Add(body);
				_pieceMats.Add(mat);
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
				Rigidbody body = _pieceBodies[i];
				coll.enabled = true;

				Vector3 dir = GetDirectionFromCenter(coll.transform.position);
				dir = new Vector3(dir.x * _horizontalForce, dir.y * _verticalForce, dir.z * _horizontalForce);

				body.isKinematic = false;
				body.useGravity = true;
				body.AddForce(dir, ForceMode.Impulse);
			}
			
			_onCrack.Invoke();
			StartCoroutine(FadePieces());
			StartCoroutine(Die());
		}

		private Vector3 GetDirectionFromCenter(Vector3 v)
		{
			return (v - _forceCenter).normalized;
		}

		private IEnumerator FadePieces()
		{
			float elapsedTime = 0f;
			Color originalColor = _pieceMats[0].color;
			while (elapsedTime < _fadeTime)
			{
				float t = Mathf.Clamp(_fadeTime - elapsedTime, 0f, 1f);
				Color color = Color.Lerp(originalColor, Color.clear, 1 - t);
				for (int i = _pieceMats.Count - 1; i >= 0; i--)
				{
					//_pieceMats[i].SetColor("_Color", color);
					_pieceMats[i].SetColor("_BaseColor", color);
				}

				yield return new WaitForEndOfFrame();
				elapsedTime += Time.deltaTime;
			}
		}

		private IEnumerator Die()
		{
			if (_hasCracked) yield break;
			_hasCracked = true;
			
			yield return new WaitForSeconds(_timeToDie);
			_onDeath.Invoke();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position + _forceOffsetFromCenter, 0.25f);
		}
	}
}