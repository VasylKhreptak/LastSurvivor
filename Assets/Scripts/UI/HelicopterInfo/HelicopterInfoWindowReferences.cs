using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HelicopterInfo
{
    [Serializable]
    public class HelicopterInfoWindowReferences
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TMP_Text _incomeMultiplierText;
        [SerializeField] private GameObject _fuelTankTextRoot;
        [SerializeField] private TMP_Text _fuelTankText;
        [SerializeField] private Button _playButton;

        public GameObject Root => _root;
        public TMP_Text IncomeMultiplierText => _incomeMultiplierText;
        public TMP_Text FuelTankText => _fuelTankText;
        public Button PlayButton => _playButton;
    }
}