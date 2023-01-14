using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryGoodNewsPortal.Core.SerilogSinks
{
    public class CustomSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        //private readonly IMailService _mailService;

        public CustomSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level == LogEventLevel.Fatal)
            {
                var message = logEvent.RenderMessage(_formatProvider);
                //_mailService.Send(message);
            }
        }
    }
}
