using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FighterAnimator), typeof(FighterAttack), typeof(FighterMove))]
public class Fighter : MonoBehaviour
{
    [SerializeField] public Character playerParams;

    [SerializeField] private Fighter _target;
    [SerializeField] private GameObject _stayPoint;

    private float _maxHP;
    private float _def;
    public float _health;
    private float _speed;
    
    private float _fightDistance;

    private FighterAnimator _animator;
    private FighterAttack _attack;
    private FighterMove _move;
    public FighterUI ui;

    public event UnityAction<Fighter> Died;

    private void Awake()
    {
        _animator = GetComponent<FighterAnimator>();
        _attack = GetComponent<FighterAttack>();
        _move = GetComponent<FighterMove>();
        ui = GetComponentInChildren<FighterUI>();
    }

    private void Start()
    {
        if (playerParams != null)
            InitializeFighter();
        else
            Debug.Log("Something wrong");
    }

    private void InitializeFighter()
    {
        if (IsShotter.Yes == playerParams.Shooter)
            _fightDistance = 15f;
        else
            _fightDistance = 5f;

        _health = playerParams.Health;
        _speed = playerParams.Speed;
        _def = playerParams.DEF;
        _attack.setDamage(playerParams.ATK);

        _maxHP = _health;
        //_ui.SetMaxHp(_maxHP);
        //_ui.UpdateHealth(_health);
    }

    public void CurrentStayPoint(GameObject spawnPoint)
    {
        _stayPoint = spawnPoint;
    }

    public void GetDMG(float damage)
    {
        var damage_after_defense = damage * (_def / 100f);
        _health -= damage_after_defense;

        //_ui.UpdateHealth(_health);
        if (_health <= 0) 
        {
            Die();
        }
    }

    public float GetSpeedValue() 
    {
        return _speed;
    }

    private void Die()
    {
        _animator.Dead();
        Died?.Invoke(this);
    }

    public void SetTarget(Fighter target)
    {
        _target = target;
    }

    public Coroutine FirstSkill()
    {
        return StartCoroutine(LightAttack(_target));
    }

    public Coroutine SecondSkill()
    {
        return StartCoroutine(HeavyAttack(_target));
    }

    public Coroutine ThirdSkill()
    {
        return StartCoroutine(Ultimate(_target));
    }

    private IEnumerator LightAttack(Fighter target)
    {
        _animator.Run();
        yield return _move.StartMove(target.transform, _fightDistance); // ідемо до ворога

        _animator.Idle();
        _attack.SetTarget(target);
        yield return new WaitForSeconds(_animator.StartAttack()); // удар по ворозі

        _animator.Run();
        yield return _move.StartLookAtRotation(_stayPoint.transform); // повертаємося назад
        yield return _move.StartMove(_stayPoint.transform);

        yield return _move.StartRotation(_stayPoint.transform.rotation); // повертаємося в початкову точку 
        _animator.Idle();
    }

    private IEnumerator HeavyAttack(Fighter target)
    {
        _animator.Run();
        yield return _move.StartMove(target.transform, _fightDistance); // ідемо до ворога

        _animator.Idle();
        _attack.SetTarget(target);
        yield return new WaitForSeconds(_animator.StartAttack()); // удар по ворозі

        _animator.Run();
        yield return _move.StartLookAtRotation(_stayPoint.transform); // повертаємося назад
        yield return _move.StartMove(_stayPoint.transform);

        yield return _move.StartRotation(_stayPoint.transform.rotation); // повертаємося в початкову точку 
        _animator.Idle();
    }

    private IEnumerator Ultimate(Fighter target)
    {
        _animator.Run();
        yield return _move.StartMove(target.transform, _fightDistance); // ідемо до ворога

        _animator.Idle();
        _attack.SetTarget(target);
        yield return new WaitForSeconds(_animator.StartAttack()); // удар по ворозі

        _animator.Run();
        yield return _move.StartLookAtRotation(_stayPoint.transform); // повертаємося назад
        yield return _move.StartMove(_stayPoint.transform);

        yield return _move.StartRotation(_stayPoint.transform.rotation); // повертаємося в початкову точку 
        _animator.Idle();
    }
}