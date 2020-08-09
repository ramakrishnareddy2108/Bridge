namespace BatteryAudit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using BatteryAudit.Models;
    using BatteryAudit.Repository;
    using BatteryAudit.Service;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class TabletServiceTests
    {
        private readonly ITabletService _tabletService;
        private readonly Mock<ITabletRepository> _tabletRepository;
        private readonly Fixture _fixture;

        public TabletServiceTests()
        {
            _fixture = new Fixture();
            _tabletRepository = new Mock<ITabletRepository>();
            _tabletService = new TabletService(_tabletRepository.Object);
        }

        #region  Method: GetTabletData

        [Fact]
        public void GetTabletBatteryUsage_WhenTabletReadingsAreNotAvailable()
        {
            // Arrange
            var serialNumber = _fixture.Create<string>();
            _tabletRepository.Setup(x => x.GetTabletData(It.IsAny<string>())).Returns((List<TabletReading>)null);

            // Act
            var result = _tabletService.GetTabletBatteryUsage(serialNumber);

            // Assert
            result.Should().NotBeNull();
            result.SerialNumber.Should().BeEquivalentTo(serialNumber);
            result.Average.Should().Contain("Unknown");
        }

        [Fact]
        public void GetTabletBatteryUsage_WhenTabletReadingsAreAvailable()
        {
            // Arrange
            var serialNumber = _fixture.Create<string>();
            var tabletReadings = new List<TabletReading>()
            {
                new TabletReading() { AcademyId = 166, BatteryLevel = 0.40m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2020, 10, 10, 8, 00, 23) },
                new TabletReading() { AcademyId = 158, BatteryLevel = 0.20m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2020, 10, 11, 4, 00, 36) }
            };
            _tabletRepository.Setup(x => x.GetTabletData(It.IsAny<string>())).Returns(tabletReadings);

            // Act
            var result = _tabletService.GetTabletBatteryUsage(serialNumber);

            // Assert
            result.Should().NotBeNull();
            result.SerialNumber.Should().BeEquivalentTo(serialNumber);
            result.Average.Should().Be("24.00");
        }

        #endregion

        #region  Method: GetAcadamyTabletsBatteryUsage

        [Fact]
        public void GetAcadamyTabletsBatteryUsage_WhenThereAreNoTabletReading()
        {
            // Arrange
            var acadamyId = _fixture.Create<int>();
            _tabletRepository.Setup(x => x.GetTabletDataForAcadamy(It.IsAny<int>())).Returns((List<TabletReading>)null);

            // Act
            var results = _tabletService.GetAcadamyTabletsBatteryUsage(acadamyId);

            // Assert
            results.Should().BeNull();
        }

        [Fact]
        public void GetAcadamyTabletsBatteryUsage_WhenReadingsHasContinuousSpikes()
        {
            // Arrange
            var acadamyId = _fixture.Create<int>();
            var serialNumber = _fixture.Create<string>();
            var tabletReadings = new List<TabletReading>()
            {
                new TabletReading() { AcademyId = 166, BatteryLevel = 0.50m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 9, 00, 00) },
                new TabletReading() { AcademyId = 158, BatteryLevel = 0.60m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 10, 00, 00) },
                new TabletReading() { AcademyId = 166, BatteryLevel = 0.70m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 11, 00, 00) },
                new TabletReading() { AcademyId = 158, BatteryLevel = 0.90m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 12, 00, 00) }
            };
            _tabletRepository.Setup(x => x.GetTabletDataForAcadamy(It.IsAny<int>())).Returns(tabletReadings);

            // Act
            var results = _tabletService.GetAcadamyTabletsBatteryUsage(acadamyId);

            // Assert
            results.Should().NotBeNull();
            results.Count().Should().Be(1);
            results.First().SerialNumber.Should().BeEquivalentTo(serialNumber);
            results.First().Average.Should().Be("0");
        }

        [Fact]
        public void GetAcadamyTabletsBatteryUsage_WhenThereAreSpikesInReadings()
        {
            // Arrange
            var acadamyId = _fixture.Create<int>();
            var serialNumber = _fixture.Create<string>();

            var tabletReadings = new List<TabletReading>()
            {
                new TabletReading() { AcademyId = 166, BatteryLevel = 0.50m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 9, 00, 00) },
                new TabletReading() { AcademyId = 158, BatteryLevel = 0.60m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 10, 00, 00) },
                new TabletReading() { AcademyId = 166, BatteryLevel = 0.70m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 11, 00, 00) },
                new TabletReading() { AcademyId = 158, BatteryLevel = 0.30m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 12, 00, 00) }
            };
            _tabletRepository.Setup(x => x.GetTabletDataForAcadamy(It.IsAny<int>())).Returns(tabletReadings);

            // Act
            var results = _tabletService.GetAcadamyTabletsBatteryUsage(acadamyId);

            // Assert
            results.Should().NotBeNull();
            results.Count().Should().Be(1);
            results.First().SerialNumber.Should().BeEquivalentTo(serialNumber);
            results.First().Average.Should().Be("320.00");
        }

        [Fact]
        public void GetAcadamyTabletsBatteryUsage_WhenThereIsSpikeOnLastReading()
        {
            // Arrange
            var acadamyId = _fixture.Create<int>();
            var serialNumber = _fixture.Create<string>();

            var tabletReadings = new List<TabletReading>()
            {
                new TabletReading() { AcademyId = 166, BatteryLevel = 1, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 9, 00, 00) },
                new TabletReading() { AcademyId = 158, BatteryLevel = 0.90m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 10, 00, 00) },
                new TabletReading() { AcademyId = 80, BatteryLevel = 0.80m, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 11, 00, 00) },
                new TabletReading() { AcademyId = 158, BatteryLevel = 1, EmployeeId = _fixture.Create<string>(), SerialNumber = serialNumber, Timestamp = new DateTime(2019, 05, 17, 12, 00, 00) }
            };
            _tabletRepository.Setup(x => x.GetTabletDataForAcadamy(It.IsAny<int>())).Returns(tabletReadings);

            // Act
            var results = _tabletService.GetAcadamyTabletsBatteryUsage(acadamyId);

            // Assert
            results.Should().NotBeNull();
            results.Count().Should().Be(1);
            results.First().SerialNumber.Should().BeEquivalentTo(serialNumber);
            results.First().Average.Should().Be("240.00");
        }

        #endregion
    }
}
