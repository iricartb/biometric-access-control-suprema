using System;

public enum InitialCrcValue { Zeros, NonZero1 = 0xffff, NonZero2 = 0x1D0F }

// in our EWS network we use NonZero2. Select initialvalue NonZero2 for this class in your project!
// you are free to optimise initialisation strictly to 0x1D0F (NonZero2), we keep the class like this for testing purposes.

public class CRC16calc {
    const ushort poly = 4129;
    ushort[] table = new ushort[256];
    ushort initialValue = 0;

    public ushort ComputeChecksum(byte[] bytes) {
        ushort crc = this.initialValue;
        for(int i = 0; i < bytes.Length; ++i) {
            crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
        }
        return crc;
    }

    public byte[] ComputeChecksumBytes(byte[] bytes) {
        ushort crc = ComputeChecksum(bytes);
        return new byte[] { (byte)(crc >> 8), (byte)(crc & 0x00ff) };
    }

    public CRC16calc(InitialCrcValue initialValue) {
        this.initialValue = (ushort)initialValue;
        ushort temp, a;
        for(int i = 0; i < table.Length; ++i) {
            temp = 0;
            a = (ushort)(i << 8);
            for(int j = 0; j < 8; ++j) {
                if(((temp ^ a) & 0x8000) != 0) {
                    temp = (ushort)((temp << 1) ^ poly);
                } else {
                    temp <<= 1;
                }
                a <<= 1;
            }
            table[i] = temp;
        }
    }
}