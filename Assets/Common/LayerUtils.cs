using UnityEngine;

namespace Common
{
	public static class LayerUtils
	{
		/// <summary>
		///  Returns if a layer is in a given layer mask.
		/// </summary>
		public static bool IsOnLayer(int layer, LayerMask mask)
		{
			return mask == (mask | (1 << layer));
		}
	}
}