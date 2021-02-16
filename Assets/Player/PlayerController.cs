using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public Rigidbody2D _rigidBody { get; private set; }
  public Animator _animator { get; private set; }
  public PlayerInputHandler _playerInputHandler { get; private set; }
  private PlayerStateMachine _playerStateMachine;

  [SerializeField] private float speed;
  [SerializeField] private float wallSlideSpeed = 0.5f;
  [SerializeField] private float jumpForce;

  [SerializeField] public bool isBlocking;

  private int direction;
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
    _playerStateMachine.SetCurrentState(_playerStateMachine.GetState((int)PlayerStatesEnum._IDLE_));
  }
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    _currentVelocity = _rigidBody.velocity;
    _playerStateMachine.Update();
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
    Debug.Log("ATACANDO");
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
}
