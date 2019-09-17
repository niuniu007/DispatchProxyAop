using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TestWeb
{
    /// <summary>
    /// 加载所有代理
    /// </summary>
    public class LoadSrvice
    {

        public static  void AddDotNettyService(IServiceCollection  serviceCollection)
        {
            var referenceAssemblies = LoadAssembly();
            foreach (var moduleAssembly in referenceAssemblies)
            {
                    //解析所需要的模块，然后注册

            }
        }

        //加载所有程序集
        static  List<Assembly> LoadAssembly()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;


            DirectoryInfo directoryInfo = new DirectoryInfo(basePath);
             List<string>    assemblys= Directory.GetFiles(basePath, "*.dll").Select(Path.GetFullPath).ToList();
            var refAssemblies = new List<Assembly>();   //程序集集合

            foreach (var assemblyFileFullName in assemblys)
            {
                var assembly = Assembly.Load(assemblyFileFullName);
                refAssemblies.Add(assembly);
            }

            //遍历程序集中的类找到具有指定特性的类

            return refAssemblies;
        }
 
    }
}
