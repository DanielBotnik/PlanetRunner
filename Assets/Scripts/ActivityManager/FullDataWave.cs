using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.HeadsetModels
{
    public class FullDataWave : WaveSignal
    {
        //Values selected somewhat arbitrarily after recording the values outputed for other people in the
        //office for roughly 3 minutes while in a neutral state
        public static double MAX_THETA { get; set; }
        public static double MAX_DELTA { get; set; }
        public static double MAX_LO_ALPHA { get; set; }
        public static double MAX_HI_ALPHA { get; set; }
        public static double MAX_LO_BETA { get; set; }
        public static double MAX_HI_BETA { get; set; }
        public static double MAX_LO_GAMMA { get; set; }
        public static double MAX_HI_GAMMA { get; set; }

        private double m_DeltaWave, m_ThetaWave, m_LowAlphaWave, m_HighAlphaWave, m_LowBetaWave, m_HighBetaWave, m_LowGammaWave, m_HighGammaWave;
        private int m_Attention, m_Meditation;

        public FullDataWave() : base(WaveTypes.FULLINFO)
        {
            m_Attention = -1;
            m_Meditation = -1;
            m_LowAlphaWave = 0;
            m_HighAlphaWave = 0;
            m_LowBetaWave = 0;
            m_HighBetaWave = 0;
            m_LowGammaWave = 0;
            m_HighGammaWave = 0;
            m_ThetaWave = 0;
            m_DeltaWave = 0;
        }

        #region Actual Values
        public int Attention
        {
            get { return m_Attention; }
            set { m_Attention = value; }
        }
        public int Meditation
        {
            get { return m_Meditation; }
            set { m_Meditation = value; }
        }
        public double DeltaWave
        {
            get { return m_DeltaWave; }
            set { m_DeltaWave = value; }
        }
        public double ThetaWave
        {
            get { return m_ThetaWave; }
            set { m_ThetaWave = value; }
        }
        public double LowAlphaWave
        {
            get { return m_LowAlphaWave; }
            set { m_LowAlphaWave = value; }
        }
        public double HighAlphaWave
        {
            get { return m_HighAlphaWave; }
            set { m_HighAlphaWave = value; }
        }
        public double LowBetaWave
        {
            get { return m_LowBetaWave; }
            set { m_LowBetaWave = value; }
        }
        public double HighBetaWave
        {
            get { return m_HighBetaWave; }
            set { m_HighBetaWave = value; }
        }
        public double LowGammaWave
        {
            get { return m_LowGammaWave; }
            set { m_LowGammaWave = value; }
        }
        public double HighGammaWave
        {
            get { return m_HighGammaWave; }
            set { m_HighGammaWave = value; }
        }
        #endregion

        #region Normalized Values
        public double NormalizedTheta { get { return Clamp(m_ThetaWave, 0, MAX_THETA) / MAX_THETA; } }
        public double NormalizedDelta { get { return Clamp(m_DeltaWave, 0, MAX_DELTA) / MAX_DELTA; } }
        public double NormalizedLoAlpha { get { return Clamp(m_LowAlphaWave, 0, MAX_LO_ALPHA) / MAX_LO_ALPHA; } }
        public double NormalizedHiAlpha { get { return Clamp(m_HighAlphaWave, 0, MAX_HI_ALPHA) / MAX_HI_ALPHA; } }
        public double NormalizedLoBeta { get { return Clamp(m_LowBetaWave, 0, MAX_LO_BETA) / MAX_LO_BETA; } }
        public double NormalizedHiBeta { get { return Clamp(m_HighBetaWave, 0, MAX_HI_BETA) / MAX_HI_BETA; } }
        public double NormalizedLoGamma { get { return Clamp(m_LowGammaWave, 0, MAX_LO_GAMMA) / MAX_LO_GAMMA; } }
        public double NormalizedHiGamma { get { return Clamp(m_HighGammaWave, 0, MAX_HI_GAMMA) / MAX_HI_GAMMA; } }
        #endregion
        public override string ToString()
        {
            return "EEG full data wave recieved with the values:\n" +
                "\n Attention: " + Attention + ", Meditation:" + Meditation +
                "\n Delta: " + m_DeltaWave +
                "\n Theta: " + m_ThetaWave +
                "\n Low Alpha: " + m_LowAlphaWave +
                "\n High Alpha: " + m_HighAlphaWave +
                "\n Low Beta: " + m_LowBetaWave +
                "\n High Beta: " + m_HighBetaWave +
                "\n Low Gamma: " + m_LowGammaWave +
                "\n High Gamma:" + m_HighGammaWave;
        }
        private double Clamp(double value, double min, double max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        public void ChangeWaveData(double delta, double theta, double lowAlpha, double highAlpha, double lowBeta, double highBeta, double lowGamma, double highGamma)
        {
            m_DeltaWave = delta;
            m_ThetaWave = theta;
            m_LowAlphaWave = lowAlpha;
            m_HighAlphaWave = highAlpha;
            m_LowBetaWave = lowBeta;
            m_HighBetaWave = highBeta;
            m_LowGammaWave = lowGamma;
            m_HighGammaWave = highGamma;
        }
    }
}
