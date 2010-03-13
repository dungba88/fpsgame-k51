using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    public interface IActionDispatcher
    {
        void AddActionListener(IActionListener al);

        void TriggerActionPerformed(IActionEvent evt);

        void TriggerMouseMove(IActionEvent evt);
    }
}
