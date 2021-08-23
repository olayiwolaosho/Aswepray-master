using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace WePray.DependencyInject
{
    /// <summary>
    /// This is the class that creates instances of classes and manages the lifetime (singleton or we create new object at every call) 
    /// </summary>
    public static class Resolver
    {
        private static IContainer container;


        /// <summary>
        /// Instance of object that implements Icontainer is pass as a parameter 
        /// </summary>
        /// <param name="container">instance of object that implements Icontainer</param>
        public static void Initialize(IContainer container)
        {
            Resolver.container = container;
        }


        /// <summary>
        /// Return instance of a particular type T
        /// </summary>
        /// <typeparam name="T">An instance of this type T will be returned</typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
           return container.Resolve<T>();
        }
    }
}
