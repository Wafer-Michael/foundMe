using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class CellMap
    {
		//デフォルトパラメータ
		static public readonly Parametor DEFAULT_PARAMETOR = new Parametor(
			new maru.Rect(Vector3.zero, 2.0f, 2.0f),	//一つのセルを構成するRectData
			6,								//widthCount
			4,								//depthCount
			new Vector3(0.0f, 0.0f, 0.0f)   //centerPosition
		);

		//--------------------------------------------------------------------------------------
		///	セルマップファクトリーのパラメータ
		//--------------------------------------------------------------------------------------
		[System.Serializable]
		public struct Parametor
		{
			public maru.Rect oneCellRect;       //一つのCellの生成情報。
			public int widthCount;				//横に展開するセルの数。
			public int depthCount;				//奥行きに展開するセルの数。
			public Vector3 centerPosition;		//マップの中心位置。

			public Parametor(
				maru.Rect oneCellRect,
				int widthCount,
				int depthCount,
				Vector3 centerPosition
			){
				this.oneCellRect = oneCellRect;
				this.widthCount = widthCount;
				this.depthCount = depthCount;
				this.centerPosition = centerPosition;
			}
		}

		/// <summary>
		/// セルの生成
		/// </summary>
		/// <param name="param">パラメータ</param>
		/// <returns>生成されたセル配列</returns>
		public static List<Cell> CreateCells(Parametor param)
        {
			return CreateCells<Cell>(param);
		}

		/// <summary>
		/// セルマップの生成
		/// </summary>
		/// <typeparam name="T">セルタイプ</typeparam>
		/// <param name="param">生成パラメータ</param>
		/// <returns>生成されたセル配列</returns>
		public static List<T> CreateCells<T>(Parametor param)
			where T : Cell
		{
			var result = new List<T>();

			float halfOneRectWidth = param.oneCellRect.width * 0.5f;
			float halfOneRectDepth = param.oneCellRect.depth * 0.5f;

			float fieldRectWidth = (float)param.widthCount * param.oneCellRect.width;
			float fieldRectDepth = (float)param.depthCount * param.oneCellRect.depth;
			var fieldRect = new maru.Rect(param.centerPosition, fieldRectWidth, fieldRectDepth);
			var startPosition = fieldRect.CalculateStartPosition(); //マップの左上の原点を取得

			//ループして生成
			for (int i = 0; i < param.depthCount; i++)
			{
				//縦位置の軸を決定
				float depth = startPosition.z + (param.oneCellRect.depth * i);
				depth += halfOneRectDepth;

				//Debug.Log("★" + i.ToString() + "★");
				for (int j = 0; j < param.widthCount; j++)
				{
					//横位置の軸を決定
					float width = startPosition.x + (param.oneCellRect.width * j);
					width += halfOneRectWidth;

					int index = j + (i * param.widthCount); //インデックス設定
					var position = new Vector3(width, param.centerPosition.y, depth); //位置情報の決定

					//Cellのパラメータを設定
					var cellParam = new Cell.Parametor(param.oneCellRect);
					var cell = maru.Generic.Construct<T, int, Cell.Parametor>(index, cellParam);			//Cell生成
					cell.SetPosition(position);									//Cellの位置変更

					result.Add(cell); //resultに追加
				}
			}

			return result;
		}
	}
}
