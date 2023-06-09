using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/State")]
public class State : ScriptableObject
{
    public Action[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.gray;

    /// <summary>
    /// 현재 State의 행동들을 Update
    /// </summary>
    public void ExecuteState(EnemyController controller)
    {
        foreach (var action in actions)
        {
            action.ExecuteAction(controller);
        }
    }

    /// <summary>
    /// State가 변경되었을 때, 처음 실행될 함수
    /// </summary>
    public void OnEnabledState(EnemyController controller)
    {
        foreach (var action in actions)
        {
            action.OnEnabledAction(controller);
        }

        foreach (var transition in transitions)
        {
            transition.OnEnabledTransition(controller);
        }
    }

    /// <summary>
    /// State가 변경 될 때, 실행될 함수
    /// </summary>
    public void ExitState(EnemyController controller)
    {
        foreach (var action in actions)
        {
            action.ExitAction(controller);
        }
    }


    /// <summary>
    /// State 전이 함수
    /// </summary>
    public void CheckTransitions(EnemyController controller)
    {
        foreach (var transition in transitions)
        {
            bool isTransition = transition.CheckTransition(controller);
            if (isTransition == true)
            {
                if (controller.currentState != transition.trueState)
                {
                    controller.currentState.ExitState(controller);
                    controller.TransitionToState(transition.trueState);
                }
            }
            /*else
            {
                if (controller.currentState != transition.falseState)
                {
                    controller.currentState.ExitState(controller);
                    controller.TransitionToState(transition.falseState);
                }
            }*/

            if (controller.currentState != this)
            {
                controller.currentState.OnEnabledState(controller);
                break;
            }
        }
    }
    
    
}
