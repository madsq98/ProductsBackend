﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProductsBackend.Security;
using ProductsBackend.Security.Model;

namespace ProductsBackend.CoreWebAPI.PolicyHandlers
{
    public class CanReadProductsHandler : AuthorizationHandler<CanReadProductsHandler>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanReadProductsHandler handler)
        {
            var defaultContext = context.Resource as DefaultHttpContext;
            if (defaultContext != null)
            {
                var user = defaultContext.Items["LoginUser"] as LoginUser;
                if (user != null)
                {
                    var authService = defaultContext.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                    var permissions = authService.GetPermissions(user.Id);
                    if (permissions.Exists(p => p.Name.Equals("CanReadProducts")))
                    {
                        context.Succeed(handler);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            else
            {
                context.Fail();
                
            }

            return Task.CompletedTask;
        }
    }
}