using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field:Header("References")]
    [field:SerializeField]public PlayerSO data {  get; private set; }
    public Rigidbody rigidbody { get;private set; }
    public Transform mainCameraTransform { get; private set; }
    public PlayerInput input { get; private set; }

    private PlayerMovementStateMachine movementStateMachine;
    private void Awake()
    {
        movementStateMachine=new PlayerMovementStateMachine(this);
        input = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();
        mainCameraTransform = Camera.main.transform;
    }
    private void Start()
    {
        
        movementStateMachine.ChangeState(movementStateMachine.idlingState);
    }
    private void Update()
    {
        movementStateMachine.HandelInput();
    }
    private void FixedUpdate()
    {
        movementStateMachine.PhysicsUpdate();
    }
    
}
