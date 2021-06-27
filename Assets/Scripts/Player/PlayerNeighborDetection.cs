using System;
using System.Collections.Generic;
using UnityEngine;

namespace Biweekly
{
	public sealed class PlayerNeighborDetection : MonoBehaviour
	{
		[SerializeField]
		private List<IceTile> _neighbors = new List<IceTile>();
		
		private void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag("IceTile")) return;
			RemoveNeighbor(other);
		}

		private void OnTriggerStay(Collider other)
		{
			if (!other.CompareTag("IceTile")) return;
			NeighborCheck(other);
		}

		private void Update()
		{
			RemoveAllInvalidNeighbors();
		}

		private void NeighborCheck(Collider other)
		{
			IceTile tile = other.GetComponentInParent<IceTile>();
			if (tile == null || _neighbors.Contains(tile) || !tile.IsTop || tile.IsPlayerStandingOn) return;
			_neighbors.Add(tile);
		}

		private void RemoveNeighbor(Collider other)
		{
			IceTile tile = other.GetComponentInParent<IceTile>();
			if (tile == null) return;
			_neighbors.Remove(tile);
		}

		private void RemoveAllInvalidNeighbors()
		{
			for (int i = _neighbors.Count - 1; i >= 0; i--)
			{
				if (_neighbors[i] == null || !_neighbors[i].IsTop || _neighbors[i].IsPlayerStandingOn) _neighbors.RemoveAt(i);
			}
		}

		public bool IsNeighbor(IceTile tile)
		{
			return _neighbors.Contains(tile);
		}
	}
}