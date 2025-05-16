using Newtonsoft.Json;
using SRDebugger.Internal;
using SRDebugger.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SRDebugger;
using UnityEngine;

namespace MageQuitMod.SRDebugWrapper
{
    public partial class SRDebugWrapper
    {
        //public static List<PlayerOptions> playerOptionsList = new List<PlayerOptions>();

        public const string DEFAULT_CATEGORY = "Default";


        /// <summary>
        /// Gets the initialized option definitions using reflection
        /// </summary>
        public static ICollection<OptionDefinition> GetOptionDefinitions()
        {
            return SRDebug.Instance.GetFieldValue<IOptionsService>("_optionsService").Options;
        }

        /// <summary>
        /// Changes the category of the option using reflection
        /// </summary>
        public static void ChangeCategory(OptionDefinition option, string newCategory)
        {
            option.SetFieldValue("<Category>k__BackingField", newCategory); // "<Category>k__BackingField" due to compiler wrapping
        }

        // todo: better name
        public static void AddOptionWrapper<T>(T optionWrapper) where T : IOptionContainerWrapper
        {
            SRDebug.Instance.AddOptionContainer(optionWrapper.OptionContainer);

            ICollection<OptionDefinition> optionDefinitions = GetOptionDefinitions();

            // Rename from default category to the specified category
            foreach (OptionDefinition option in optionDefinitions.Where(option => option.Category == DEFAULT_CATEGORY))
            {
                Debug.Log("Changing category of option: " + option.Name);
                ChangeCategory(option, optionWrapper.CategoryName);
                Debug.Log("Done!");
            }
        }

        public SRDebugWrapper()
        {
            Debug.Log("Adding container to options");
            AddOptionWrapper(ElementSelection.Instance);
            AddOptionWrapper(Round.Instance);
            Debug.Log("Done!");
        }
    }
}
