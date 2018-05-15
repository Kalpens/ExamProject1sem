using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Converter;
using Quartz;

namespace WebApi.Quartz
{
    public class IDGJob : IJob
    {

        Task IJob.Execute(IJobExecutionContext context)
        {
            var conv = new Converter.Converter();
            conv.Convert();
            return null;
        }
    }
}
