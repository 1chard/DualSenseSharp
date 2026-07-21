internal static class BluetoothCrc
{
    private static readonly uint[] Table = CreateTable();

    public static void Write(byte reportId, byte[] reportData)
    {
        if (reportData.Length < 4)
            throw new ArgumentException(nameof(reportData));

        uint crc = 0xFFFFFFFF;

        crc = Update(crc, 0xA2);
        crc = Update(crc, reportId);

        for (int i = 0; i < reportData.Length - 4; i++)
        {
            crc = Update(crc, reportData[i]);
        }

        crc ^= 0xFFFFFFFF;

        int offset = reportData.Length - 4;

        reportData[offset + 0] = (byte)(crc);
        reportData[offset + 1] = (byte)(crc >> 8);
        reportData[offset + 2] = (byte)(crc >> 16);
        reportData[offset + 3] = (byte)(crc >> 24);
    }

    private static uint Update(uint crc, byte value)
    {
        return (crc >> 8) ^ Table[(crc ^ value) & 0xFF];
    }

    private static uint[] CreateTable()
    {
        var table = new uint[256];

        for (uint i = 0; i < 256; i++)
        {
            uint c = i;

            for (int j = 0; j < 8; j++)
            {
                c = (c & 1) != 0
                    ? 0xEDB88320u ^ (c >> 1)
                    : c >> 1;
            }

            table[i] = c;
        }

        return table;
    }
}