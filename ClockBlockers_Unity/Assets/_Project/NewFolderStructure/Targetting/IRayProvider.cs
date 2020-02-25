using UnityEngine;


namespace ClockBlockers.Targetting
{
	public interface IRayProvider
	{
		Ray CreateRay();
	}
}