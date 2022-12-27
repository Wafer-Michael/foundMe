using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GraphType = SparseGraph<AstarNode, AstarEdge>;

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

		public bool IsInRect(Vector3 position)
		{
			var rectStartPosition = CalculateStartPosition();

			if (position.x >= rectStartPosition.x &&
				position.x <= rectStartPosition.x + this.width &&
				position.z >= rectStartPosition.z &&
				position.z <= rectStartPosition.z + this.depth
			){
				return true;
			}

			return false;
		}

	}

}


namespace Factory
{
	public class WayPointsMap_FloodFill : MonoBehaviour
	{
		//�����}�b�v
		readonly Dictionary<DirectionType, Vector3> DIRECTION_MAP = new Dictionary<DirectionType, Vector3>() {
			{ DirectionType.Right,         Vector3.right },							//�E
			{ DirectionType.RightForward,	(Vector3.right + Vector3.forward)},		//�E��
			{ DirectionType.RightBack,      (Vector3.right - Vector3.forward)},		//�E��O

			{ DirectionType.Left,          -Vector3.right},							//��
			{ DirectionType.LeftForward,   (-Vector3.right + Vector3.forward) },	//����
			{ DirectionType.LeftBack,      (-Vector3.right - Vector3.forward) },	//����O

			{ DirectionType.Foward,        Vector3.forward},						//����
			{ DirectionType.Back,          -Vector3.forward},						//����O
		};

		/// <summary>
		/// �������̐i�s�^�C�v
		/// </summary>
		enum DirectionType
		{
			Right,
			RightForward,
			RightBack,
			Left,
			LeftForward,
			LeftBack,
			Foward,
			Back,
		}

		/// <summary>
		/// �����^�C�v�ʃf�[�^
		/// </summary>
		struct DataByDirectionType
		{
			public Vector3 direction; //����
			public int plusIndex;  //���Z����m�[�h�C���f�b�N�X
		};

		/// <summary>
		/// �����p�̃f�[�^
		/// </summary>
		struct OpenData
		{
			public AstarNode parentNode;    //�����̑O�̃m�[�h
			public AstarNode selfNode;    //�������g�̃m�[�h
			public bool isActive;                          //�m�[�h�������Ă��邩�ǂ���

			public OpenData(
				AstarNode parentNode,
				AstarNode selfNode
			) {
				this.parentNode = parentNode;
				this.selfNode = selfNode;
				this.isActive = true;
			}
		}

		/// <summary>
		/// �p�����[�^
		/// </summary>
		public struct Parametor
		{
			public float intervalRange;     //�m�[�h�̊Ԋu����(5.0f)
			public maru.Rect rect;          //�l�p�f�[�^
			public float createHeight;      //�����ݒ�(0.5f)
		};

		Queue<OpenData> m_openDataQueue = new Queue<OpenData>();    //�I�[�v���f�[�^�L���[
		Dictionary<DirectionType, int> m_plusIndexMapByDirection = new Dictionary<DirectionType, int>();   //�����ʂ̉��Z����C���f�b�N�X��

		/// <summary>
		/// �����f�[�^�ɍ��킹���C���f�b�N�X�̏����ݒ肷��B
		/// </summary>
		/// <param name="parametor">�p�����[�^</param>
		/// <returns>�����f�[�^�ɍ��킹���C���f�b�N�X�̏����ݒ肷��</returns>
		private Dictionary<DirectionType, int> SettingIndexByDirection(Parametor parametor)
		{
			Dictionary<DirectionType, int> result = new Dictionary<DirectionType, int>();

			maru.Rect rect = parametor.rect;
			float intervalRange = parametor.intervalRange;

			//��ƂȂ鉡�̑傫���ƁA�c�̑傫��
			int widthCount = (int)(rect.width / intervalRange);
			int plusIndex = widthCount + 1; //���̒����������ŉ��s�����̃C���f�b�N�X�ɂȂ�B

			result[DirectionType.Right] = +1;
			result[DirectionType.RightForward] = 1 + plusIndex;
			result[DirectionType.RightBack] = 1 - plusIndex;
			result[DirectionType.Left] = -1;
			result[DirectionType.LeftForward] = -1 + plusIndex;
			result[DirectionType.LeftBack] = -1 - plusIndex;
			result[DirectionType.Foward] = +plusIndex;
			result[DirectionType.Back] = -plusIndex;

			return result;
		}

		/// <summary>
		/// ����WayPoint�������ł��邩�ǂ����𔻒f����B
		/// </summary>
		/// <param name="newOpenData">�V�K�f�[�^</param>
		/// <param name="graph">�O���t</param>
		/// <param name="parametor">�����p�����[�^</param>
		/// <param name="isRayHit">��Q���ɓ����������ǂ������Q�Ƃ���bool�ɕۑ�����</param>
		/// <returns>�����ł���Ȃ�true</returns>
		private bool IsNodeCreate(
			OpenData newOpenData,
			GraphType graph,
			Parametor parametor,
			ref bool isRayHit
		)
		{
			var startPosition = newOpenData.parentNode.GetPosition();
			var targetPosition = newOpenData.selfNode.GetPosition();

			//�^�[�Q�b�g���G���A���O���ɂ���Ȃ�
			int testIndex = newOpenData.selfNode.GetIndex();
			var selfPosition = newOpenData.selfNode.GetPosition();
			if (!parametor.rect.IsInRect(newOpenData.selfNode.GetPosition())) {
				return false;
			}

			//��Q���ɓ������Ă�����(��ɏ�Q����������Ȃ��ƁA�G�b�W�Ƌ��L���Ă��邽�߃o�O��(�C��������))
			//��Q���̃��C���[����
			if (isRayHit = maru.UtilityObstacle.IsRayObstacle(startPosition, targetPosition)) {
				return false;   //�����ł��Ȃ�
			}

			//�����m�[�h�����݂���Ȃ�
			if (graph.IsSomeIndexNode(newOpenData.selfNode.GetIndex())) {
				return false;
			}

			return true;    //�ǂ̏����ɂ����Ă͂܂�Ȃ��Ȃ�true
		}

		/// <summary>
		/// �G�b�W�������ł��邩�ǂ����𔻒f����B
		/// </summary>
		/// <param name="newOpenData">�V�K�f�[�^</param>
		/// <param name="graph">�O���t</param>
		/// <param name="parametor">�����p�����[�^</param>
		/// <param name="isRayHit">��Q���Ƀq�b�g�������ǂ���</param>
		/// <returns>�����ł���Ȃ�true</returns>
		private bool IsEdgeCreate(
			OpenData newOpenData,
			GraphType graph,
			Parametor parametor,
			bool isRayHit
		) {
			//��Q���ɓ������Ă���Ȃ�A�������Ȃ�
			if (isRayHit)
			{
				return false;
			}

			//�^�[�Q�b�g���G���A���O���ɂ���Ȃ�
			int testIndex = newOpenData.selfNode.GetIndex();
			var selfPosition = newOpenData.selfNode.GetPosition();
			if (!parametor.rect.IsInRect(newOpenData.selfNode.GetPosition()))
			{
				return false;
			}

			//�����G�b�W�����݂���Ȃ�
			var parentNode = newOpenData.parentNode;
			var selfNode = newOpenData.selfNode;
			if (graph.IsSomeIndexEdge(parentNode.GetIndex(), selfNode.GetIndex()))
			{
				return false;   //�������Ȃ�
			}

			return true;    //�������ʂ������߁A��������B
		}

		/// <summary>
		/// �E�F�C�|�C���g�̕�������
		/// </summary>
		/// <param name="startPosition">�J�n�ʒu</param>
		/// <param name="parametor">�����p�����[�^</param>
		private void CreateWayPoints(
			OpenData parentOpenData,
			GraphType graph,
			Parametor parametor
		){
			var openDatas = CreateChildrenOpenDatas(parentOpenData, parametor);    //�I�[�v���f�[�^�̐���

			//���[�v���āA�I�[�v���f�[�^�̒����琶���ł�����̂�ݒ�
			foreach (var openData in openDatas) {
				var parentNode = openData.parentNode;
				var selfNode = openData.selfNode;
				bool isRayHit = false;  //��Q���ɓ����������ǂ������L�^����B

				//�m�[�h�������ł���Ȃ�
				if (IsNodeCreate(openData, graph, parametor,ref isRayHit))
				{
					var node = graph.AddNode(openData.selfNode); //�O���t�Ƀm�[�h�ǉ�
					m_openDataQueue.Enqueue(openData);                 //�����L���[��OpenData��ǉ�
				}

				//�G�b�W�̐���������������Ă���Ȃ�
				if (IsEdgeCreate(openData, graph, parametor, isRayHit))
				{
					var fromNode = graph.GetNode(parentNode.GetIndex());
					var toNode = graph.GetNode(selfNode.GetIndex());
					graph.AddEdge(new AstarEdge(fromNode, toNode));   //�O���t�ɃG�b�W�ǉ�
				}
			}
		}

		/// <summary>
		/// �C���f�b�N�X���v�Z���ĕԂ�
		/// </summary>
		/// <param name="parentOpenData">�e�ƂȂ�I�[�v���f�[�^</param>
		/// <param name="directionType">������������f�[�^</param>
		/// <returns></returns>
		int CalculateIndex(OpenData parentOpenData, DirectionType directionType)
        {
			var parent = parentOpenData.selfNode;
			int index = parent.GetIndex() + m_plusIndexMapByDirection[directionType];   //�C���f�b�N�X�̌v�Z
			return index;
		}

		/// <summary>
		/// ��������OpenData�𐶐�����B
		/// </summary>
		/// <param name="parentOpenData">�e�ƂȂ�I�[�v���f�[�^</param>
		/// <param name="parametor">�p�����[�^</param>
		/// <returns>��������OpenData�𐶐�����</returns>
		private List<OpenData> CreateChildrenOpenDatas(
			OpenData parentOpenData,
			Parametor parametor
        ) {
			List<OpenData> result = new List<OpenData>();
			var parent = parentOpenData.selfNode;	//�e�m�[�h���擾

			foreach (var pair in DIRECTION_MAP) {
				DirectionType directionType = pair.Key;	//�����^�C�v
				Vector3 direction = pair.Value;				//�����x�N�g��

				int index = CalculateIndex(parentOpenData, directionType);			//�C���f�b�N�X�̌v�Z
				if (index < 0) {	//�C���f�b�N�X��0��菬�����Ȃ珈�����΂��B
					continue;
				}

				Vector3 startPosition = parent.GetPosition();			//�J�n�ʒu
				Vector3 targetPosition = startPosition + (direction * parametor.intervalRange);	//�����ʒu

				var newNode = new AstarNode(index, targetPosition);	//�V�K�m�[�h�̍쐬

				var newOpenData = new OpenData(parent, newNode);		//�V�K�f�[�^�쐬
				result.Add(newOpenData);
			}

			return result;
        }

		/// <summary>
		/// �E�F�C�|�C���g�̐���
		/// </summary>
		/// <param name="startPosition">�J�n�ʒu</param>
		/// <param name="graph">�����������O���t</param>
		/// <param name="parametor">�����p�����[�^</param>
		public void AddWayPointMap(
			GraphType graph,
			Parametor parametor
		){
			m_plusIndexMapByDirection = SettingIndexByDirection(parametor); //�����ʂ̉��Z����C���f�b�N�X�����Z�b�e�B���O

			var baseStartPosition = parametor.rect.CalculateStartPosition();

			m_openDataQueue.Clear();
			var newNode = new AstarNode(0, baseStartPosition);
			//Debug::GetInstance()->Log(newNode->GetPosition());
			graph.AddNode(newNode);
			m_openDataQueue.Enqueue(new OpenData(null, newNode));

			while (m_openDataQueue.Count != 0)
			{   //�L���[����ɂȂ�܂�
				var parentData = m_openDataQueue.Dequeue();
				//m_openDataQueue.pop();
				CreateWayPoints(parentData, graph, parametor);
			}
		}

	}
}

