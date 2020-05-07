namespace ClockBlockers.MapData.Pathfinding
{
	public readonly struct PathRequest
	{
		public static PathRequest CreateInstance(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker)
		{
			return new PathRequest(pathRequester, startMarker, endMarker);
		}

		private PathRequest(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker)
		{
			this.pathRequester = pathRequester;
			this.startMarker = startMarker;
			this.endMarker = endMarker;
		}

		public readonly IPathRequester pathRequester;
		public readonly PathfindingMarker startMarker;
		public readonly PathfindingMarker endMarker;
	}
}