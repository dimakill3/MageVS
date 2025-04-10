using System;
using System.Collections.Generic;
using _Assets.Scripts.Configs;
using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.Events;
using _Assets.Scripts.Spells.Enum;
using TMPro;
using UnityEngine;

namespace _Assets.Scripts.UI.HUD
{
    public class Hud : MonoBehaviour, IDisposable
    {
        [SerializeField] private SpellCellView spellCellView;
        [SerializeField] private RectTransform spellsContainer;
        [SerializeField] private TMP_Text swapSpellLeftText;
        [SerializeField] private TMP_Text swapSpellRightText;
        [SerializeField] private TMP_Text attackKeyText;

        private Dictionary<SpellType, SpellCellView> _cells = new();
        private IEventProvider _eventProvider;

        public void Initialize(IEventProvider eventProvider, SpellConfig[] spellConfigs, KeyCode swapSpellLeft,
            KeyCode swapSpellRight, KeyCode attack)
        {
            _eventProvider = eventProvider;
            foreach (var spellConfig in spellConfigs)
            {
                var cell = Instantiate(spellCellView, spellsContainer);
                cell.Initialize(spellConfig.Sprite, false);
                _cells.Add(spellConfig.SpellType, cell);
            }
            
            swapSpellLeftText.text = swapSpellLeft.ToString();
            swapSpellRightText.text = swapSpellRight.ToString();
            attackKeyText.text = attack.ToString();
            
            _eventProvider.Subscribe<CurrentSpellChanged>(ChangeCurrentSpell);
            _eventProvider.Subscribe<SpellCasted>(HandleSpellCasted);
        }

        public void Dispose()
        {
            _eventProvider.UnSubscribe<CurrentSpellChanged>(ChangeCurrentSpell);
            _eventProvider.UnSubscribe<SpellCasted>(HandleSpellCasted);
            
            foreach (var cell in _cells)
                Destroy(cell.Value.gameObject);
            
            _cells.Clear();
        }

        public void ChangeSpell(SpellType spellType)
        {
            foreach (var cell in _cells)
                cell.Value.SetSelected(cell.Key == spellType);
        }

        private void ChangeCurrentSpell(CurrentSpellChanged currentSpellChanged) =>
            ChangeSpell(currentSpellChanged.CurrentSpellType);

        private void HandleSpellCasted(SpellCasted spellCasted)
        {
            foreach (var cell in _cells)
                cell.Value.StartCooldown(spellCasted.Cooldown);
        }
    }
}