using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(selectedUnit == null) { return; }

        selectedUnit.SelectUnitVisual(false);
        selectedUnit = null;
    }

    private void Update()
    {
        // Check to see if user try to press a button
        if (EventSystem.current.IsPointerOverGameObject()) { return; }

        if (TryHandleUnitSelection()) { return ; }

        HandleSelectedUnit();
    }

    private void HandleSelectedUnit()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(selectedUnit == null) { return; }

            selectedUnit.Move(MouseWorld.GetPosition());
        }
    }

    private bool TryHandleUnitSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        // Unit is already selected
                        return false;
                    }

                    // Unit is enemy but is player turn
                    if (unit.IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
                    {
                        return false;
                    }

                    // Unit is player but is enemy turn
                    if (!unit.IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
                    {
                        return false;
                    }

                    // No unit is selectd - First time and when we change turn 
                    // selected unit is null
                    if (selectedUnit != null)
                    {
                        selectedUnit.SelectUnitVisual(false);
                    }

                    selectedUnit = unit;
                    selectedUnit.SelectUnitVisual(true);
                    return true;
                }
            }
        }

        return false;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
