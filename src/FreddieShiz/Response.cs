namespace FreddieShiz
{
    public struct Response
    {
        public bool CorrectionMade { get; private set; }
        public string Report { get; private set; }

        public Response(bool correctionMade, string report = null)
        {
            this.Report = report;
            this.CorrectionMade = correctionMade;
        }
    }

}
