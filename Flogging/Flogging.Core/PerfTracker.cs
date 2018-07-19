﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Flogging.Core
{
    public class PerfTracker
    {
        private readonly Stopwatch _stopwatch;
        private readonly FlogDetail _infoToLog;

        public PerfTracker(string name, string userId, string userName, string location, string product, string layer)
        {
            _stopwatch = Stopwatch.StartNew();
            _infoToLog = new FlogDetail
            {
                Message = name,
                UserId = userId,
                UserName = userName,
                Product = product,
                Layer = layer,
                Location = location,
                Hostname = Environment.MachineName
            };

            var beginTime = DateTime.Now;
            _infoToLog.AdditionalInfo = new Dictionary<string, object>()
            {
                { "Started", beginTime.ToString(CultureInfo.InstalledUICulture) }
            };
        }

        public PerfTracker(string name, string userId, string userName, string location, string product, string layer, Dictionary<string, object> perfParams)
            : this(name, userId, userName, location, product, layer)
        {
            foreach (var perfParam in perfParams)
            {
                _infoToLog.AdditionalInfo.Add($"input-{perfParam.Key}", perfParam.Value);
            }
        }

        public void Stop()
        {
            _stopwatch.Stop();
            _infoToLog.ElapsedMillisenconds = _stopwatch.ElapsedMilliseconds;
            Flogger.WritePerf(_infoToLog);
        }
    }
}