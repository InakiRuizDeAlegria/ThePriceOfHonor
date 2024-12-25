using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class SetTurnTypeFromPlayerPref : MonoBehaviour
{
    public ActionBasedSnapTurnProvider snapTurn;
    public ActionBasedContinuousTurnProvider continuousTurn;
    public TMP_Dropdown turnTypeDropdown;

    void Start()
    {
        if (PlayerPrefs.HasKey("turn"))
        {
            int savedValue = PlayerPrefs.GetInt("turn");
            turnTypeDropdown.value = savedValue;
        }
        else
        {
            PlayerPrefs.SetInt("turn", 0);
        }

        ApplyPlayerPref();

        turnTypeDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void ApplyPlayerPref()
    {
        if (PlayerPrefs.HasKey("turn"))
        {
            int value = PlayerPrefs.GetInt("turn");
            if (value == 0)
            {
                snapTurn.leftHandSnapTurnAction.action.Enable();
                snapTurn.rightHandSnapTurnAction.action.Enable();
                continuousTurn.leftHandTurnAction.action.Disable();
                continuousTurn.rightHandTurnAction.action.Disable();
            }
            else if (value == 1)
            {
                snapTurn.leftHandSnapTurnAction.action.Disable();
                snapTurn.rightHandSnapTurnAction.action.Disable();
                continuousTurn.leftHandTurnAction.action.Enable();
                continuousTurn.rightHandTurnAction.action.Enable();
            }
        }
    }

    public void OnDropdownValueChanged(int value)
    {
        PlayerPrefs.SetInt("turn", value);
        PlayerPrefs.Save();

        ApplyPlayerPref();
    }
}
