using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class CheckVisioConfiguration : ICheckVisioConfiguration
    {
        public IList<string> Check()
        {
            List<string> configErrors = new();
#if (DEBUG)
            return configErrors;
#else
            if (Visio2003Installed() || Visio2007Installed())
            {
                configErrors.Add(InstallVisioMessage);
                return configErrors;
            }

            bool bVisio2010Installed = Visio2010Installed();
            bool bVisio2013Installed = Visio2013Installed();
            bool bVisio2016Installed = Visio2016Installed();
            bool bVisio2019Installed = Visio2019Installed();
            if (!((bVisio2019Installed && !bVisio2016Installed && !bVisio2010Installed && !bVisio2013Installed)
                || (!bVisio2019Installed && bVisio2016Installed && !bVisio2010Installed && !bVisio2013Installed)
                || (!bVisio2019Installed && !bVisio2016Installed && bVisio2010Installed && !bVisio2013Installed)
                || (!bVisio2019Installed && !bVisio2016Installed && !bVisio2010Installed && bVisio2013Installed)))
                configErrors.Add(InstallVisioMessage);

            return configErrors;
#endif

        }

#if (!DEBUG)
        /// <summary>
        /// Returns true if Visio 2016 is installed otherwise false
        /// </summary>
        /// <returns></returns>
        private static bool Visio2019Installed()
            => Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegistryKeys.Visio2019Info) != null;

        /// <summary>
        /// Returns true if Visio 2016 is installed otherwise false
        /// </summary>
        /// <returns></returns>
        private static bool Visio2016Installed()
            => Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegistryKeys.Visio2016Info) != null;

        /// <summary>
        /// returns true if Visio 2013 is installed otherwise false
        /// </summary>
        /// <returns></returns>
        private static bool Visio2013Installed()
        {
            Microsoft.Win32.RegistryKey? visio2013Infokey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegistryKeys.Visio2013Info);

            if (visio2013Infokey == null)
                return false;

            return visio2013Infokey.GetValue(RegistryValueNames.CurrentlyRegisteredVersion) is string;
        }

        /// <summary>
        /// returns true if Visio 2010 is installed otherwise false
        /// </summary>
        /// <returns></returns>
        private static bool Visio2010Installed()
        {
            Microsoft.Win32.RegistryKey? visio2010Infokey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegistryKeys.Visio2010Info);

            if (visio2010Infokey == null)
                return false;

            return visio2010Infokey.GetValue(RegistryValueNames.CurrentlyRegisteredVersion) is string;
        }

        /// <summary>
        /// returns true if Visio 2007 is installed otherwise false
        /// </summary>
        /// <returns></returns>
        private static bool Visio2007Installed()
        {
            Microsoft.Win32.RegistryKey? visio2007Infokey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegistryKeys.Visio2007Info);

            if (visio2007Infokey == null)
                return false;

            return visio2007Infokey.GetValue(RegistryValueNames.CurrentlyRegisteredVersion) is string;
        }

        /// <summary>
        /// Returns true if Visio 2003 is installed otherwise false
        /// </summary>
        /// <returns></returns>
        private static bool Visio2003Installed()
        {
            Microsoft.Win32.RegistryKey? visio2003Infokey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegistryKeys.Visio2003Info);

            if (visio2003Infokey == null)
                return false;

            return visio2003Infokey.GetValue(RegistryValueNames.CurrentlyRegisteredVersion) is string;
        } 


        private static string InstallVisioMessage

#if VERSION_32
            => Strings.installVisio2010or2013or2016or2019x86;
#else
            => Strings.installVisio2010or2013or2016or2019x64;
#endif

#endif
    }
}
