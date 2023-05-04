using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Vector3 targetPosition;

    [SerializeField] private bool isEnemy;
    [SerializeField] private GameObject isUnitSelected;

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
            Move();
        }
        // Move enemy Units
        else if(!TurnSystem.Instance.IsPlayerTurn() && isEnemy)
        {
            Move();    
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

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }
}
