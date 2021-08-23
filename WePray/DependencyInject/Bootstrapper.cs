using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WePray.Repository._class;
using WePray.Services.Connection;
using WePray.Services.WordPressServices;
using WePray.ViewModels;
using Xamarin.Forms;

namespace WePray.DependencyInject
{
    /// <summary>
    /// This class initializes Autofac we will call it at the startup of each individual app Android and ios (it is implemented by each platform)
    /// </summary>
    public abstract class Bootstrapper
    {
        /// <summary>
        /// This gotten from Autofac it builds the container for us. it is built after configuration is done i'm building it in FinishInitialization() method
        /// </summary>
        protected ContainerBuilder ContainerBuilder
        {
            get; private set;
        }


        #region ctor

        public Bootstrapper()
        {
            Initialize();
            FinishInitialization();
        }

        #endregion ctor


        #region methods

        /// <summary>
        ///  We can override this in the platform specific projects
        /// </summary>
        protected virtual void Initialize()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            ContainerBuilder = new ContainerBuilder();
            foreach (var type in currentAssembly.DefinedTypes.Where(e => e.IsSubclassOf(typeof(Page)) || e.IsSubclassOf(typeof(BaseViewModel))))
            {
                //We register types which are pages and BaseViewModels (gives new instance to all types here when type is called) 
                ContainerBuilder.RegisterType(type.AsType());
            }

            //Registers types as singleton
            RegisterAsSingleton();

        }//end Initialize()


        /// <summary>
        /// Gets same instance of all types registered here through out the life time of the app
        /// </summary>
        private void RegisterAsSingleton()
        {
            ContainerBuilder.RegisterType<ConvertModels>().SingleInstance().As<IConvertModel>();
            ContainerBuilder.RegisterType<NetworkConnection>().SingleInstance().As<IConnection>();
            ContainerBuilder.RegisterType<WPServices>().SingleInstance().As<IWPServices>();



            //wow made a mistake with my naming Folder should be called Repositories
            ContainerBuilder.RegisterType<Repository.Repository>().SingleInstance().As<Repository.IRepository>();
        }


        private void FinishInitialization()
        {
           var container = ContainerBuilder.Build();
           Resolver.Initialize(container);
        }

        #endregion methods

    }
}

