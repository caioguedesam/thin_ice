using UnityEngine;

namespace Common
{
	[CreateAssetMenu(fileName = CreateMenus.intEventFile, menuName = CreateMenus.intEventMenu,
		order = CreateMenus.intEventOrder)]
	public sealed class IntEvent : BaseEvent<int>
	{
		
	}
}