using System;
using System.Text;

namespace Common.Utility
{
    public class Utility_CiA309_3
    {
        #region 309-3 Communications Methods 
        /// <summary>
        /// This method takes a message and will extract a sequence number if one is found
        /// </summary>
        /// <param name="packet">Message to search for a sequence number</param>
        /// <param name="sequenceNumber">The sequence number to be returned by reference</param>
        /// <returns>True if sequence number is found in the given message</returns>
        public static Boolean TryExtractSequenceNum(String packet, out UInt64 sequenceNumber)
        {
            // "["<sequence>"]" <response>
            bool beginSequence = false;
            StringBuilder sequenceNumber_SB = new StringBuilder();
            for(int p = 0; p< packet.Length; p++)
            {
                if(!beginSequence && packet[p] == '[')
                {
                    beginSequence = true;
                }
                else if(packet[p] == ']')
                {
                    break;
                }
                else if(beginSequence)
                {
                    sequenceNumber_SB.Append(packet[p]);
                }
            }
            if (UInt64.TryParse(sequenceNumber_SB.ToString(), out sequenceNumber))
            {
                return true;// If its not an SDO response, its a very clever error
            }
            sequenceNumber = 0;//doesn't matter, it fails
            return false;
        }

        public static Boolean TryExtractSequenceNum(String packet, out UInt64 sequenceNum, out String message)
        {
            // "["<sequence>"]" <response>
            if (packet[0] == '[' && packet.Contains(']'))
            {// We must have these for the sequence number.
             // Now we make sure the sequence number is parsable.
                StringBuilder sequenceNoStr = new StringBuilder();
                int messageIndex = 1;
                for (; packet[messageIndex] != ']'; messageIndex++)// We already know it must have a ']' so this is safe
                {
                    sequenceNoStr.Append(packet[messageIndex]);
                }
                message = packet.Substring(messageIndex + 1).Trim();
                if (UInt64.TryParse(sequenceNoStr.ToString(), out sequenceNum))
                {
                    return true;// If its not an SDO response, its a very clever error
                }
            }
            message = packet;
            sequenceNum = 0;//doesn't matter, it fails
            return false;
        }
        #endregion
    }
}
