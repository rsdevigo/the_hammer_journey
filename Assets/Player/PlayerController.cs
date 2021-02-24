using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
  public Rigidbody2D _rigidBody { get; private set; }
  public Animator _animator { get; private set; }
  public PlayerInputHandler _playerInputHandler { get; private set; }
  private PlayerStateMachine _playerStateMachine;

  [SerializeField] private float speed;
  [SerializeField] private float wallSlideSpeed = 0.5f;
  [SerializeField] private float jumpForce;

  [SyncVar(hook = nameof(SyncBlocking))] private bool _isBlocking;
  [SerializeField]
  public bool isBlocking
  {
    get
    {
      return _isBlocking;
    }
    set
    {
      if (hasAuthority)
      {
        CmdBlocking(value);
      }
    }
  }
  [SyncVar(hook = nameof(SyncHealth))] private int _health;

  public int health
  {
    get
    {
      return _health;
    }
    set
    {
      if (hasAuthority)
      {
        CmdHealth(value);
      }
    }
  }

  [SyncVar(hook = nameof(SyncHited))] private bool _isHited;
  public bool isHited
  {
    get
    {
      return _isHited;
    }
    set
    {
      if (hasAuthority)
      {
        CmdHited(value);
      }
    }
  }

  [SyncVar(hook = nameof(SyncDead))] private bool _isDead;
  public bool isDead
  {
    get
    {
      return _isDead;
    }
    set
    {
      if (hasAuthority)
      {
        CmdDead(value);
      }
    }
  }

  private int direction;
  [SerializeField] private float timeToResetHealth = 2.0f;
  private float currentResetHealthTime = 0;
  public Vector2 _currentVelocity;
  private Vector2 _workspace;

  [SerializeField] private Transform _groundCheckPosition;
  [SerializeField] private float _groundedRadius;
  [SerializeField] private LayerMask _groundMask;

  [SerializeField] private Transform _wallCheckPosition;
  [SerializeField] private float _wallLineSize = 0.1f;
  [SerializeField] private LayerMask _wallMask;

  [SerializeField] private Transform _attackCheckPosition;
  [SerializeField] private float _attackRadius;
  [SerializeField] private LayerMask _attackMask;

  void Awake()
  {
    _animator = GetComponent<Animator>();
    _rigidBody = GetComponent<Rigidbody2D>();
    _playerInputHandler = GetComponent<PlayerInputHandler>();
    _playerStateMachine = new PlayerStateMachine();
    _playerStateMachine.Add((int)PlayerStatesEnum._IDLE_, new IdlePlayerState(_playerStateMachine, this, "Idle"));
    _playerStateMachine.Add((int)PlayerStatesEnum._MOVING_, new MovingPlayerState(_playerStateMachine, this, "Moving"));
    _playerStateMachine.Add((int)PlayerStatesEnum._INAIR_, new AirPlayerState(_playerStateMachine, this, "Jumping"));
    _playerStateMachine.Add((int)PlayerStatesEnum._JUMPING_, new JumpingPlayerState(_playerStateMachine, this, "Jumping"));
    _playerStateMachine.Add((int)PlayerStatesEnum._ATTACK_, new AttackPlayerState(_playerStateMachine, this, "Attacking"));
    _playerStateMachine.Add((int)PlayerStatesEnum._BLOCK_, new BlockPlayerState(_playerStateMachine, this, "Idle"));
    _playerStateMachine.Add((int)PlayerStatesEnum._WALLSLIDING_, new WallSlidePlayerState(_playerStateMachine, this, "Sliding"));
    _playerStateMachine.Add((int)PlayerStatesEnum._HITED_, new HitPlayerState(_playerStateMachine, this, "Hited"));
    _playerStateMachine.Add((int)PlayerStatesEnum._DEAD_, new DeadPlayerState(_playerStateMachine, this, "Dead"));
    _playerStateMachine.SetCurrentState(_playerStateMachine.GetState((int)PlayerStatesEnum._IDLE_));
  }
  // Start is called before the first frame update

  public override void OnStartClient()
  {
    SyncHealth(_health, 2);
    SyncHited(_isHited, false);
    SyncDead(_isDead, false);
    base.OnStartClient();
  }

  public override void OnStartServer()
  {
    SyncHealth(_health, 2);
    SyncHited(_isHited, false);
    SyncDead(_isDead, false);
    base.OnStartServer();
  }

  public override void OnStartAuthority()
  {
    base.OnStartAuthority();

    GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
  }

  // Update is called once per frame
  void Update()
  {
    _currentVelocity = _rigidBody.velocity;
    _playerStateMachine.Update();

    if (health == 1 && isLocalPlayer)
    {
      currentResetHealthTime -= Time.deltaTime;
      if (currentResetHealthTime <= 0.0)
      {
        health = 2;
      }
    }
  }

  void FixedUpdate()
  {
    _playerStateMachine.FixedUpdate();
  }



  public void Move(float move)
  {
    _workspace.Set(move * speed * 5f, _currentVelocity.y);
    _rigidBody.velocity = _workspace;
    _currentVelocity = _workspace;

    if (move > 0)
    {
      direction = 1;
    }
    else if (move < 0)
    {
      direction = -1;
    }

    Flip();
  }

  public void Flip()
  {
    if (direction == -1)
    {
      transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    else if (direction == 1)
    {
      transform.rotation = Quaternion.identity;
    }
  }

  public void Jump()
  {
    _workspace.Set(_currentVelocity.x, jumpForce);
    _rigidBody.velocity = _workspace;
    _currentVelocity = _workspace;
  }

  public void Attack()
  {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCheckPosition.position, _attackRadius, _attackMask);
    GameObject[] gameObjectsViewed = new GameObject[10];
    int numberOfGameObject = 0;
    foreach (Collider2D collider in colliders)
    {
      if (collider.gameObject != gameObject)
      {
        bool foundGameObject = false;
        for (int i = 0; i < numberOfGameObject; i++)
        {
          if (gameObjectsViewed[i] == collider.gameObject)
          {
            foundGameObject = true;
            break;
          }
        }

        if (!foundGameObject)
        {
          gameObjectsViewed[numberOfGameObject++] = collider.gameObject;
          CmdAttack(collider.gameObject, direction);
        }
      }
    }
  }

  public void Slide()
  {
    _workspace.Set(0, -wallSlideSpeed);
    _rigidBody.velocity = _workspace;
    _currentVelocity = _workspace;
  }

  public bool CheckIfTouchWall()
  {
    Vector2 movementInput = _playerInputHandler.movementInput;
    Vector2 direction;
    if (movementInput.x == 0.0f)
    {
      return false;
    }

    if (movementInput.x > 0)
    {
      direction = Vector2.right;
    }
    else
    {
      direction = Vector2.left;
    }
    RaycastHit2D collider = Physics2D.Raycast(_wallCheckPosition.position, direction, _wallLineSize, _wallMask);
    if (collider)
    {
      return true;
    }

    return false;
  }

  public bool CheckIfTouchGround()
  {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheckPosition.position, _groundedRadius, _groundMask);
    foreach (Collider2D collider in colliders)
    {
      if (collider.gameObject != gameObject)
      {
        return true;
      }
    }

    return false;
  }

  public bool CheckAttackPosition()
  {
    return _attackCheckPosition.gameObject.activeSelf;
  }



  public void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(_groundCheckPosition.position, _groundedRadius);

    Gizmos.DrawWireSphere(_attackCheckPosition.position, _attackRadius);

    Gizmos.DrawLine(_wallCheckPosition.position, _wallCheckPosition.position + new Vector3(_wallLineSize, 0, 0));
  }

  [Command]
  private void CmdAttack(GameObject target, int dir)
  {
    PlayerController playerTarget = target.GetComponent<PlayerController>();
    if (!playerTarget.isBlocking)
    {
      TargetOnHit(target.GetComponent<NetworkIdentity>().connectionToClient, dir);
    }
    else
    {
      Debug.Log("Bloqueando");
    }
  }

  [TargetRpc]
  private void TargetOnHit(NetworkConnection target, int dir)
  {
    PlayerController playerTarget = target.identity.GetComponent<PlayerController>();
    playerTarget._rigidBody.AddForce(new Vector2(10.0f * dir, 2.0f), ForceMode2D.Impulse);
    playerTarget.isHited = true;
    playerTarget.health -= 1;
  }

  private void SyncHealth(int oldValue, int newValue)
  {
    _health = newValue;
    if (_health == 1)
    {
      currentResetHealthTime = timeToResetHealth;
      GetComponent<SpriteRenderer>().material = GetComponent<PlayerMaterials>().materials[1];
    }
    else if (_health == 2)
    {
      GetComponent<SpriteRenderer>().material = GetComponent<PlayerMaterials>().materials[0];
    }
    else if (_health <= 0)
    {
      isDead = true;
    }
  }

  [Command]
  private void CmdHealth(int value)
  {
    SyncHealth(_health, value);
  }

  private void SyncHited(bool oldValue, bool newValue)
  {
    _isHited = newValue;
  }

  [Command]
  private void CmdHited(bool value)
  {
    SyncHited(_isHited, value);
  }

  private void SyncDead(bool oldValue, bool newValue)
  {
    _isDead = newValue;
  }

  [Command]
  private void CmdDead(bool value)
  {
    SyncDead(_isDead, value);
  }

  private void SyncBlocking(bool oldValue, bool newValue)
  {
    Debug.Log(netId);
    Debug.Log(newValue);
    _isBlocking = newValue;
  }

  [Command]
  private void CmdBlocking(bool value)
  {
    SyncBlocking(_isBlocking, value);
  }
}
