using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    public class ActionListener
    {
        public delegate void ActionPerformedMethod(IActionEvent evt);

        public delegate void MouseOverMethod(IActionEvent evt);
        
        public delegate void MouseOutMethod(IActionEvent evt);

        private ActionPerformedMethod apMethod;

        private MouseOverMethod movMethod;

        private MouseOutMethod mouMethod;

        public ActionPerformedMethod GetActionPerformedMethod()
        {
            return apMethod;
        }

        public MouseOverMethod GetMouseOverMethod()
        {
            return movMethod;
        }

        public MouseOutMethod GetMouseOutMethod()
        {
            return mouMethod;
        }

        public void SetActionPerformedMethod(ActionPerformedMethod method)
        {
            this.apMethod = method;
        }

        public void SetMouseOverMethod(MouseOverMethod method)
        {
            this.movMethod = method;
        }

        public void SetMouseOutMethod(MouseOutMethod method)
        {
            this.mouMethod = method;
        }
    }
}
