using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace MageQuitMod.SRDebugWrapper
{
    // TODO: wrap logic for healing/damaging to one function
    public class Round : IOptionContainerWrapper
    {
        public static readonly Round Instance = new Round();

        public string CategoryName => "Round";

        public RoundContainer OptionContainer => new RoundContainer();
        object IOptionContainerWrapper.OptionContainer => OptionContainer;

        public List<PlayerOptions> PlayerOptionsList = new List<PlayerOptions>();
    }

    public class RoundContainer
    {
        [SROptions.DisplayName("Sudden Death")]
        public void SuddenDeath()
        {
            foreach (Player player in PlayerManager.players.Values.Where(i => i.wizard != null))
            {
                var wizardStatus = player.wizard.GetComponent<WizardStatus>();

                // Make sure the wizard is not dead
                if (wizardStatus == null || wizardStatus.isDead == true)
                    continue;

                // Make sure the wizard is damaged to 0.01hp
                wizardStatus.ApplyDamage(wizardStatus.health - 0.01f, 0, -4);
            }
        }

        [SROptions.DisplayName("Heal All")]
        public void HealAll()
        {
            foreach (var player in PlayerManager.players.Values.Where(i => i.wizard != null))
            {
                var wizardStatus = player.wizard.GetComponent<WizardStatus>();

                // Make sure the wizard is not dead
                if (wizardStatus == null || wizardStatus.isDead == true)
                    continue;

                // Make sure the wizard is healed to full
                wizardStatus?.ApplyHealing(wizardStatus.maxHealth - wizardStatus.health, 0);
            }
        }

        [SROptions.DisplayName("Update Players")]
        public void UpdatePlayers()
        {
            // Remove all player options containers
            foreach (PlayerOptions playerOptions in Round.Instance.PlayerOptionsList)
            {
                Debug.Log("Removing playerContainer: " + playerOptions.OptionContainer.PlayerName);
                SRDebug.Instance.RemoveOptionContainer(playerOptions.OptionContainer);
            }

            Round.Instance.PlayerOptionsList.Clear();

            // Add a player options container for each player
            foreach (Player player in PlayerManager.players.Values)
            {
                // Add player container
                Debug.Log("Adding playerContainer: " + player.name);
                PlayerOptions optionsContainer = new PlayerOptions(player);
                optionsContainer.CategoryName = player.name;

                // Keep track of our player containers to refresh the list later
                Round.Instance.PlayerOptionsList.Add(optionsContainer);

                SRDebugWrapper.AddOptionWrapper(optionsContainer);
            }
        }
    }
}
