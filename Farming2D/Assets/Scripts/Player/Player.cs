using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public float Speed { get => _speed; set { _speed = value; } }

    [SerializeField] private float _speed;

    private Vector2 _moveDirection;
    private Rigidbody2D _rb;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        _rb.MovePosition((Vector2)transform.position + _speed * Time.fixedDeltaTime * _moveDirection);
    }
}