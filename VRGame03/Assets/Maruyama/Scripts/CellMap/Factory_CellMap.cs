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
			public int widthCount;				//���ɓW�J����Z���̐��B
			public int depthCount;				//���s���ɓW�J����Z���̐��B
			public Vector3 centerPosition;		//�}�b�v�̒��S�ʒu�B

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
			return CreateCells<Cell>(param);
		}

		/// <summary>
		/// �Z���}�b�v�̐���
		/// </summary>
		/// <typeparam name="T">�Z���^�C�v</typeparam>
		/// <param name="param">�����p�����[�^</param>
		/// <returns>�������ꂽ�Z���z��</returns>
		public static List<T> CreateCells<T>(Parametor param)
			where T : Cell
		{
			var result = new List<T>();

			float halfOneRectWidth = param.oneCellRect.width * 0.5f;
			float halfOneRectDepth = param.oneCellRect.depth * 0.5f;

			float fieldRectWidth = (float)param.widthCount * param.oneCellRect.width;
			float fieldRectDepth = (float)param.depthCount * param.oneCellRect.depth;
			var fieldRect = new maru.Rect(param.centerPosition, fieldRectWidth, fieldRectDepth);
			var startPosition = fieldRect.CalculateStartPosition(); //�}�b�v�̍���̌��_���擾

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

					int index = j + (i * param.widthCount); //�C���f�b�N�X�ݒ�
					var position = new Vector3(width, param.centerPosition.y, depth); //�ʒu���̌���

					//Cell�̃p�����[�^��ݒ�
					var cellParam = new Cell.Parametor(param.oneCellRect);
					var cell = maru.Generic.Construct<T, int, Cell.Parametor>(index, cellParam);			//Cell����
					cell.SetPosition(position);                                 //Cell�̈ʒu�ύX

					//��Q���ɏd�Ȃ��Ă���ꍇ�́A��A�N�e�B�u�Z���ɕύX
					float quadOneRectWidth = halfOneRectWidth * 0.5f;	//1/4�X�P�[�������܂��Ă�����A��A�N�e�B�u�ɂ���B
					float sphereRange = quadOneRectWidth;
					var obstacleLayer = LayerMask.GetMask(maru.UtilityObstacle.DEFAULT_RAY_OBSTACLE_LAYER_STRINGS);
					var colliders = Physics.OverlapSphere(position, sphereRange, obstacleLayer);  //�I�u�W�F�N�g�����ɑ��݂��邩�ǂ���
					if(colliders.Length != 0) {		//��ł�����Ȃ�A���܂��Ă���
						cell.SetIsActive(false);    //��A�N�e�B�u��Ԃɂ���B
						var impactCell = cell as ImpactCell;
                        if (impactCell != null) {
							impactCell.SetDangerValue(0);
                        }
					}

					result.Add(cell); //result�ɒǉ�
				}
			}

			return result;
		}
	}
}
