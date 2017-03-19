using System.Net;

namespace ViridiX.Linguist.Network
{
    /// <summary>
    /// TODO: description
    /// </summary>
    public class XboxConnectionInformation
    {
        /// <summary>
        /// TODO: description
        /// </summary>
        public IPAddress Ip { get; }

        /// <summary>
        /// TODO: description
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="name"></param>
        public XboxConnectionInformation(IPAddress ip, string name)
        {
            Name = name;
            Ip = ip;
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} ({Ip})";
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Ip.GetHashCode() ^ Name.GetHashCode();
        }

        /// <summary>
        /// TODO: description
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            XboxConnectionInformation info = obj as XboxConnectionInformation;
            return info != null && (Name.Equals(info.Name) && Ip.Equals(info.Ip));
        }
    }
}
