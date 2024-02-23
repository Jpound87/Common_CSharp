using Common.Utility;
using System;
using System.Text;

namespace AM_WinForms.Datam.Extensions
{
    public static class Extensions_Datam_SequenceNumber
    {
        #region Identity
        public const String ClassName = nameof(Extensions_Datam_SequenceNumber);
        #endregion

        #region Get
        public static Boolean TryGetSequenceNumber(this String requestOrResponse, out uint sequenceNumber, out String commandOrResult)
        {
            if (requestOrResponse != null && requestOrResponse.Length > 3)// [x]
            {
                int index_s = requestOrResponse.IndexOf('[') + 1;
                int index_f = requestOrResponse.IndexOf(']');
                if (index_s != -1 && index_f > index_s)// It works, do a proof if you need to.
                {
                    StringBuilder sequenceNumberText = new StringBuilder(12);
                    int commandOrResultStartIndex = index_f + 2;

                    for (; index_s < index_f; index_s++)
                    {
                        sequenceNumberText.Append(requestOrResponse[index_s]);
                    }

                    if (Utility_General.TryConvertFromHexStringToUInt(sequenceNumberText.ToString(), out sequenceNumber))
                    {
                        if (commandOrResultStartIndex < requestOrResponse.Length - 1)
                        {
                            commandOrResult = requestOrResponse.Substring(commandOrResultStartIndex);
                            return true;
                        }
                        commandOrResult = String.Empty;
                        return true;
                    }
                }
            }
            commandOrResult = null;
            sequenceNumber = 0;
            return false;
        }
        #endregion /Get
    }
}
