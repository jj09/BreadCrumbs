using System;
using System.Collections.Generic;
using System.Text;
using SimpleInjector;
using BreadCrumbs.Shared.ViewModels;

namespace BreadCrumbs.Shared.Helpers
{
    public static class ContainerHelper
    {
        public static Container Container;

        public static void Init()
        {
            // 1. Create a new Simple Injector container
            Container = new Container();

            // 2. Configure the container (register)
            //container.Register<IOrderRepository, SqlOrderRepository>();
            //container.Register<ILogger, FileLogger>(Lifestyle.Singleton);
            Container.Register<MainViewModel>();

            // 3. Verify your configuration
            Container.Verify();
        }
    }
}
