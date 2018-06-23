using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VmstatsGUI
{
    public interface ITypedHubClient
    {
        Task ReturnResultToClient(string connectionId, string displayArea, string[] xData, float[] yData);
    }
}