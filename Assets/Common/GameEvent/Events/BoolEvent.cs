using UnityEngine;

namespace Common
{
	[CreateAssetMenu(fileName = CreateMenus.boolEventFile, menuName = CreateMenus.boolEventMenu,
		order = CreateMenus.boolEventOrder)]
	public sealed class BoolEvent : BaseEvent<bool>
	{
		
	}
}