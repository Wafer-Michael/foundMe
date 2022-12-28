using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace maru
{
	public struct Rect
	{
		public Vector3 centerPosition;  //���S�ʒu
		public float width;             //���̃T�C�Y
		public float depth;             //���s�̃T�C�Y

		public Rect(Vector3 centerPosition, float width, float depth)
		{
			this.centerPosition = centerPosition;
			this.width = width;
			this.depth = depth;
		}

		/// <summary>
		/// �J�n�ʒu�̌v�Z
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
		/// �͈͓��ɂ��邩�ǂ����𔻒f
		/// </summary>
		/// <param name="position">���f�������|�W�V����</param>
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