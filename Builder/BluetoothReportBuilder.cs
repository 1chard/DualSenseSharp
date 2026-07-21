using DualSenseSharp.Utils;

namespace DualSenseSharp.Builder;


internal sealed class BluetoothReportBuilder : ReportBuilder
{
    private byte _sequence = 0;

    internal BluetoothReportBuilder(DataBuilder dataBuilder) : base(dataBuilder)
    {
    }

    internal override byte[] Build()
    {
        var prebuffer = new byte[77];
        prebuffer[0] = (byte) (((_sequence++) << 4) & 0xff);
        prebuffer[1] = 0x10;
        var data = DataBuilder.BuildData();
        ByteArrayUtil.SetBytes(prebuffer, 2, data);
        BluetoothCrc.Write(0x31, prebuffer);
        var buffer = new byte[78];
        buffer[0] = 0x31;
        ByteArrayUtil.SetBytes(buffer, 1, prebuffer);
        return buffer;
    }
}
