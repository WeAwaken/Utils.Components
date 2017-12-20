﻿using Awaken.Utils.Test;
using Awaken.Utils.Widgets;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiServiceExtensions
    {
        /// <summary>
        /// 添加服务注册机构
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiService(
            this IServiceCollection serviceCollection,
            Action<ApiRouteOptions> optionsAction = null)
        {
            serviceCollection.AddApiRegister(optionsAction);

            //Transient： 服务在每次被请求时都会被创建。这种生命周期比较适用于轻量级的无状态服务。

            //Scoped： 服务是每次Web HttpRequest请求被创建。

            //Singleton： 服务的生命周期在第一被请求时创建，在后续的每个请求都会使用同一个实例。如果你的应用需要单例服务，推荐的做法是交给服务容器来负责单例的创建和生命周期管理。

            //Instance： 直接添加实例到服务容器。如果这样做，该实例会被后续的所有请求所使用（这样就会创建一个scoped - Singleton实例）。Instance和Singleton的一个主要区别在于，Instance服务是由ConfigureServices创建，然而Singleton服务是lazy - loaded，在第一个被请求时才会被创建。

          

            return serviceCollection;

        }

        //public static IServiceCollection AddApiService(
        //    this IServiceCollection serviceCollection,
        //    ApiRouteOptions options)
        //{
        //    serviceCollection.AddApiRegister(o => {
        //        o.BaseAddress = options.BaseAddress;
        //        o.Authority = options.Authority;
        //        o.ClientId = options.ClientId;
        //        o.ClientScope = options.ClientScope;
        //        o.ClientSecret = options.ClientSecret;

        //    });

        //    //Transient： 服务在每次被请求时都会被创建。这种生命周期比较适用于轻量级的无状态服务。

        //    //Scoped： 服务是每次Web HttpRequest请求被创建。

        //    //Singleton： 服务的生命周期在第一被请求时创建，在后续的每个请求都会使用同一个实例。如果你的应用需要单例服务，推荐的做法是交给服务容器来负责单例的创建和生命周期管理。

        //    //Instance： 直接添加实例到服务容器。如果这样做，该实例会被后续的所有请求所使用（这样就会创建一个scoped - Singleton实例）。Instance和Singleton的一个主要区别在于，Instance服务是由ConfigureServices创建，然而Singleton服务是lazy - loaded，在第一个被请求时才会被创建。

        //    serviceCollection.AddScoped<SettingApi>();

        //    return serviceCollection;

        //}
    }
}
