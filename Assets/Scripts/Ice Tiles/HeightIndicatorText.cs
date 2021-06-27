using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Biweekly
{
	public sealed class HeightIndicatorText : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private TextMeshPro _text = null;

		[Header("Show Effects")]
		[SerializeField]
		private float _scaleInTime = 1f;
		[SerializeField]
		private float _scaleOutTime = 1f;

		[Header("Cast Controls")]
		[SerializeField]
		private LayerMask _downCastLayers = 0;
		[SerializeField]
		private float _downCastDistance = 10f;
		private int _height = 0;

		private void Awake()
		{
			_text.rectTransform.localScale = Vector3.zero;
			CheckHeight();
		}

		public void Show()
		{
			_text.DOScale(1f, _scaleInTime);
		}

		public void Hide()
		{
			_text.DOScale(0f, _scaleOutTime);
		}

		public void CheckHeight()
		{
			_height = GetCurrentHeight();
			_text.text = $"{_height}";
		}

		private int GetCurrentHeight()
		{
			RaycastHit[] hits =
				Physics.RaycastAll(transform.position, Vector3.down, _downCastDistance, _downCastLayers);
			if (hits.Length == 0) return 1;
			return hits.Length;
		}
	}
}