using System;
using System.Collections.Generic;
using System.Linq;

using ClockBlockers.MapData.Pathfinding;
using ClockBlockers.Utility;

using Unity.Burst;

using UnityEngine;


namespace ClockBlockers.MapData
{
    [ExecuteInEditMode][BurstCompile][Serializable]
    public class PathfindingMarker : MonoBehaviour, IPathRequester
    {
        // TODO: During pathfinding, find the Marker closest to the 'destination', and then recursive find the marker that's closest to you

        [SerializeReference]
        public List<MarkerStats> connectedMarkers;

        public IPathfinder CurrentPathfinder { get; set; }

        public int ConnMarkerCount => connectedMarkers?.Count(node => node != null) ?? 0;

        [HideInInspector]
        public PathfindingGrid grid;

        public float creationHeightAboveFloor;

        private PathfindingMarker otherSelectedMarker;

        private List<PathfindingMarker> pathToOtherSelectMarker;

        #region Gizmo
#if UNITY_EDITOR
        
        public float scale = 0.5f;
        private static readonly Color DefaultDrawColor = Color.white;
        private Color drawColor = DefaultDrawColor;

        private void OnDrawGizmos()
        {
            if (grid.alwaysDrawRays) DrawTransparentRays();

            if (grid.alwaysDrawNodes) DrawCubeGizmoOnPosition();

        }
        
        private void OnDrawGizmosSelected()
        {
            if (UnityEditor.Selection.activeGameObject != gameObject) return;

            if (CurrentPathfinder != null)
            {
                AffectDrawOfMarkers(CurrentPathfinder.OpenList, Color.red, grid.searchedNodeScale);
                return;
            }

            // ReSharper disable twice BitwiseOperatorOnEnumWithoutFlags
            const UnityEditor.SelectionMode selectionMode = UnityEditor.SelectionMode.TopLevel | UnityEditor.SelectionMode.ExcludePrefab | UnityEditor.SelectionMode.Editable;
            
            Transform[] transforms = UnityEditor.Selection.GetTransforms(selectionMode);
            
            if (transforms == null)
            {
                Logging.LogWarning("How could this possibly be null, assuming the selection is this gameObject");
                return;
            }

            var selectedMarkers = new List<PathfindingMarker>();

            PathfindingMarker otherMarker = null;
            
            foreach (Transform currTrans in transforms)
            {
                var currMarker = currTrans.GetComponent<PathfindingMarker>();
                if (currMarker == null) continue;

                selectedMarkers.Add(currMarker);

                if (currMarker != this) otherMarker = currMarker;
            }

            grid.ResetMarkerGizmos();
            switch (selectedMarkers.Count)
            {
                case 1:
                    DrawSingleSelectedMarker();
                    break;

                case 2 when otherMarker != null:
                    DrawPathToSelectedMarker(otherMarker);
                    break;
            }
        }


        private void DrawPathToSelectedMarker(PathfindingMarker marker)
        {
            if (marker != otherSelectedMarker)
            {
                otherSelectedMarker = marker;
                // Logging.Log("Selected different markers");
                RequestPathFrom(marker, grid.defaultJumpHeight);
            }

            if (pathToOtherSelectMarker == null || pathToOtherSelectMarker.Count == 0) return;
            
            // Logging.Log("Drawing path between markers");
            AffectDrawOfMarkers( pathToOtherSelectMarker);

            otherSelectedMarker.drawColor = Color.black;
            drawColor = Color.black;
        }

        private void DrawSingleSelectedMarker()
        {
            drawColor = Color.black;

            if (grid.selectionDrawRays) DrawTransparentRays();

            if (!grid.AffectNodes) return;

            scale = grid.selectionNodeScale;

            if (connectedMarkers == null) return;

            AffectDrawOfMarkers(GetAvailableConnectedMarkers(grid.defaultJumpHeight));
        }

        private void AffectDrawOfMarkers(List<MarkerStats> markerStatList)
        {
            AffectDrawOfMarkers(markerStatList.ConvertAll(markerStat => markerStat.marker));
        }

        private void AffectDrawOfMarkers(IEnumerable<PathfindingMarker> markerList)
        {
            AffectDrawOfMarkers(markerList, Color.magenta, grid.selectionNodeScale);
        }

        private void AffectDrawOfMarkers(List<Node> nodeList, Color newColor, float newScale)
        {
            AffectDrawOfMarkers(nodeList.ConvertAll(node => node.marker), newColor, newScale);
        }
        private void AffectDrawOfMarkers(IEnumerable<PathfindingMarker> markerList, Color newColor, float newScale)
        {
            foreach (PathfindingMarker marker in markerList.Where(marker => marker != null))
            {
                if (grid.selectionChangeNodeColors) marker.drawColor = newColor;

                if (grid.selectionChangeScale) marker.scale = newScale;
            }
        }

        private void DrawTransparentRays()
        {
            if (connectedMarkers == null) return;
            const float rayTransparency = 0.6f;
            var transparentDrawColor = new Color(drawColor.a, drawColor.g, drawColor.b, drawColor.a * rayTransparency);

            Gizmos.color = transparentDrawColor;
            DrawRayGizmosToConnectedMarkers();
        }

        private void PickAColor()
        {
            const int fewAdjacentNodesAmount = 3;
            const int someAdjacentNodesAmount = 6;

            drawColor = DefaultDrawColor;

            int connMarkersAmount = ConnMarkerCount;

            if (ConnMarkerCount == -1) return;

            if (connMarkersAmount == 0)
            {
                drawColor = Color.black;
            }
            else if (connMarkersAmount > 0 && connMarkersAmount < fewAdjacentNodesAmount)
            {
                drawColor = Color.green;
            }
            else if (connMarkersAmount >= fewAdjacentNodesAmount && connMarkersAmount < someAdjacentNodesAmount)
            {
                drawColor = Color.blue;
            }
            else
            {
                drawColor = Color.yellow;
            }
        }

        private void DrawCubeGizmoOnPosition()
        {
            Gizmos.color = drawColor;

            Vector3 position = transform.position;
            position.y += (grid.drawHeightAboveFloor - creationHeightAboveFloor) + (scale/2);

            Gizmos.DrawCube(position, Vector3.one * scale);
        }

        private void DrawRayGizmosToConnectedMarkers()
        {
            // if (adjacentMarkers == null || adjacentMarkers.Count == 0) return;

            foreach (MarkerStats markerStat in connectedMarkers.Where(markerStat => markerStat != null))
            {
                PathfindingMarker marker = markerStat.marker;
                
                Vector3 markerPos = marker.transform.position;
                Vector3 position = transform.position;

                Vector3 vectorToChild = markerPos - position;
                Gizmos.DrawRay(position, vectorToChild);
            }
        }

        public void ResetGizmo()
        {
            scale = grid.nodeScale;
            PickAColor();
        }
        
#endif
        #endregion


        public static PathfindingMarker CreateInstance(string markerName, PathfindingGrid grid, Vector3 markerPos, Transform parent, float creationYPosAboveFloor)
        {
            var newMarker = new GameObject(markerName).AddComponent<PathfindingMarker>();

            newMarker.transform.position = markerPos;
            newMarker.transform.SetParent(parent);
            
            newMarker.creationHeightAboveFloor = creationYPosAboveFloor;

            newMarker.grid = grid;

            grid.markers.Add(newMarker);
            return newMarker;
        }

        public static PathfindingMarker CreateInstance(string markerName, ref PathfindingGrid grid)
        {
            var newMarker = new GameObject(markerName).AddComponent<PathfindingMarker>();

            newMarker.grid = grid;
            
            grid.markers.Add(newMarker);
            
            return newMarker;
        }

        private void RequestPathTo(PathfindingMarker marker, float maxJumpHeight)
        {
            Logging.Log($"Finding path to {marker.name} from {name}");

            pathToOtherSelectMarker = null;
            
            grid.GetPath(this, marker, this, maxJumpHeight);
        }

        private void RequestPathFrom(PathfindingMarker marker, float maxJumpHeight)
        {
            Logging.Log($"Finding path to {marker.name} from {name}");
            
            pathToOtherSelectMarker = null;
            
            grid.GetPath(marker, this, this, maxJumpHeight);
        }

        void IPathRequester.PathCallback(List<PathfindingMarker> pathFinderPath)
        {
            pathToOtherSelectMarker = pathFinderPath;
            Logging.Log("Path Callback");
        }

        public IEnumerable<PathfindingMarker> GetAvailableConnectedMarkers(float jumpHeight)
        {
            if (connectedMarkers == null) return null;
            return (from connectedMarker in connectedMarkers
                where !(connectedMarker.yDistance > jumpHeight)
                select connectedMarker.marker).ToList();
        }
    }
}
