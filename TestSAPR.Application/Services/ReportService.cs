using System;
using System.Collections.Generic;
using System.Text;
using TestSAPR.Application.Interfaces;

namespace TestSAPR.Application.Services
{
    public class ReportService : IReportService
    {
        public Task<byte[]> MakeReport(Guid part_id)
        {
            throw new NotImplementedException();
        }
    }
}
