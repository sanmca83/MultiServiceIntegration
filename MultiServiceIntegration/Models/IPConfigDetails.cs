using System.Collections.Generic;

namespace MultiServiceIntegration.Models
{
    public class IPConfigDetails
    {
        public IPConfigDetails()
        {
            IpInfos = new List<IpInfo>();
        }
        public List<IpInfo> IpInfos { get; set; }
    }

    public class IpInfo
    {
        public string ServiceName { get; set; }
        public string StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Response { get; set; }

    }
}

