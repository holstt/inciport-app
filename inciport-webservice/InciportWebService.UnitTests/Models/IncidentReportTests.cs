using AutoFixture;
using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InciportWebService.UnitTests {
  // If ctor should be injected with specific value, see -> https://stackoverflow.com/questions/28350054/easy-way-to-specify-the-value-of-a-single-constructor-parameter

  public class IncidentReportTests {
    private IncidentReportEntity _report;
    private Fixture _fixture;

    public IncidentReportTests() {
      Random random = new Random();
      _fixture = new Fixture();
      _fixture.Register(() => new Coordinates(random.Next(-90, 91), random.Next(-180, 181)));
      _report = _fixture.Create<IncidentReportEntity>();
    }

    [Theory]
    [InlineData(ReportStatus.Recieved)]
    [InlineData(ReportStatus.InProgress)]
    [InlineData(ReportStatus.Archived)]
    public void ArchiveIfEligible_WithStatusNotArchivable_DoesNotArchive(ReportStatus withStatus) {
      // ARRANGE

      _report.Status = withStatus;
      ReportStatus expectedStatus = withStatus;
      DateTimeOffset currentTime = _fixture.Create<DateTimeOffset>();

      // ACT
      _report.ArchiveIfEligible(currentTime);

      // ASSERT
      Assert.Equal(expectedStatus, actual: _report.Status);
    }

    public static IEnumerable<object[]> Data_ArchiveIfEligible_WithStatusArchivableButNotOldEnough_DoesNotArchive =>
  new List<object[]>
     {
            new object[] { ReportStatus.Recieved, DateTimeOffset.UnixEpoch.AddDays(0) },
            new object[] { ReportStatus.Recieved, DateTimeOffset.UnixEpoch.AddDays(29)},
            new object[] { ReportStatus.Recieved, DateTimeOffset.UnixEpoch.AddDays(30)}
    };

    [Theory]
    [MemberData(nameof(Data_ArchiveIfEligible_WithStatusArchivableButNotOldEnough_DoesNotArchive))]
    public void ArchiveIfEligible_WithStatusArchivableButNotOldEnough_DoesNotArchive(ReportStatus withStatus, DateTimeOffset withCurrentDate) {
      // ARRANGE
      DateTimeOffset lastModifiedDate = new DateTimeOffset(DateTimeOffset.UnixEpoch.Date, TimeSpan.Zero);
      _report.Status = withStatus;
      _report.TimestampLastModifiedUtc = lastModifiedDate;
      ReportStatus expectedStatus = withStatus;

      // ACT
      _report.ArchiveIfEligible(withCurrentDate);

      // ASSERT
      Assert.Equal(expectedStatus, actual: _report.Status);
    }

    public static IEnumerable<object[]> Data_ArchiveIfEligible_WithStatusArchivableAndOldEnough_DoesArchive =>
      new List<object[]>
         {
            new object[] {  DateTimeOffset.UnixEpoch.AddDays(31) },
            new object[] { DateTimeOffset.UnixEpoch.AddDays(200)}
        };

    [Theory]
    [MemberData(nameof(Data_ArchiveIfEligible_WithStatusArchivableAndOldEnough_DoesArchive))]
    public void ArchiveIfEligible_WithStatusArchivableAndOldEnough_DoesArchive(DateTimeOffset withCurrentDate) {
      // ARRANGE
      ReportStatus withStatus = ReportStatus.Completed;
      DateTimeOffset lastModifiedDate = new DateTimeOffset(DateTimeOffset.UnixEpoch.Date, TimeSpan.Zero);
      _report.Status = withStatus;
      _report.TimestampLastModifiedUtc = lastModifiedDate;

      // ACT
      _report.ArchiveIfEligible(withCurrentDate);

      // ASSERT
      Assert.Equal(expected: ReportStatus.Archived, actual: _report.Status);
    }
  }
}