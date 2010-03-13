using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FPSGame.Core
{
    public interface IActionListener
    {
        void OnActionPerformed(IActionEvent evt);

        void OnMouseOver(IActionEvent evt);

        void OnMouseOut(IActionEvent evt);
    }
}
