using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// @author Benjamin van Kessel
/// 10/12/2024
/// This script manages the UI elements that the player interacts with for the math aspect of the game.
/// Link to documentation: N.A.
/// </summary> 
/// 
public class FormulaUIManager : MonoBehaviour
{
    private GameObject[] allZones;
    public GameObject formulaUI;
    public TMP_InputField xText;
    public TMP_InputField yText;
    private string _mX;
    private string _mY;
    [SerializeField] private List<char> acceptedChars;
    [HideInInspector] public SolutionZone currentSolutionZone = null;

    private void Start()
    {
        allZones = GameObject.FindGameObjectsWithTag("SolutionZone");
        xText.onValidateInput += delegate (string input, int charIndex, char addedChar) { return ValidateInputs(addedChar); };
        yText.onValidateInput += delegate (string input, int charIndex, char addedChar) { return ValidateInputs(addedChar); };
    }

    private void Update()
    {
        if(_mX != xText.text || _mY != yText.text)
        {
            _mX = xText.text;
            _mY = yText.text;
            //Debug.Log($"X:{_mX} & Y:{_mY}");
            foreach (GameObject zone in allZones)
            {
                SolutionZone script = zone.GetComponent<SolutionZone>();
                script.UpdateGraph(_mX, _mY);
            }
        }
    }

    private char ValidateInputs(char charToValidate)
    {
        if (!acceptedChars.Contains(charToValidate)) charToValidate = '\0';
        return charToValidate;
    }

    public void ToggleFormulaUI(bool active)
    {
        formulaUI.SetActive(active);
    }

    public void InitializeFormulas(string X, string Y, SolutionZone zone)
    {
        xText.text = X;
        yText.text = Y;
        _mX = X;
        _mY = Y;
    }

    public void ConfirmBtnPressed()
    {
        foreach(GameObject zone in allZones)
        {
            SolutionZone script = zone.GetComponent<SolutionZone>();
            script.TrySolution();
        }
    }

    public void ClearInputs()
    {
        yText.text = "";
        xText.text = "";
    }
}
