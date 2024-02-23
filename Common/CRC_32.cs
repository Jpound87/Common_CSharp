using System;

namespace Common
{
    public class CRC_32 : IIdentifiable
    {
        #region Identity
        public const String ClassName = nameof(CRC_32);
        public String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Readonly
        private readonly bool inputByteReflection;
        private readonly bool outputBitReflection;
        private readonly bool enableOutputInversion;
        private readonly uint initial;
        private readonly uint polynomial;
        #endregion /Readonly

        #region Constructor
        public CRC_32()
        {
            inputByteReflection = true;
            outputBitReflection = false;
            enableOutputInversion = true;
            initial = 0xffffffff;
            polynomial = 0x04c11db7;
        }

        public CRC_32(uint initial, uint polynomial)
        {
            this.initial = initial;
            this.polynomial = polynomial;
        }
        #endregion /Constructor

        #region Methods
        public byte ByteReflection(byte inputByte)
        {
            int b = inputByte;

            b = ((b & 0xF0) >> 4) | ((b & 0x0F) << 4);
            b = ((b & 0xCC) >> 2) | ((b & 0x33) << 2);
            b = ((b & 0xAA) >> 1) | ((b & 0x55) << 1);

            return Convert.ToByte(b);
        }

        public uint Uint32Reflection(uint v)
        {
            uint r = v; // r will be reversed bits of v; first get LSB of v
            int s = 31; // extra shift needed at end
            for (v >>= 1; v != 0; v >>= 1)
            {
                r <<= 1;
                r |= (byte)(v & 1);
                s--;
            }
            r <<= s; // shift when v's highest bits are zero
            return r;
        }

        public uint Compute(byte[] data, int startIndex, int length)
        {
            uint crc = initial;

            if ((length % 4) != 0)
            {
                throw new ArgumentException("length needs to be divisible by 4.");
            }

            for (int index = startIndex; index < (startIndex + length); index += sizeof(uint))
            {
                byte[] inputData = new byte[sizeof(int)];
                if (inputByteReflection)
                {
                    for (int uintByteIndex = 0; uintByteIndex < sizeof(uint); uintByteIndex++)
                    {
                        inputData[uintByteIndex] = ByteReflection(data[index + uintByteIndex]);
                    }
                }
                else
                {
                    for (int uintByteIndex = 0; uintByteIndex < sizeof(uint); uintByteIndex++)
                    {
                        inputData[uintByteIndex] = data[index + uintByteIndex];
                    }
                }

                uint current = BitConverter.ToUInt32(inputData, 0);

                crc ^= current;
                // Process all the bits in input data.
                for (uint bitIndex = 0; (bitIndex < 32); ++bitIndex)
                {
                    // If the MSB for CRC == 1
                    if ((crc & 0x80000000) != 0)
                    {
                        crc = ((crc << 1) ^ polynomial);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            if (inputByteReflection != outputBitReflection)
            {
                crc = Uint32Reflection(crc);
            }

            if (enableOutputInversion)
            {
                crc ^= 0xffffffff;
            }

            return crc;
        }
        #endregion /Methods
    }
}
