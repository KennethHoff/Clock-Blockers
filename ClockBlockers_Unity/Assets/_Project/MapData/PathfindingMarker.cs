using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public IPathfinder[] CurrentPathfinders { get; set; }

        public int ConnMarkerCount => connectedMarkers?.Count(node => node != null) ?? 0;

        [HideInInspector]
        public PathfindingGrid grid;

        public float creationHeightAboveFloor;

        private List<PathfindingMarker> _currentlySelectedMarkers;

        private List<PathfindingMarker> _pathToCurrentlySelectedMarkers;

        private List<PathfindingMarker>[] _workInProgressPath;

        #region Gizmo
#if UNITY_EDITOR
        
        public float scale = 0.5f;
        private static readonly Color DefaultDrawColor = Color.white;
        private Color _drawColor = DefaultDrawColor;

        private void OnDrawGizmos()
        {
            if (grid.alwaysDrawRays) DrawTransparentRays();

            if (grid.alwaysDrawNodes) DrawCubeGizmoOnPosition();

        }
        
        [SuppressMessage("ReSharper", "BitwiseOperatorOnEnumWithoutFlags")]
        private void OnDrawGizmosSelected()
        {
            if (UnityEditor.Selection.activeGameObject != gameObject) return;

            if (CurrentPathfinders != null)
            {
                foreach (IPathfinder currentPathfinder in CurrentPathfinders)
                {
                    if (currentPathfinder == null) continue;
                    AffectDrawOfMarkers(currentPathfinder.OpenList, Color.red, grid.searchedNodeScale);
                }
                return;
            }

            const UnityEditor.SelectionMode selectionMode = UnityEditor.SelectionMode.TopLevel | UnityEditor.SelectionMode.ExcludePrefab | UnityEditor.SelectionMode.Editable;
            
            Transform[] transforms = UnityEditor.Selection.GetTransforms(selectionMode);
            
            if (transforms == null)
            {
                Logging.LogWarning("How could this possibly be null, assuming the selection is this gameObject");
                return;
            }

            var selectedMarkers = new List<PathfindingMarker> {this};

            foreach (Transform currTrans in transforms)
            {
                var currMarker = currTrans.GetComponent<PathfindingMarker>();
                if (currMarker == null || currMarker == this) continue;

                selectedMarkers.Add(currMarker);
            }

            grid.ResetMarkerGizmos();
            switch (selectedMarkers.Count)
            {
                case 1:
                    DrawSingleSelectedMarker();
                    break;

                case 2:
                    DrawPathToSelectedMarker(selectedMarkers);
                    break;
                default:
                    DrawPathToSelectedMarkers(selectedMarkers);
                    break;
            }
        }

        private void DrawPathToSelectedMarkers(List<PathfindingMarker> selectedMarkers)
        {
            if (_currentlySelectedMarkers == null || !selectedMarkers.SequenceEqual(_currentlySelectedMarkers))
            {
                Logging.Log("Selected multiple new markers");
                RequestMultiPathTo(selectedMarkers, grid.defaultJumpHeight);
                _currentlySelectedMarkers = selectedMarkers;
            }

            if (_pathToCurrentlySelectedMarkers == null)
            {
                Logging.Log("Path not created yet!");
                return;
            }

            AffectDrawOfMarkers(_pathToCurrentlySelectedMarkers);
            
            foreach (PathfindingMarker currentlySelectedMarker in _currentlySelectedMarkers)
            {
                currentlySelectedMarker._drawColor = Color.black;
            }
            
        }
        
        private void DrawPathToSelectedMarker(IEnumerable<PathfindingMarker> selectedMarkers)
        {
            PathfindingMarker otherSelectedMarker = selectedMarkers.First(marker => marker != this);
            
            const int defaultIndex = 0;
            if (_currentlySelectedMarkers == null || 
                (_currentlySelectedMarkers != null && 
                 _currentlySelectedMarkers[defaultIndex] 
                 != otherSelectedMarker))
            {
                _currentlySelectedMarkers = new List<PathfindingMarker>() {otherSelectedMarker};
                Logging.Log("Selected new markers");
                RequestPathTo(otherSelectedMarker, grid.defaultJumpHeight);
            }

            if (_pathToCurrentlySelectedMarkers == null)
            {
                Logging.Log("Path not created yet");
                return;
            }

            PathfindingMarker currentSelectedMarker = _currentlySelectedMarkers[defaultIndex];
            // Logging.Log("Drawing path between markers");
            AffectDrawOfMarkers( _pathToCurrentlySelectedMarkers);

            currentSelectedMarker._drawColor = Color.black;
            _drawColor = Color.black;
        }

        private void DrawSingleSelectedMarker()
        {
            _drawColor = Color.black;

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
                if (grid.selectionChangeNodeColors) marker._drawColor = newColor;

                if (grid.selectionChangeScale) marker.scale = newScale;
            }
        }

        private void DrawTransparentRays()
        {
            if (connectedMarkers == null) return;
            const float rayTransparency = 0.6f;
            var transparentDrawColor = new Color(_drawColor.a, _drawColor.g, _drawColor.b, _drawColor.a * rayTransparency);

            Gizmos.color = transparentDrawColor;
            DrawRayGizmosToConnectedMarkers();
        }

        private void PickAColor()
        {
            const int fewAdjacentNodesAmount = 3;
            const int someAdjacentNodesAmount = 6;

            _drawColor = DefaultDrawColor;

            int connMarkersAmount = ConnMarkerCount;

            if (ConnMarkerCount == -1) return;

            if (connMarkersAmount == 0)
            {
                _drawColor = Color.black;
            }
            else if (connMarkersAmount > 0 && connMarkersAmount < fewAdjacentNodesAmount)
            {
                _drawColor = Color.green;
            }
            else if (connMarkersAmount >= fewAdjacentNodesAmount && connMarkersAmount < someAdjacentNodesAmount)
            {
                _drawColor = Color.blue;
            }
            else
            {
                _drawColor = Color.yellow;
            }
        }

        private void DrawCubeGizmoOnPosition()
        {
            Gizmos.color = _drawColor;

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

            newMarker.Inject(grid);

            grid.markers.Add(newMarker);
            return newMarker;
        }

        private void Inject(PathfindingGrid currGrid)
        {
            grid = currGrid;
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

            _pathToCurrentlySelectedMarkers = null;
            
            grid.GetPath(this, marker, this, maxJumpHeight);
            
            _workInProgressPath = new List<PathfindingMarker>[1];
            CurrentPathfinders = new IPathfinder[1];
        }

        private void RequestPathFrom(PathfindingMarker marker, float maxJumpHeight)
        {
            Logging.Log($"Finding path to {marker.name} from {name}");
            
            _pathToCurrentlySelectedMarkers = null;
            
            grid.GetPath(this, this, marker, maxJumpHeight);
            
            _workInProgressPath = new List<PathfindingMarker>[1];
            
            CurrentPathfinders = new IPathfinder[1];
        }

        private void RequestMultiPathTo(List<PathfindingMarker> markers, float maxJumpHeight)
        {
            int markersCount = markers.Count;
            
            // If 3 markers, that means Start -> 1 -> End => 1 'other marker' (3-2)
            
            Logging.Log($"Requesting path to {markers.Last().name} from {name} via {markersCount - 2} other markers");

            _pathToCurrentlySelectedMarkers = null;


            grid.GetMultiPath(this, this, markers, maxJumpHeight);
            
            // If 3 marks, that means Start -> 1. 1 -> End ==> 2 paths (3-1)
            _workInProgressPath = new List<PathfindingMarker>[markersCount-1];
            CurrentPathfinders = new IPathfinder[markersCount];
        }

        public void PathCallback(List<PathfindingMarker> pathFinderPath, int pathfinderIndex)
        {
            if (CurrentPathfinders?[pathfinderIndex] == null) return;

            CurrentPathfinders[pathfinderIndex] = null;

            Logging.Log($"Got a path callback from pathfinder #{pathfinderIndex}!");

            _workInProgressPath[pathfinderIndex] = pathFinderPath;

            MergeWorkInProgressPaths();
        }

        private void MergeWorkInProgressPaths()
        {
            if (CurrentPathfinders.Any(pathfinder => pathfinder != null)) return;

            if (_workInProgressPath.Any(path => path == null))
            {
                Logging.Log("<color='red'>All Pathfinders were finalized, but at least one of the paths were unsuccessful</color>");
                return;
            }
            

            _pathToCurrentlySelectedMarkers = _workInProgressPath.SelectMany(x => x).ToList();
            Logging.Log("Finalized Creation of paths");

            CurrentPathfinders = null;
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
