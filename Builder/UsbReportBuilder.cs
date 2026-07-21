using DualSenseSharp.Utils;

namespace DualSenseSharp.Builder;

internal sealed class UsbReportBuilder : ReportBuilder
{


    internal UsbReportBuilder(DataBuilder dataBuilder) : base(dataBuilder)
    {
    }

    internal override byte[] Build()
    {
        var report = new byte[48];
        var data = DataBuilder.BuildData();
        ByteArrayUtil.SetBytes(report, 1, data);
        report[0] = 0x02;
        return report;
    }
}