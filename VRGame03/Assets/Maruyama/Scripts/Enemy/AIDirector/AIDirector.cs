using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : SingletonMonoBehaviour<AIDirector>
{
    private List<EnemyBase> m_members = new List<EnemyBase>();

    [Header("�t�B�[���h�̃G���A�}�b�v"), SerializeField]
    private FieldCellMap m_areaMap;

    [Header("�t�B�[���h�̃E�F�C�|�C���g�}�b�v"), SerializeField]
    private FieldWayPointsMap m_wayPointsMap;

    [Header("�t�B�[���h�̉e���Z���}�b�v"), SerializeField]
    private FieldImpactCellMap m_impactCellMap;

    private List<FactionCoordinator> m_factionCoordinators = new List<FactionCoordinator>();    //�t�@�N�V�����R�[�f�B�l�[�^�[�Q

    protected override void Awake()
    {
        base.Awake();

        NullCheck();
    }

    public void Start()
    {
        //�E�F�C�|�C���g�ɃG���A�������蓖�Ă�B
        SettingArea();
    }

    private void Update()
    {
        //�t�@�N�V�����R�[�f�B�l�[�^�[�̍X�V
        foreach(var faction in m_factionCoordinators)
        {
            faction.OnUpdate();
        }
    }

    //--------------------------------------------------------------------------------------
    /// �A�N�Z�b�T
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// �t�@�N�V�����R�[�f�B�l�[�^�̐���(�����I�ɉ���)
    /// </summary>
    private FactionCoordinator CreateFactionCoordinator()
    {
        var newFaction = new FactionCoordinator();
        newFaction.OnCreate();
        newFaction.OnStart();   //�J�n���ɌĂяo����������(�����I�ɕʂ̏ꏊ�ŌĂԂ���)

        return newFaction;
    }

    /// <summary>
    /// �t�@�N�V�����R�[�f�B�l�[�^�̒ǉ�
    /// </summary>
    /// <param name="factionCoordinator">�ǉ��������t�@�N�V����</param>
    private void AddFactionCoordinator(FactionCoordinator factionCoodinator)
    {
        m_factionCoordinators.Add(factionCoodinator);
    }

    /// <summary>
    /// �t�@�N�V�����R�[�f�B�l�[�^�[�̍폜
    /// </summary>
    /// <param name="factionCoordinator">�폜�������t�@�N�V����</param>
    public void RemoveFactionCoordinator(FactionCoordinator factionCoordinator)
    {
        factionCoordinator.OnExit();
        m_factionCoordinators.Remove(factionCoordinator);
    }

    public List<FactionCoordinator> GetFactionCoordinators() { return m_factionCoordinators; }

    public CellMap<Cell> GetAreaCellMap() { return m_areaMap.GetCellMap(); }

    public WayPointsMap GetWayPointsMap() { return m_wayPointsMap.GetWayPointsMap(); }

    public Factory.WayPointsMap_FloodFill.Parametor GetFieldWayPointsMap_FactoryParametor() { return m_wayPointsMap.GetFactoryParametor(); }

    public CellMap<ImpactCell> GetImpactCellMap() { return m_impactCellMap.GetCellMap(); }

    //--------------------------------------------------------------------------------------
    /// �����Z�b�e�B���O
    //--------------------------------------------------------------------------------------

    /// <summary>
    /// �G���A�̃Z�b�e�B���O
    /// </summary>
    private void SettingArea()
    {
        foreach (var node in m_wayPointsMap.GetWayPointsMap().GetGraph().GetNodes())
        {
            foreach (var area in m_areaMap.GetCellMap().GetCells())
            {
                if (area.GetParametor().rect.IsInRect(node.GetPosition()))
                {
                    node.SetParent(area);   //�G���A��e�ɐݒ肷��B
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Null�m�F
    /// </summary>
    private void NullCheck()
    {
        //�G�l�~�[��S�Ĕz��ɓ����
        if (m_members.Count == 0)
        {
            m_members = new List<EnemyBase>(FindObjectsOfType<EnemyBase>());
        }

        //�G���A�p�̃t�B�[���h�Z���}�b�v���擾
        if (m_areaMap == null)
        {
            m_areaMap = GetComponentInChildren<FieldCellMap>();
        }

        //�E�F�C�|�C���g�}�b�v���擾
        if (m_wayPointsMap == null)
        {
            m_wayPointsMap = GetComponentInChildren<FieldWayPointsMap>();
        }

        //�e���}�b�v���擾
        if(m_impactCellMap == null)
        {
            m_impactCellMap = GetComponentInChildren<FieldImpactCellMap>();
        }
    }

}
