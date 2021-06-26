using UnityEngine;

namespace Biweekly
{
	public sealed class MusicPlayer : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}
