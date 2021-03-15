using Delivery.Core.DomainObjects;
using Delivery.Core.Notifications;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Delivery.Api.Core.Controllers
{

    [ApiController]
    public abstract class MainController : Controller
    {
        private readonly INotifier _notifier;

        protected MainController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected ActionResult<T> CustomResponseOk<T>(T result)
        {
            if (OperationValid())
                return Ok(result);

            return BadRequestReturm();
        }

        protected ActionResult<T> CustomResponseCreate<T>(string url, T result)
        {
            if (OperationValid())
                return Created(url, result);

            return BadRequestReturm();
        }

        protected ActionResult<T> CustomResponseError<T>(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AddError(erro.ErrorMessage);
            }

            return CustomResponseOk(default(T));
        }

        protected ActionResult<T> CustomResponseError<T>(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                AddError(erro.ErrorMessage);
            }

            return CustomResponseOk(default(T));
        }

        protected ActionResult<T> CustomResponseError<T>(IEnumerable<string> roleValidationResult)
        {
            foreach (var erro in roleValidationResult)
            {
                AddError(erro);
            }

            return CustomResponseOk(default(T));
        }

        protected bool OperationValid()
        {
            return !_notifier.ExistNotification();
        }

        protected void AddError(string erro)
        {
            _notifier.Handle(new Notification(erro)); 
        }

        private BadRequestObjectResult BadRequestReturm() {
            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", _notifier.GetNotification().Select(x=>x.Message).ToArray() }
            }));
        }
    }
}
