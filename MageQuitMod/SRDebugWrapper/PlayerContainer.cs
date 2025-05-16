using UnityEngine;
using System.ComponentModel;
using static SROptions;

namespace MageQuitMod.SRDebugWrapper
{
    public class PlayerOptions : IOptionContainerWrapper
    {
        public string CategoryName { get; set; }

        public PlayerContainer OptionContainer { get; }
        object IOptionContainerWrapper.OptionContainer => OptionContainer;

        public PlayerOptions(Player player)
        {
            CategoryName = player.name;

            OptionContainer = new PlayerContainer(player);
        }
    }


    public class PlayerContainer
    {
        public PlayerContainer(Player player)
        {
            _player = player;

            PlayerName = _player.name;
            PlayerNumber = player.playerNumber;
        }

        private Player _player { get; }
        private WizardStatus _wizardStatus 
        { 
            get
            {
                GameObject wizard = _player?.wizard;
                if (wizard == null)
                    return null;
                else
                    return wizard.GetComponent<WizardStatus>();
            } 
        }

        [SROptions.DisplayName("Player Name")]
        public string PlayerName { get; }

        [SROptions.DisplayName("Player Number")]
        public int PlayerNumber { get; }

        [SROptions.DisplayName("Health (%)")]
        [NumberRange(0, 100), Increment(10)]
        public int HealthPercentage 
        { 
            get 
            {
                return (int)((_wizardStatus?.health ?? 0) / (_wizardStatus?.maxHealth ?? 1) * 100f);
            }
            set 
            {
                var wizardStatus = _wizardStatus;

                if (wizardStatus == null)
                    return;

                Debug.Log("Applying healing/damage");

                float newHealth = value / 100f * wizardStatus.maxHealth;

                if (wizardStatus.health > newHealth)
                    wizardStatus.ApplyDamage(wizardStatus.health - newHealth, 0, -4);
                else if (wizardStatus.health < newHealth)
                    wizardStatus.ApplyHealing(newHealth - wizardStatus.health, 0);

                Debug.Log("Done!");
            } 
        }

        public void Heal()
        {
            _wizardStatus.ApplyHealing(_wizardStatus.maxHealth - _wizardStatus.health, 0);
        }

        public void Kill()
        {
            _wizardStatus.ApplyDamage(_wizardStatus.health, 0, -4);
        }

        public int Primary { 
            get { return (int)_player.spell_library[SpellButton.Primary]; } 
            set { Globals.spell_manager.AddSpellToPlayer(SpellButton.Primary, (SpellName)value, PlayerNumber); } 
        }
        public int Movement { 
            get { return (int)_player.spell_library[SpellButton.Movement]; } 
            set { Globals.spell_manager.AddSpellToPlayer(SpellButton.Movement, (SpellName)value, PlayerNumber); }
        }
        public int Melee { 
            get { return (int)_player.spell_library[SpellButton.Melee]; } 
            set { Globals.spell_manager.AddSpellToPlayer(SpellButton.Melee, (SpellName)value, PlayerNumber); }
        }
        public int Secondary { 
            get { return (int)_player.spell_library[SpellButton.Secondary]; } 
            set { Globals.spell_manager.AddSpellToPlayer(SpellButton.Secondary, (SpellName)value, PlayerNumber); }
        }
        public int Defensive { 
            get { return (int)_player.spell_library[SpellButton.Defensive]; } 
            set { Globals.spell_manager.AddSpellToPlayer(SpellButton.Defensive, (SpellName)value, PlayerNumber); }
        }
        public int Utility { 
            get { return (int)_player.spell_library[SpellButton.Utility]; } 
            set { Globals.spell_manager.AddSpellToPlayer(SpellButton.Utility, (SpellName)value, PlayerNumber); }
        }
        public int Ultimate { 
            get { return (int)_player.spell_library[SpellButton.Ultimate]; } 
            set { Globals.spell_manager.AddSpellToPlayer(SpellButton.Ultimate, (SpellName)value, PlayerNumber); }
        }
    }
}
