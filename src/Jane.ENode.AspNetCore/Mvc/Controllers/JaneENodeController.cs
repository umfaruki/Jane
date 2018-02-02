using Castle.Core.Internal;
using ECommon.Extensions;
using ECommon.IO;
using ENode.Commanding;
using Jane.AspNetCore.Mvc.Controllers;
using System.Threading.Tasks;
using System;

namespace Jane.ENode.AspNetCore.Mvc.Controllers
{
    /// <summary>
    /// Base class for all MVC Controllers in Jane system.
    /// </summary>
    public abstract class JaneENodeController : JaneController
    {
        protected readonly ICommandService _commandService;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected JaneENodeController(
            ICommandService commandService
            )
        {
            _commandService = commandService;
        }

        #region Authorize Properties

        public string AccessToken
        {
            get
            {
                if (Request.Headers != null)
                {
                    if (!Request.Headers["authorization"].IsNullOrEmpty())
                    {
                        var authorization = Request.Headers["authorization"].ToString();
                        var items = authorization.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (items.Length == 2)
                        {
                            return items[1];
                        }
                    }
                }

                return Request.Query["access_token"];
            }
        }

        public string DeviceToken
        {
            get
            {
                return Request.Query["device_token"];
            }
        }

        public string UserAgent
        {
            get
            {
                if (Request.Headers != null)
                {
                    return Request.Headers["User-Agent"];
                }
                return null;
            }
        }

        public long UserId
        {
            get
            {
                return JaneSession.UserId.Value;
            }
        }

        #endregion Authorize Properties

        protected Task<AsyncTaskResult<CommandResult>> ExecuteCommandAsync(ICommand command, CommandReturnType commandReturnType = CommandReturnType.CommandExecuted, int millisecondsDelay = 5000)
        {
            return _commandService.ExecuteAsync(command, commandReturnType).TimeoutAfter(millisecondsDelay);
        }

        protected void ProcessExecuteCommandAsyncResult(AsyncTaskResult<CommandResult> result)
        {
            if (result.Status != AsyncTaskStatus.Success)
            {
                throw new UserFriendlyException("internal network meets problems!");
            }

            if (result.Data.Status != CommandStatus.Success)
            {
                throw new UserFriendlyException(result.Data.Result);
            }
        }

        protected void ProcessSendCommandAsyncResult(AsyncTaskResult result)
        {
            if (result.Status != AsyncTaskStatus.Success)
            {
                throw new UserFriendlyException("internal network meets problems!");
            }
        }

        protected Task<AsyncTaskResult> SendCommandAsync(ICommand command)
        {
            return _commandService.SendAsync(command);
        }
    }
}