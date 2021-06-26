using System.Collections.Generic;
using UnityEngine;

namespace Biweekly
{
	public sealed class CrackSound : MonoBehaviour
	{
		[SerializeField]
		private AudioSource _source = null;
		[SerializeField]
		private List<AudioClip> _clips = new List<AudioClip>();

		public void PlayCrackSound()
		{
			int selectedClip = Random.Range(0, _clips.Count);
			_source.clip = _clips[selectedClip];
			_source.Play();
		}
	}
}