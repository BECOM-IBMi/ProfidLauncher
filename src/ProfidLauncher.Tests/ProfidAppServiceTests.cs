﻿
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using ProfidLauncher.Extensions;
using ProfidLauncher.Models;
using ProfidLauncher.Services;

namespace ProfidLauncher.Tests;

public class ProfidAppServiceTests
{
    List<AppMode> _appModes = new();

    ProfidLauncherSettings _settings = new();

    public ProfidAppServiceTests()
    {
        _settings = new ProfidLauncherSettings
        {
            HttpPort = 8082,
            HttpsPort = 8085
        };

        setupAppModes();
    }

    [Theory]
    [InlineData("ATRIUMP", 0, false, "001-itsv-mprat2", "http://as400.becom.at:8082/profoundui/atrium?workstnid=mprat2&suffixid=1")]
    [InlineData("GENIEP", 0, false, "001-itsv-mprat2", "http://as400.becom.at:8082/profoundui/genie?skin=BECOM&workstnid=mprat2&suffixid=1")]
    [InlineData("ATRIUMD", 0, false, "001-itsv-mprat2", "http://as400test.becom.at:8082/profoundui/atrium?workstnid=mprat2&suffixid=1")]
    [InlineData("GENIED", 0, false, "001-itsv-mprat2", "http://as400test.becom.at:8082/profoundui/genie?skin=BECOM&workstnid=mprat2&suffixid=1")]
    [InlineData("ATRIUMP", 0, false, "002-VENB-AMAZA1", "http://as400.becom.at:8082/profoundui/atrium?workstnid=amaza1&suffixid=1")]
    [InlineData("ATRIUMP", 0, false, "HITJKATO1", "http://as400.becom.at:8082/profoundui/atrium?workstnid=jkato1&suffixid=1")]
    [InlineData("ATRIUMCN", 0, false, "032-GRPC-STAFF1", "http://032-itsv-as400.becom.at:8082/profoundui/atrium?workstnid=staff1&suffixid=1")]
    [InlineData("ATRIUMP", 8085, true, "001-itsv-mprat2", "https://as400.becom.at:8085/profoundui/auth/atrium?workstnid=mprat2&suffixid=1")]
    [InlineData("ATRIUMP", 1234, false, "001-itsv-mprat2", "http://as400.becom.at:1234/profoundui/atrium?workstnid=mprat2&suffixid=1")]
    public void Test_GetUrl_OK(string operationMode, int port, bool useSecure, string hostName, string resultUrl)
    {
        var config = ProfidAppExtensions.GetProfidAppConfiguration(new CommandLineOptions { OperationMode = operationMode, Port = port, UseSecure = useSecure }, hostName);

        var sut = new ProfidAppService(NullLogger<ProfidAppService>.Instance, _appModes, config, _settings);

        var url = sut.GetCurrentUrl();

        url.Should().Be(resultUrl);
    }

    private void setupAppModes()
    {
        _appModes = new List<AppMode> { new AppMode
        {
            OperationMode = "ATRIUMP",
            IconName = "favicon.ico",
            BaseUrl = "{0}://as400.becom.at:{1}/profoundui/{2}atrium?workstnid={3}&suffixid=1",
            ProgramShortcutName = "Profid Launcher",
            UseHttps = false
        },
        new AppMode
        {
            OperationMode = "GENIEP",
            IconName = "favicon_genie.ico",
            BaseUrl = "{0}://as400.becom.at:{1}/profoundui/{2}genie?skin=BECOM&workstnid={3}&suffixid=1",
            ProgramShortcutName = "Genie Production",
            UseHttps = false
        },
        new AppMode
        {
            OperationMode = "ATRIUMD",
            IconName = "favicon_dev.ico",
            BaseUrl = "{0}://as400test.becom.at:{1}/profoundui/{2}atrium?workstnid={3}&suffixid=1",
            ProgramShortcutName = "Profid Development",
            UseHttps = false
        },
        new AppMode
        {
            OperationMode = "GENIED",
            IconName = "favicon_genie_dev.ico",
            BaseUrl = "{0}://as400test.becom.at:{1}/profoundui/{2}genie?skin=BECOM&workstnid={3}&suffixid=1",
            ProgramShortcutName = "Genie Development",
            UseHttps = false
        },
        new AppMode
        {
            OperationMode = "ATRIUMCN",
            IconName = "favicon_cn.ico",
            BaseUrl = "{0}://032-itsv-as400.becom.at:{1}/profoundui/{2}atrium?workstnid={3}&suffixid=1",
            ProgramShortcutName = "Profid Development",
            UseHttps = false
        },
        new AppMode
        {
            OperationMode = "GENIECN",
            IconName = "favicon_genie_cn.ico",
            BaseUrl = "{0}://032-itsv-as400.becom.at:{1}/profoundui/{2}genie?skin=BECOM&workstnid={3}&suffixid=1",
            ProgramShortcutName = "Genie Development",
            UseHttps = false
        }};
    }
}
