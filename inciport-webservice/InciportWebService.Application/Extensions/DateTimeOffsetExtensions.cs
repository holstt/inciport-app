using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InciportWebService.Application {

  public static class DateTimeOffsetExtensions {

    public static TimeSpan GetTimeUntilTimeOfDay(this DateTimeOffset instance, TimeSpan targetTimeOfDay) {
      if (targetTimeOfDay < TimeSpan.Zero) {
        throw new ArgumentException($"{nameof(targetTimeOfDay)} cannot be negative", nameof(targetTimeOfDay));
      }
      if (targetTimeOfDay > TimeSpan.FromHours(24)) {
        throw new ArgumentException($"{nameof(targetTimeOfDay)} exceed 24 hours", nameof(targetTimeOfDay));
      }

      TimeSpan currentTimeOfDay = instance.TimeOfDay;
      // Assume target time is today
      DateTimeOffset targetTime = new DateTimeOffset(instance.Date, instance.Offset).Add(targetTimeOfDay);
      if (targetTimeOfDay < currentTimeOfDay) {
        // Target will be tomorrow
        targetTime = targetTime.AddDays(1);
      }

      return targetTime - instance;
    }
  }
}