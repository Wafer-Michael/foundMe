using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Factory
{
    public class CellMap
    {
		//�f�t�H���g�p�����[�^
		static public readonly Parametor DEFAULT_PARAMETOR = new Parametor(
			new maru.Rect(Vector3.zero, 2.0f, 2.0f),	//��̃Z�����\������RectData
			6,								//widthCount
			4,								//depthCount
			new Vector3(0.0f, 0.0f, 0.0f)   //centerPosition
		);

		//--------------------------------------------------------------------------------------
		///	�Z���}�b�v�t�@�N�g���[�̃p�����[�^
		//--------------------------------------------------------------------------------------
		[System.Serializable]
		public struct Parametor
		{
			public maru.Rect oneCellRect;       //���Cell�̐������B
			public int widthCount;         //���ɓW�J����Z���̐��B
			public int depthCount;         //���s���ɓW�J����Z���̐��B
			public Vector3 centerPosition;    //�}�b�v�̒��S�ʒu�B

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
		/// �Z���̐���
		/// </summary>
		/// <param name="param">�p�����[�^</param>
		/// <returns>�������ꂽ�Z���z��</returns>
		public static List<Cell> CreateCells(Parametor param)
        {
			var result = new List<Cell>();

			float halfOneRectWidth = param.oneCellRect.width * 0.5f;
			float halfOneRectDepth = param.oneCellRect.depth * 0.5f;

			float fieldRectWidth = (float)param.widthCount * param.oneCellRect.width;
			float fieldRectDepth = (float)param.depthCount * param.oneCellRect.depth;
			var fieldRect = new maru.Rect(param.centerPosition, fieldRectWidth, fieldRectDepth);
			var startPosition = fieldRect.CalculateStartPosition();	//�}�b�v�̍���̌��_���擾

			//���[�v���Đ���
			for (int i = 0; i < param.depthCount; i++)
			{
				//�c�ʒu�̎�������
				float depth = startPosition.z + (param.oneCellRect.depth * i);
				depth += halfOneRectDepth;

				//Debug.Log("��" + i.ToString() + "��");
				for (int j = 0; j < param.widthCount; j++)
				{
					//���ʒu�̎�������
					float width = startPosition.x + (param.oneCellRect.width * j);
					width += halfOneRectWidth;

					int index = j + (i * param.widthCount);	//�C���f�b�N�X�ݒ�
					var position = new Vector3(width, param.centerPosition.y, depth); //�ʒu���̌���

					//Cell�̃p�����[�^��ݒ�
					var cellParam = new Cell.Parametor(param.oneCellRect);
					var cell = new Cell(index, cellParam);          //Cell����
					cell.SetPosition(position);                            //Cell�̈ʒu�ύX

					//Debug.Log(position);

					result.Add(cell); //result�ɒǉ�
				}
			}

			return result;
		}
	}
}
