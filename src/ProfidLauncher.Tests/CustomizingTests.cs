using FluentAssertions;
using ProfidLauncher.Extensions;
using ProfidLauncher.Models;

namespace ProfidLauncher.Tests;

public class CustomizingTests
{
    [Theory]
    [InlineData("001-itsv-mprat2", true, "mprat2", Sites.HOCHSTRASS)]
    [InlineData("002-VENB-AMAZA1", true, "AMAZA1", Sites.KOERNYE)]
    [InlineData("HEWAUNGE1", false, "AUNGE1", Sites.UNKNOWN)]
    [InlineData("HITJKATO1", false, "JKATO1", Sites.UNKNOWN)]
    [InlineData("032-GRPC-STAFF1", true, "STAFF1", Sites.HEYUAN)]
    [InlineData("001-RDNB-MKRUT1", true, "MKRUT1", Sites.HOCHSTRASS)]
    public void TestWorkstationInfo(string hostname, bool isNewWorkstation, string workstationId, Sites site)
    {
        var info = ProfidAppExtensions.GetWorkstationInfo(hostname);

        info.isNewWorkstation.Should().Be(isNewWorkstation);
        info.workstationId.Should().Be(workstationId);
        info.site.Should().Be(site);
    }
}
