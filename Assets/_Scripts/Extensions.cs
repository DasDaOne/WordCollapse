using UnityEngine;

public static class Extensions
{
	public static Rect GetScreenRect(this RectTransform rectTransform)
	{
		var worldCorners = new Vector3[4];
		rectTransform.GetWorldCorners(worldCorners);
		var result = new Rect(
			worldCorners[0].x,
			worldCorners[0].y,
			worldCorners[2].x - worldCorners[0].x,
			worldCorners[2].y - worldCorners[0].y);
		return result;
	}

}
