using UnityEngine;

namespace Common
{
	[CreateAssetMenu(fileName = CreateMenus.stringEventFile, menuName = CreateMenus.stringEventMenu,
		order = CreateMenus.stringEventOrder)]
	public sealed class StringEvent : BaseEvent<string>
	{
		
	}
}