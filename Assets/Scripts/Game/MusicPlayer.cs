using UnityEngine;

namespace Biweekly
{
	public sealed class MusicPlayer : MonoBehaviour
	{

		private static MusicPlayer _instance = null;
		
		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}
