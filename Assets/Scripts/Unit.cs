using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] private Animator unitAnimator;
    private Vector3 targetPosition;
    [SerializeField] private string walkingStringAnimatorName;
    [SerializeField] private bool isEnemy;
    [SerializeField] private GameObject isUnitSelected;
    [SerializeField] private float rotateSpeed = 10f;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        targetPosition = transform.position;
    }

    public void SelectUnitVisual(bool isSelected)
    {
        isUnitSelected.SetActive(isSelected);
    }

    private void Update()
    {
        
        // Move player Units
        if (TurnSystem.Instance.IsPlayerTurn() && !isEnemy)
        {
            //Move();
            //MoveWalkingAnimation();
            // MoveWalkingAnimationImmediatlyRotateToTarget();
             MoveWalkingAnimationSmoothRotateToTarget();
        }
        // Move enemy Units
        else if(!TurnSystem.Instance.IsPlayerTurn() && isEnemy)
        {
            //Move();
            //MoveWalkingAnimation();
            //MoveWalkingAnimationImmediatlyRotateToTarget();
             MoveWalkingAnimationSmoothRotateToTarget();
        }

    }

    private void Move()
    {
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void MoveWalkingAnimation()
    {
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            // Activate Animation for walking
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            // Deactivate Animation for walking
            unitAnimator.SetBool("IsWalking", false);
        }
    }

    private void MoveWalkingAnimationImmediatlyRotateToTarget()
    {
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Instant Rotation towards the target
            transform.forward = moveDirection;

            // Activate Animation for walking
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            // Deactivate Animation for walking
            unitAnimator.SetBool("IsWalking", false);
        }
    }

    private void MoveWalkingAnimationSmoothRotateToTarget()
    {
        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Smooth Rotation towards the target
            //transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime);

            // Smooth Rotation towards the target - use speed variable to make faster rotation towards the target
            float rotateSped = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSped);

            // Activate Animation for walking
            unitAnimator.SetBool(walkingStringAnimatorName, true);
         
        }
        else
        {
            // Deactivate Animation for walking
            unitAnimator.SetBool("IsWalking", false);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }
}
