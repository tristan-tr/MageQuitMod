using SRF.Components;
using System.Collections.Generic;
using System.ComponentModel;
using static MageQuitMod.SRDebugWrapper.ElementSelection;

namespace MageQuitMod.SRDebugWrapper
{
    public class ElementSelection : IOptionContainerWrapper
    {
        public static readonly ElementSelection Instance = new ElementSelection();

        public string CategoryName => "Element Selection";

        public ElementSelectionContainer OptionContainer => new ElementSelectionContainer();
        object IOptionContainerWrapper.OptionContainer => OptionContainer;

        public enum ElementSelectionStatus
        {
            Available,
            Always,
            Never
        }

        public Dictionary<Element, ElementSelectionStatus> GetElements()
        {
            Dictionary<Element, ElementSelectionStatus> elementStatusDictionary = new Dictionary<Element, ElementSelectionStatus>
            {
                { Element.Fire, OptionContainer.Fire },
                { Element.Water, OptionContainer.Water },
                { Element.Air, OptionContainer.Air },
                { Element.Earth, OptionContainer.Earth },
                { Element.Sand, OptionContainer.Sand },
                { Element.Nature, OptionContainer.Nature },
                { Element.Electric, OptionContainer.Electric },
                { Element.Steam, OptionContainer.Steam },
                { Element.Metal, OptionContainer.Metal },
                { Element.Ice, OptionContainer.Ice }
            };

            return elementStatusDictionary;
        }
    }

    public class ElementSelectionContainer
    {
        public ElementSelectionStatus Fire { get; set; }
        public ElementSelectionStatus Water { get; set; }
        public ElementSelectionStatus Air { get; set; }
        public ElementSelectionStatus Earth { get; set; }
        public ElementSelectionStatus Sand { get; set; }
        public ElementSelectionStatus Nature { get; set; }
        public ElementSelectionStatus Electric { get; set; }
        public ElementSelectionStatus Steam { get; set; }
        public ElementSelectionStatus Metal { get; set; }
        public ElementSelectionStatus Ice { get; set; }
    }
}