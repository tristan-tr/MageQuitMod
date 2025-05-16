using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MageQuitMod.SRDebugWrapper
{
    public interface IOptionContainerWrapper
    {
        string CategoryName { get; }

        object OptionContainer { get; }
    }
}
