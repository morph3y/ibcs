using System;
using System.Web;
using Framework;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Web;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Web
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();
        private static IKernel _kernel;

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        public static IKernel GetKernel()
        {
            return _kernel;
        }

        private static IKernel CreateKernel()
        {
            if (_kernel == null)
            {
                _kernel = new StandardKernel();
            }

            try
            {
                _kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                _kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices();

                _kernel.Settings.AllowNullInjection = true;
                return _kernel;
            }
            catch
            {
                _kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices()
        {
            _kernel.Load(new FrameworkInjectionModule(_kernel));
        }
    }
}
