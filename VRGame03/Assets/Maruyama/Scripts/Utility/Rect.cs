using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace maru
{
	[System.Serializable]
	public struct Rect
	{
		public Vector3 centerPosition;  //中心位置
		public float width;             //横のサイズ
		public float depth;             //奥行のサイズ

		public Rect(Vector3 centerPosition, float width, float depth)
		{
			this.centerPosition = centerPosition;
			this.width = width;
			this.depth = depth;
		}

		/// <summary>
		/// 開始位置の計算
		/// </summary>
		/// <returns></returns>
		public Vector3 CalculateStartPosition()
		{
			var position = centerPosition;
			var scale = new Vector3(width, 0.0f, depth);
			var halfScale = scale * 0.5f;
			float x = position.x - halfScale.x;
			float y = position.y;
			float z = position.z - halfScale.z;
			var startPosition = new Vector3(x, y, z);

			return startPosition;
		}

		/// <summary>
		/// 範囲内にいるかどうかを判断
		/// </summary>
		/// <param name="position">判断したいポジション</param>
		/// <returns></returns>
		public bool IsInRect(Vector3 position)
		{
			var rectStartPosition = CalculateStartPosition();

			if (position.x >= rectStartPosition.x &&
				position.x <= rectStartPosition.x + this.width &&
				position.z >= rectStartPosition.z &&
				position.z <= rectStartPosition.z + this.depth
			) {
				return true;
			}

			return false;
		}

	}

}