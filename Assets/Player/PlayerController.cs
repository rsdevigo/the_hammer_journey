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
  [SerializeField] private float jumpForce;

  private int direction;
  public Vector2 _currentVelocity;
  private Vector2 _workspace;

  [SerializeField] private Transform _groundCheckPosition;
  [SerializeField] private float _groundedRadius;
  [SerializeField] private LayerMask _groundMask;

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

  public void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(_groundCheckPosition.position, _groundedRadius);
  }
}
