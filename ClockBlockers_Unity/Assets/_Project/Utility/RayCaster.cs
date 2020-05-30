﻿using UnityEngine;


namespace ClockBlockers.Utility
{
	public static class RayCaster
	{
		// ~ is the "BITWISE NOT". Basically, it flips every bit in the int (int32)

		// Therefore, ~0 = 2^32-1

		private const int MarkerLayerBit = 13;
		private static void RemoveMarkerLayer(ref int layerToChange)
		{
			layerToChange ^= (-1 ^ layerToChange) & (1 << MarkerLayerBit);
		}
		
		public static RaycastHit[] CastRayAll(Ray ray, float distance, int layersToHit = ~0)
		{
			return Physics.RaycastAll(ray, distance, layersToHit, QueryTriggerInteraction.Ignore);
		}

		public static RaycastHit[] CastRayAllTriggers(Ray ray, float distance, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return Physics.RaycastAll(ray, distance, layersToHit, QueryTriggerInteraction.Collide);
		}

		public static RaycastHit[] CastRayAllTriggersIncludeMarkers(Ray ray, float distance, int layersToHit = ~0)
		{
			return Physics.RaycastAll(ray, distance, layersToHit, QueryTriggerInteraction.Collide);
		}

		
		
		public static bool CastRay(Ray ray, float distance, out RaycastHit hit, int layersToHit = ~0)
		{
			return Physics.Raycast(ray, out hit, distance, layersToHit, QueryTriggerInteraction.Ignore);
		}
		public static bool CastRay(Ray ray, float distance, int layersToHit = ~0)
		{
			return Physics.Raycast(ray, distance, layersToHit, QueryTriggerInteraction.Ignore);
		}

		public static bool CastRayTriggers(Ray ray, float distance, out RaycastHit hit, int layersToHit = ~0)
		{
			return Physics.Raycast(ray, out hit, distance, layersToHit, QueryTriggerInteraction.Collide);
		}
		public static bool CastRayTriggers(Ray ray, float distance, int layersToHit = ~0)
		{
			return Physics.Raycast(ray, distance, layersToHit, QueryTriggerInteraction.Collide);
		}

		public static bool CastRayTriggersIncludeMarkers(Ray ray, float distance, out RaycastHit hit, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return Physics.Raycast(ray, out hit, distance, layersToHit, QueryTriggerInteraction.Collide);
		}
		public static bool CastRayTriggersIncludeMarkers(Ray ray, float distance, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return Physics.Raycast(ray, distance, layersToHit, QueryTriggerInteraction.Collide);
		}
		
		

		public static bool BoxCast(Vector3 origin, Vector3 halfExtents, Vector3 direction, float distance, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return Physics.BoxCast(origin, halfExtents, direction, Quaternion.identity, distance, layersToHit, QueryTriggerInteraction.Ignore);
		}

		
		
		public static Collider[] OverLapBox(Vector3 origin, Vector3 halfExtents, Quaternion identity, int layersToHit = ~0)
		{
			return Physics.OverlapBox(origin, halfExtents, Quaternion.identity, layersToHit, QueryTriggerInteraction.Ignore);
		}
		
		public static Collider[] OverLapBoxTriggers(Vector3 origin, Vector3 halfExtents, Quaternion rotation, int layersToHit = ~0)
		{
			RemoveMarkerLayer(ref layersToHit);
			return OverLapBoxTriggersIncludeMarkers(origin, halfExtents, Quaternion.identity, layersToHit);
		}
		
		public static Collider[] OverLapBoxTriggersIncludeMarkers(Vector3 origin, Vector3 halfExtents, Quaternion rotation, int layersToHit = ~0)
		{
			return Physics.OverlapBox(origin, halfExtents, rotation, layersToHit, QueryTriggerInteraction.Collide);
		} 
		public static bool CheckBox(Vector3 origin, Vector3 halfExtents, Quaternion rotation, int layersToHit = ~0)
		{
			return Physics.CheckBox(origin, halfExtents, rotation, layersToHit, QueryTriggerInteraction.Ignore);
		}
		
		public static bool CheckBoxTriggers(Vector3 origin, Vector3 halfExtents, Quaternion rotation, int layersToHit = ~0)
		{
			return Physics.CheckBox(origin, halfExtents, rotation, layersToHit, QueryTriggerInteraction.Collide);
		}

		
		
		public static bool LineCast(Vector3 origin, Vector3 endPos, out RaycastHit raycastHit)
		{
			return Physics.Linecast(origin, endPos, out raycastHit);
		}
	}
}