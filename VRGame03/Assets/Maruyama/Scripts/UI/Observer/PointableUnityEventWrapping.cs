using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;
using UnityEngine.Events;
using Oculus.Interaction;

public class PointableUnityEventWrapping : MonoBehaviour
{
    [SerializeField, Interface(typeof(IPointable))]
    private MonoBehaviour _pointable;
    private IPointable Pointable;

    private HashSet<int> _pointers;

    [SerializeField]
    private UnityEvent<PointerEvent> _whenRelease;  //�G�ꂽ�u�Ԃ̃C�x���g

    [SerializeField]
    private UnityEvent<PointerEvent> _whenHover;    //�G��Ă���Ԃ̃C�x���g
    [SerializeField]
    private UnityEvent<PointerEvent> _whenUnhover;  //���ꂽ���̃C�x���g
    [SerializeField]
    private UnityEvent<PointerEvent> _whenSelect;   //�I��(��������)�������̃C�x���g
    [SerializeField]
    private UnityEvent<PointerEvent> _whenUnselect; //�I���I�����̃C�x���g
    [SerializeField]
    private UnityEvent<PointerEvent> _whenMove;     //�G�ꂽ�u�Ԃ��痣���܂ł̃A�b�v�f�[�g�C�x���g
    [SerializeField]
    private UnityEvent<PointerEvent> _whenCancel;

    //�v���p�e�B

    public UnityEvent<PointerEvent> WhenRelease => _whenRelease;

    public UnityEvent<PointerEvent> WhenHover => _whenHover;
    public UnityEvent<PointerEvent> WhenUnhover => _whenUnhover;
    public UnityEvent<PointerEvent> WhenSelect => _whenSelect;
    public UnityEvent<PointerEvent> WhenUnselect => _whenUnselect;
    public UnityEvent<PointerEvent> WhenMove => _whenMove;
    public UnityEvent<PointerEvent> WhenCancel => _whenCancel;

    protected bool _started = false;

    protected virtual void Awake()
    {
        Pointable = _pointable as IPointable;
    }

    protected virtual void Start()
    {
        this.BeginStart(ref _started);
        Assert.IsNotNull(Pointable);
        _pointers = new HashSet<int>();
        this.EndStart(ref _started);
    }

    protected virtual void OnEnable()
    {
        if (_started)
        {
            Pointable.WhenPointerEventRaised += HandlePointerEventRaised;
        }
    }

    protected virtual void OnDisable()
    {
        if (_started)
        {
            Pointable.WhenPointerEventRaised -= HandlePointerEventRaised;
        }
    }

    private void HandlePointerEventRaised(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Hover:
                _whenHover.Invoke(evt);
                _pointers.Add(evt.Identifier);
                break;
            case PointerEventType.Unhover:
                _whenUnhover.Invoke(evt);
                _pointers.Remove(evt.Identifier);
                break;
            case PointerEventType.Select:
                _whenSelect.Invoke(evt);
                break;
            case PointerEventType.Unselect:
                if (_pointers.Contains(evt.Identifier))
                {
                    _whenRelease.Invoke(evt);
                }
                _whenUnselect.Invoke(evt);
                break;
            case PointerEventType.Move:
                _whenMove.Invoke(evt);
                break;
            case PointerEventType.Cancel:
                _whenCancel.Invoke(evt);
                _pointers.Remove(evt.Identifier);
                break;
        }
    }

    #region Inject

    public void InjectAllPointableUnityEventWrapper(IPointable pointable)
    {
        InjectPointable(pointable);
    }

    public void InjectPointable(IPointable pointable)
    {
        _pointable = pointable as MonoBehaviour;
        Pointable = pointable;
    }

    #endregion
}
