using System;
using System.Collections.Generic;
using System.Text;

namespace TestSAPR.Application.Interfaces
{
    public interface IReportService
    {
        public Task<byte[]> MakeReport(Guid part_id);

    }
}
