using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ViridiX.Linguist.Process
{
    /// <summary>
    /// TODO: description
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class XboxModule
    {
        /// <summary>
        /// Name of the module that was loaded.
        /// </summary>
        public string Name;

        /// <summary>
        /// Address that the module was loaded to.
        /// </summary>
        public long BaseAddress;

        /// <summary>
        /// Size of the module.
        /// </summary>
        public int Size;

        /// <summary>
        /// Time stamp of the module.
        /// </summary>
        public DateTime TimeStamp;

        /// <summary>
        /// Checksum of the module.
        /// </summary>
        public long Checksum;

        /// <summary>
        /// Sections contained within the module.
        /// </summary>
        public List<XboxModuleSection> Sections;

        /// <summary>
        /// Indicates whether or not the module uses TLS.
        /// </summary>
        public bool HasTls;

        /// <summary>
        /// Indicates whether or not the module is an Xbox executable.
        /// </summary>
        public bool IsXbe;

        /// <summary>
        /// Gets an Xbox module section by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XboxModuleSection GetSection(string name)
        {
            return Sections.FirstOrDefault(section => section.Name.Equals(name));
        }
    }
}
