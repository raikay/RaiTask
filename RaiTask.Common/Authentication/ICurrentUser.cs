﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RaiTask.Infrastructure.Authentication
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 昵称
        /// </summary>
        string NickName { get; }
    }
}
