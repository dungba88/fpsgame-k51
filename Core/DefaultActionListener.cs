using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    class DefaultActionListener : ActionListener
    {
        public DefaultActionListener()
        {
            SetActionPerformedMethod(new ActionPerformedMethod(OnActionPerformed));
            SetMouseOverMethod(new MouseOverMethod(OnMouseOver));
            SetMouseOutMethod(new MouseOutMethod(OnMouseOut));
        }

        void OnActionPerformed(IActionEvent evt)
        {
        }

        void OnMouseOver(IActionEvent evt)
        {
            evt.GetSource().SwitchTextureOff();
        }

        void OnMouseOut(IActionEvent evt)
        {
            evt.GetSource().SwitchTextureOn();
        }
    }
}
