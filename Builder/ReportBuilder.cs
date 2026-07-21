namespace DualSenseSharp.Builder;

internal abstract class ReportBuilder
{
    protected readonly DataBuilder DataBuilder;

    internal ReportBuilder(DataBuilder dataBuilder)
    {
        DataBuilder = dataBuilder;
    }

    internal abstract byte[] Build();

}