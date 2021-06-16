using UnityEngine;

namespace Biweekly
{
	public sealed class PlayerSnowballStacker : MonoBehaviour
	{
		[Header("Snowball Stacking")]
		[SerializeField]
		private int _snowballLayerOnStack = 0;
		[SerializeField]
		private Transform _snowballParent = null;
		[SerializeField]
		private PlayerMovement _playerMovement = null;
		private Transform _currentRootSnowball = null;

		private void Awake()
		{
			_currentRootSnowball = _snowballParent.GetChild(0);
		}

		private void OnCollisionEnter(Collision other)
		{
			if (!other.collider.CompareTag("Snowball")) return;
			
			StackSnowball(other.transform);
		}

		private void StackSnowball(Transform newSnowball)
		{
			// Setup for stacking new snowball
			Vector3 bottomPos = newSnowball.position;
			SphereCollider coll = newSnowball.GetComponent<SphereCollider>();
			float yOffset = coll.radius * 2f;
			
			// Put new snowball in position
			newSnowball.gameObject.layer = _snowballLayerOnStack;
			newSnowball.parent = _snowballParent;
			newSnowball.localPosition = Vector3.zero;

			// And already collected snowballs on top
			_currentRootSnowball.parent = newSnowball;
			_currentRootSnowball.localPosition = new Vector3(0f, yOffset, 0f);

			// Setup for stacking next snowball
			transform.position = bottomPos;
			_currentRootSnowball = newSnowball;
			_playerMovement.CurrentTile.RemoveSnowball();
		}
	}
}
