using UnityEngine;

namespace Common
{
	[CreateAssetMenu(fileName = CreateMenus.floatEventFile, menuName = CreateMenus.floatEventMenu,
		order = CreateMenus.floatEventOrder)]
	public sealed class FloatEvent : BaseEvent<float>
	{
		
	}
}