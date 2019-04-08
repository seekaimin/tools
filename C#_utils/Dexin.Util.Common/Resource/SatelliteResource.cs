using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Collections;

namespace Dexin.Util.Common.Resource
{
    /// <summary>
    /// 提供区域性的附属程序集资源信息
    /// </summary>
    public class SatelliteResource
    {
        private IList<DictionaryEntry> _ResourceList;
        /// <summary>
        /// 资源列表
        /// </summary>
        public IList<DictionaryEntry> ResourceList
        {
            get { return _ResourceList; }
            set { _ResourceList = value; }
        }

        /// <summary>
        /// 初始化资源程序集信息
        /// </summary>
        /// <param name="assembly">被关联的程序集信息</param>
        public SatelliteResource(Assembly assembly)
        {
            _ResourceList = new List<DictionaryEntry>();

            Assembly resourceAssembly = assembly.GetSatelliteAssembly(CultureInfo.CurrentUICulture);
            foreach (string resourceName in resourceAssembly.GetManifestResourceNames())
            {
                ResourceLocation resourceLocation = resourceAssembly.GetManifestResourceInfo(resourceName).ResourceLocation;

                // if this resource is in another assemlby, we will skip it
                if ((resourceLocation & ResourceLocation.ContainedInAnotherAssembly) != 0)
                {
                    continue;   // in resource assembly, we don't have resource that is contained in another assembly
                }

                Stream resourceStream = resourceAssembly.GetManifestResourceStream(resourceName);
                using (ResourceReader reader = new ResourceReader(resourceStream))
                {
                    EnumerateResources(reader, resourceName);
                }
            }
        }

        //--------------------------------
        // private function
        //--------------------------------
        /// <summary>
        /// Enumerate baml streams in a resources file
        /// </summary>        
        private void EnumerateResources(ResourceReader reader, string resourceName)
        {
            foreach (DictionaryEntry entry in reader)
            {
                _ResourceList.Add(entry);
            }
        }
    }
}
