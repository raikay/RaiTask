﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using RaiTask.Infrastructure.Authentication;
using RaiTask.IServices;

namespace RaiTask.API.Attributes
{
    /// <summary>
    /// 启用权限
    /// </summary>
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 权限校验
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task PermissionAuthorization(AuthorizationFilterContext context)
        {
            //排除匿名访问
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(AllowAnonymousAttribute)))
                return;

            //登录验证
            var user = context.HttpContext.RequestServices.GetService<ICurrentUser>();
            if (string.IsNullOrWhiteSpace(user?.Id))
            {
                context.Result = new ChallengeResult();
                return;
            }
            //权限验证
            var httpMethod = context.HttpContext.Request.Method;
            var api = context.ActionDescriptor.AttributeRouteInfo.Template;
            var _userService = context.HttpContext.RequestServices.GetService<IUserService>();
            //获取用户权限
            var permissionList = await _userService.GetUserPermissionsAsync();
            var isValid = permissionList.Any(m => m != null &&!string.IsNullOrWhiteSpace(m.HttpMethod) && m.Path.EqualsIgnoreCase($"/{api}")&&m.HttpMethod.EqualsIgnoreCase(httpMethod));
            if (!isValid)
            {
                context.Result = new ForbidResult();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            await PermissionAuthorization(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await PermissionAuthorization(context);
        }
    }



}
