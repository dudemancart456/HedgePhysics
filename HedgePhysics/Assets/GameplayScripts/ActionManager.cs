using UnityEngine;
using System.Collections;

public class ActionManager : MonoBehaviour {


    public int Action { get; set; }

    //Action Scrips, Always leave them in the correct order;

    public Action00_Regular Action00;
    public Action01_Jump Action01;
    public Action02_Homing Action02;
    public Action03_SpinDash Action03;
    public HomingAttackControl Action02Control;
    public Action04_Hurt Action04;
    public HurtControl Action04Control;
    public Action05_Rail Action05;

    //Etc

    PlayerBhysics Phys;

    void Start()
    {
        Phys = GetComponent<PlayerBhysics>();
        ChangeAction(0);
    }

    void DeactivateAllActions()
    {
        //Put all actions here
        //Also put variables that you want re-set out of actions
        Action00.enabled = false;
        Action01.enabled = false;
        Action02.enabled = false;
        Action02.ResetHomingVariables();
        Action03.enabled = false;
        Action03.ResetSpinDashVariables();
        Action04.enabled = false;
        Action05.enabled = false;
    }

    //Call this function to change the action

    public void ChangeAction(int ActionToChange)
    {

        Action = ActionToChange;
        DeactivateAllActions();

        //Put an case for all your actions here
        switch (ActionToChange)
        {
            case 0:
                Action00.enabled = true;
                break;
            case 1:
                Action01.enabled = true;
                break;
            case 2:
                Action02.enabled = true;
                break;
            case 3:
                Action03.enabled = true;
                break;
            case 4:
                Action04.enabled = true;
                break;
            case 5:
                Action05.enabled = true;
                break;
            default:
                Debug.Log("Action is not available.");
                break;
        }

    }

}
