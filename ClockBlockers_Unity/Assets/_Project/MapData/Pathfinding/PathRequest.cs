namespace ClockBlockers.MapData.Pathfinding
{
	public readonly struct PathRequest
	{
		public static PathRequest CreateInstance(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker, float maxJumpHeight)
		{
			return new PathRequest(pathRequester, startMarker, endMarker, maxJumpHeight);
		}

		private PathRequest(IPathRequester pathRequester, PathfindingMarker startMarker, PathfindingMarker endMarker, float maxJumpHeight)
		{
			this.pathRequester = pathRequester;
			this.startMarker = startMarker;
			this.endMarker = endMarker;
			this.maxJumpHeight = maxJumpHeight;
		}

		public readonly IPathRequester pathRequester;
		public readonly PathfindingMarker startMarker;
		public readonly PathfindingMarker endMarker;
		public readonly float maxJumpHeight;
	}
}