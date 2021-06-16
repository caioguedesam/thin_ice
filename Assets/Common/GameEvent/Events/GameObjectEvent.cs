using UnityEngine;

namespace Common
{
	[CreateAssetMenu(fileName = CreateMenus.gameObjectEventFile, menuName = CreateMenus.gameObjectEventMenu,
		order = CreateMenus.gameObjectEventOrder)]
	public sealed class GameObjectEvent : BaseEvent<GameObject>
	{
		
	}
}