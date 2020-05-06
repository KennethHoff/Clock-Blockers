using System.Collections.Generic;
using System.Linq;

using ClockBlockers.Utility;

using UnityEditor;

using UnityEngine;


namespace ClockBlockers.MapData
{
    [ExecuteInEditMode]
    public class PathfindingMarker : MonoBehaviour
    {
        // TODO: During pathfinding, find the Marker closest to the 'destination', and then recursive find the marker that's closest to you

        [SerializeReference]
        public List<PathfindingMarker> connectedMarkers;

        // public List<MarkerStats> connectedMarkers;

        // public List<Node> connectedMarkers;

        public int ConnMarkerCount
        {
            get
            {
                if (connectedMarkers == null) return -1;

                return connectedMarkers.Count(node => node != null);
            }
        }

        public float scale = 0.5f;

        // private static readonly Color DefaultDrawColor = new Color(137, 0, 255, 255); // purple
        private static readonly Color DefaultDrawColor = Color.white;

        private Color drawColor = DefaultDrawColor;

        [HideInInspector]
        public PathfindingGrid grid;

        // _Exclusively_ for equalizing the 'Draw Height Above Floor' gizmo drawing.
        public float creationHeightAboveFloor;


        
        private PathfindingMarker otherSelectedMarker;
        private List<PathfindingMarker> pathToOtherSelectMarker;

        
        
        private void OnDrawGizmos()
        {
            if (grid.alwaysDrawRays) DrawTransparentRays();

            if (grid.alwaysDrawNodes) DrawCubeGizmoOnPosition();

            if (grid.alwaysDrawCollisionArea) DrawCollisionArea();
        }

        private void DrawCollisionArea()
        {
            Vector3 markerPos = transform.position;
            var newPos = new Vector3(markerPos.x, markerPos.y + grid.minimumOpenAreaAroundMarkers.y / 2, markerPos.z);

            Gizmos.DrawWireCube(newPos, grid.minimumOpenAreaAroundMarkers);
        }

        private void OnDrawGizmosSelected()
        {
            if (Selection.activeGameObject != gameObject) return;
            // DrawSingleSelectedMarker();
            

            // // var gameObjects = Selection.GetFiltered(typeof(PathfindingMarker),SelectionMode.TopLevel | SelectionMode.ExcludePrefab | SelectionMode.Editable) as GameObject[];
            
            var transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab | SelectionMode.Editable);
            
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
            if (selectedMarkers.Count == 1) DrawSingleSelectedMarker();
            else if (selectedMarkers.Count == 2 && otherMarker != null) DrawPathToSelectedMarker(otherMarker);
        }

        private void DrawPathToSelectedMarker(PathfindingMarker marker)
        {
            if (marker != otherSelectedMarker)
            {
                otherSelectedMarker = marker;
                Logging.Log("Selected different markers");
                pathToOtherSelectMarker = GetPathFrom(marker);
            }
            

            Logging.Log("Drawing path between markers");
            AffectDrawOfMarkers(pathToOtherSelectMarker);
            
            
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

            AffectDrawOfMarkers(connectedMarkers);
        }

        private void AffectDrawOfMarkers(IReadOnlyCollection<PathfindingMarker> markerList)
        {
            foreach (PathfindingMarker marker in markerList.Where(marker => marker != null))
            {
                if (grid.selectionChangeNodeColors) marker.drawColor = Color.magenta;

                marker.scale = grid.selectionChangeScale ? grid.selectionNodeScale : grid.nodeScale;
            }
        }

        private bool CheckIfConnected(PathfindingMarker marker)
        {
            return grid.GetPath(this, marker) != null;
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
                drawColor = new Color(137, 0, 255, 255); // purple
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

            foreach (PathfindingMarker marker in connectedMarkers)
            {
                // PathfindingMarker marker = markerStat.marker;
                if (marker == null)
                {
                    Logging.Log("Marker in list does not exist for some reason..");
                    return;
                }

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

        public static PathfindingMarker CreateInstance(string markerName, PathfindingGrid grid, Vector3 markerPos, Transform parent, float creationYPosAboveFloor)
        {
            var newMarker = new GameObject(markerName).AddComponent<PathfindingMarker>();
            // newMarker.connectedMarkers = new List<MarkerStats>();

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
            // newMarker.connectedMarkers = new List<MarkerStats>();

            newMarker.grid = grid;
            
            grid.markers.Add(newMarker);
            
            return newMarker;
        }

        public void GetOrAddToGridDictionary()
        {
            Node node = grid.GetOrAddMarkerToDictionary(this, -1, -1);
        }

        private List<PathfindingMarker> GetPathTo(PathfindingMarker marker)
        {
            return grid.GetPath(this, marker);
        }
        private List<PathfindingMarker> GetPathFrom(PathfindingMarker marker)
        {
            return grid.GetPath(marker, this);
        }
    }
}
