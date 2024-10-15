using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace P7CreateRestApi.Controllers.ControllersExtension
{
    public static class IdentityResultExtension
    {
        public static void ToModelResult(this IdentityResult identityResult, ModelStateDictionary modelState)
        {
            foreach (var error in identityResult.Errors)
            {
                var key = error.Description.Substring(0, error.Description.IndexOf(" "));
                modelState.AddModelError(key, error.Description);
            }
        }
    }
}
