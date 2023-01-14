using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.SerilogSinks;

namespace FirstMvcApp.Core.SerilogSinks
{
    public static class CustomSinkExtension
    {
        public static LoggerConfiguration CustomSink(this LoggerSinkConfiguration loggerConfiguration, IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new CustomSink(formatProvider));
        }
    }
}
