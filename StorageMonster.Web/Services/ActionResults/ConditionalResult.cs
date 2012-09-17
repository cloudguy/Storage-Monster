using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.ActionResults
{
    public class ConditionalResult : ActionResult
    {
        private Func<ActionResult> _ifAjaxAction;
        private Func<ActionResult> _ifNotAjaxAction;

        public ConditionalResult DoIfAjax(Func<ActionResult> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            _ifAjaxAction = action;
            return this;
        }

        public ConditionalResult DoIfNotAjax(Func<ActionResult> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            _ifNotAjaxAction = action;
            return this;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                if (_ifAjaxAction == null)
                    throw new InvalidOperationException("Ajax action not set");

                _ifAjaxAction().ExecuteResult(context);
                return;
            }

            if (_ifNotAjaxAction == null)
                throw new InvalidOperationException("Regular action not set");
            _ifNotAjaxAction().ExecuteResult(context);
        }
    }
}