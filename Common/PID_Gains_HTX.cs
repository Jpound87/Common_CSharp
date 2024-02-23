using System;

namespace Common
{
    #region Struct
    /// <summary>
    /// Current-loop tuning gains
    /// </summary>
    public struct PID_Gains
    {
        #region Identity
        public const String StructName = nameof(PID_Gains);
        #endregion

        #region Accessors
        /// <summary>
        /// proportional gain
        /// </summary>
        public double Kp { get; set; }
        /// <summary>
        /// integral gain
        /// </summary>
        public double Ki { get; set; }
        /// <summary>
        /// derivative gain
        /// </summary>
        public double Kd { get; set; }

        public double OpenLoopGainMargin { get; set; }

        public double OpenLoopPhaseMargin { get; set; }

        public double ClosedLoop3dB { get; set; }

        /// <summary>
        /// 
        /// margin (degrees)
        /// </summary>
        public double PhaseMarginInDegrees { get; set; }
        /// <summary>
        /// 3-db cutoff frequency (Hz)
        /// </summary>
        public double ThreeDBFrequency { get; set; }
        #endregion
    }
    #endregion

    public static class PID_Gains_HTX
    {
        #region Identity
        public const String ClassName = nameof(PID_Gains_HTX);
        #endregion

        #region Gain Calculations
        //TODO: this should be simplified

        public static PID_Gains TuneIPID(double coilResistanceInOhms, double coilInductanceInHenrys)
        {
            PID_Gains CompensatorGains = new PID_Gains();

            const double gainModifier = 464.0 / 500.0;
            const double epsilon = 0.01;
            const double gainMarginInDB = 8;
            const double phaseMarginInputInDegrees = 45;
            const double iterationSearchDelta = 0.1;
            const double ts = 1.0 / 10000; // 0.0001

            coilResistanceInOhms /= 2.0;
            coilInductanceInHenrys /= 2.0;
            double tau = coilInductanceInHenrys / coilResistanceInOhms;
            double loopGain = (2.0 * gainModifier - 1.0) / coilResistanceInOhms / 2.0;
            double a = 1.0 - Math.Exp(-1.0 * ts / tau);
            double b = 1.0 + Math.Exp(-1.0 * ts / tau);
            double delta = 3;

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            double gh1 = 0.0, gh2 = 0.0, gh3 = 0.0, gh4 = 0.0, gh5 = 0.0;
            double x1 = 0.0, x2 = 0.0, x3 = 0.0, x4 = 0.0, x5 = 0.0;
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            double Kp, Ki, Kd, Wd;

            double omega1 = (1.0 / ts) / 2.0, omega2, omega3, omega4, omega5;
            double target1 = 180, target2, target3, target4, target5;

            while (Math.Abs(delta) >= epsilon)
            {
                double angle = (5 * Math.Atan(ts / 2 * omega1) + Math.Atan(ts / 2 * omega1 * b / a)) / Math.PI * 180;
                delta = target1 - angle;
                omega1 += Math.Sign(delta) * iterationSearchDelta;
            }

            x1 = ts / 2 * omega1;
            gh1 = 20 * (Math.Log10(loopGain * a) + 0.5 * (Math.Log10(1 + Math.Pow(x1, 2)) - Math.Log10(Math.Pow(a, 2) + Math.Pow(b * x1, 2))));
            Kp = 1 / Math.Pow(10, (gainMarginInDB + gh1) / 20);

            target2 = 180 - phaseMarginInputInDegrees - 45;
            delta = 3;
            omega2 = (1.0 / ts) / 2.0;

            while (Math.Abs(delta) >= epsilon)
            {
                double Angle = (5 * Math.Atan(ts / 2 * omega2) + Math.Atan(ts / 2 * omega2 * b / a)) / Math.PI * 180.0;
                delta = target2 - Angle;
                omega2 += Math.Sign(delta) * iterationSearchDelta;
            }

            // x2 and gh2 aren't even used?
            x2 = ts / 2 * omega2;
            gh2 = (loopGain * a * Kp) * Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b * x2, 2));

            Ki = omega2 * Kp;
            Wd = Math.Pow(omega1, 2) / omega2;
            Kd = Kp / Wd;
            Kp = Kp - Ki * ts / 2;

            omega3 = (1.0 / ts) / 2.0;
            target3 = 0;
            delta = 3;

            while (Math.Abs(delta) >= epsilon)
            {
                x3 = ts / 2 * omega3;
                gh3 = 10 * Math.Log10(Math.Pow(Kp * omega3, 2) + Math.Pow(Ki - Kd * Math.Pow(omega3, 2), 2)) - 20 * Math.Log10(omega3) + 20 * (Math.Log10(loopGain * a) + 0.5 * Math.Log10(1 + Math.Pow(x3, 2)) - 0.5 * Math.Log10(Math.Pow(a, 2) + Math.Pow(b * x3, 2)));
                delta = target3 - gh3;
                omega3 -= Math.Sign(delta) * iterationSearchDelta;
            }

            double OpenLoopPhaseMargin = -180 - (Math.Atan(Kp * omega3 / (Ki - Kd * Math.Pow(omega3, 2))) / Math.PI * 180.0 - 90 - (5 * Math.Atan(ts / 2 * omega3) + Math.Atan(ts / 2 * omega3 * b / a)) / Math.PI * 180.0);

            omega4 = (1.0 / ts) / 2.0;
            target4 = -180;
            delta = 3;

            while (Math.Abs(delta) >= epsilon)
            {
                double Angle = (Math.Atan(Kp * omega4 / (Ki - Kd * Math.Pow(omega4, 2))) / Math.PI * 180.0 - 90 - (5 * Math.Atan(ts / 2 * omega4) + Math.Atan(ts / 2 * omega4 * b / a)) / Math.PI * 180.0);
                delta = target4 - Angle;
                omega4 -= Math.Sign(delta) * iterationSearchDelta;
            }

            x4 = ts / 2 * omega4;
            gh4 = 10 * Math.Log10(Math.Pow(Kp * omega4, 2) + Math.Pow(Ki - Kd * Math.Pow(omega4, 2), 2)) - 20 * Math.Log10(omega4) + 20 * (Math.Log10(loopGain * a) + 0.5 * Math.Log10(1 + Math.Pow(x4, 2)) - 0.5 * Math.Log10(Math.Pow(a, 2) + Math.Pow(b * x4, 2)));

            double OpenLoopGainMargin = gh4;
            omega5 = (1.0 / ts) / 2;
            target5 = -3;
            delta = 3;

            while (Math.Abs(delta) >= epsilon)
            {
                x5 = ts / 2 * omega5;
                double A1 = 20 * (Math.Log10(loopGain * a) + 1.5 * Math.Log10(1 + Math.Pow(x5, 2)) + 0.5 * Math.Log10(Math.Pow(Kp, 2) + Math.Pow((-Ki / omega5 + Kd * omega5), 2)));
                double A2 = Math.Pow(2 * x5 * a + b * x5 * (1 - Math.Pow(x5, 2)) + (loopGain * a) * ((1 - 3 * Math.Pow(x5, 2)) * (Kd * omega5 - Ki / omega5) + Kp * (-3 * x5 + Math.Pow(x5, 3))), 2);
                double A3 = Math.Pow((1 - Math.Pow(x5, 2)) * a - 2 * b * Math.Pow(x5, 2) + (loopGain * a) * ((1 - 3 * Math.Pow(x5, 2)) * Kp - (Kd * omega5 - Ki / omega5) * (-3 * x5 + Math.Pow(x5, 3))), 2);
                gh5 = A1 - 20 * (0.5 * Math.Log10(A2 + A3));
                delta = target5 - gh5;
                omega5 -= Math.Sign(delta) * iterationSearchDelta;
            }

            double OpenLoopMagAt3bB = 10 * Math.Log10(Math.Pow(Kp * omega5, 2) + Math.Pow(Ki - Kd * Math.Pow(omega5, 2), 2)) - 20 * Math.Log10(omega5) + 20 * (Math.Log10(loopGain * a) + 0.5 * Math.Log10(1 + Math.Pow(x5, 2)) - 0.5 * Math.Log10(Math.Pow(a, 2) + Math.Pow(b * x5, 2)));
            double OpenLoopPhaseAt3dB = (Math.Atan(Kp * omega5 / (Ki - Kd * Math.Pow(omega5, 2))) / Math.PI * 180.0 - 90 - (5 * Math.Atan(ts / 2 * omega5) + Math.Atan(ts / 2 * omega5 * b / a)) / Math.PI * 180.0);
            double GHreal = Math.Pow(10, OpenLoopMagAt3bB / 20) * Math.Cos(OpenLoopMagAt3bB * Math.PI / 180.0);
            double GHimag = Math.Pow(10, OpenLoopMagAt3bB / 20) * Math.Sin(OpenLoopMagAt3bB * Math.PI / 180.0);
            double denomphase = Math.Atan((GHimag / (1.0 + GHreal))) * 180 / Math.PI;
            double ClosedLoop3dBFrequency = omega5 / 2.0 / Math.PI;//in Hz
            double ClosedLoopPhaseAt3dB = OpenLoopPhaseAt3dB - denomphase;

            CompensatorGains.ThreeDBFrequency = omega5 / 2.0 / Math.PI;

            CompensatorGains.Kp = Kp;
            CompensatorGains.Ki = Ki;
            CompensatorGains.Kd = Kd;
            CompensatorGains.OpenLoopGainMargin = OpenLoopGainMargin;
            CompensatorGains.OpenLoopPhaseMargin = OpenLoopPhaseMargin;
            CompensatorGains.ClosedLoop3dB = ClosedLoop3dBFrequency;

            return CompensatorGains;
        }
        #endregion
    }
}

