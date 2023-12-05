using InciportWebService.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InciportWebService.UnitTests {

  public class DateTimeOffsetExentionsTests {

    [Fact]
    public void GetTimeUntilTimeOfDay_TimeIsSameDay_ReturnsCorrectResult() {
      // ARRANGE
      TimeSpan targetTimeOfDay = TimeSpan.FromHours(2);
      DateTimeOffset currentTime = new DateTimeOffset(year: 2020, month: 1, day: 1, hour: 1, 0, 0, TimeSpan.Zero); ;
      TimeSpan expectedTimeUntil = TimeSpan.FromHours(1);

      // ACT
      TimeSpan actualTimeUntil = currentTime.GetTimeUntilTimeOfDay(targetTimeOfDay);

      // ASSERT
      Assert.Equal(expectedTimeUntil, actualTimeUntil);
    }

    [Fact]
    public void GetTimeUntilTimeOfDay_TimeIsNextDay_ReturnsCorrectResult() {
      // ARRANGE
      TimeSpan targetTimeOfDay = TimeSpan.FromHours(2);
      DateTimeOffset currentTime = new DateTimeOffset(year: 2020, month: 1, day: 1, hour: 16, 0, 0, TimeSpan.Zero); ;
      TimeSpan expectedTimeUntil = TimeSpan.FromHours(10);

      // ACT
      TimeSpan actualTimeUntil = currentTime.GetTimeUntilTimeOfDay(targetTimeOfDay);

      // ASSERT
      Assert.Equal(expectedTimeUntil, actualTimeUntil);
    }

    [Fact]
    public void GetTimeUntilTimeOfDay_TimeIsNow_ReturnsCorrectResult() {
      // ARRANGE
      TimeSpan targetTimeOfDay = TimeSpan.FromHours(2);
      DateTimeOffset currentTime = new DateTimeOffset(year: 2020, month: 1, day: 1, hour: 2, 0, 0, TimeSpan.Zero); ;
      TimeSpan expectedTimeUntil = TimeSpan.Zero;

      // ACT
      TimeSpan actualTimeUntil = currentTime.GetTimeUntilTimeOfDay(targetTimeOfDay);

      // ASSERT
      Assert.Equal(expectedTimeUntil, actualTimeUntil);
    }

    [Fact]
    public void GetTimeUntilTimeOfDay_NegativeTarget_ThrowsException() {
      // ARRANGE
      TimeSpan targetTimeOfDay = TimeSpan.FromHours(-2);
      DateTimeOffset currentTime = new DateTimeOffset(year: 2020, month: 1, day: 1, hour: 2, 0, 0, TimeSpan.Zero); ;
      TimeSpan expectedTimeUntil = TimeSpan.Zero;

      // ACT & ASSERT
      Assert.Throws<ArgumentException>(() => currentTime.GetTimeUntilTimeOfDay(targetTimeOfDay));
    }

    [Fact]
    public void GetTimeUntilTimeOfDay_TargetExceeds24Hours_ThrowsException() {
      // ARRANGE
      TimeSpan targetTimeOfDay = TimeSpan.FromHours(25);
      DateTimeOffset currentTime = new DateTimeOffset(year: 2020, month: 1, day: 1, hour: 2, 0, 0, TimeSpan.Zero); ;
      TimeSpan expectedTimeUntil = TimeSpan.Zero;

      // ACT & ASSERT
      Assert.Throws<ArgumentException>(() => currentTime.GetTimeUntilTimeOfDay(targetTimeOfDay));
    }
  }
}