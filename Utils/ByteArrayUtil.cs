namespace DualSenseSharp.Utils;

internal sealed class ByteArrayUtil
{
	public static void SetBytes(byte[] array, int offset, byte[] bytes)
	{
		bytes.CopyTo(array.AsSpan(offset));
	}
}