namespace Assets.Scripts.Models.HeadsetModels
{
    public enum WaveTypes { RAWDATA, FULLINFO, BLINK }

    public class WaveSignal
    {
        private WaveTypes m_Type;

        protected WaveSignal(WaveTypes type)
        {
            m_Type = type;
        }

        public WaveTypes Type
        {
            get { return m_Type; }
        }
    }
}
