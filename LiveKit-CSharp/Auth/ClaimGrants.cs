namespace LiveKit_CSharp.Auth
{
    public class ClaimGrants
    {
        public string Identity { get; set; }
        public string Name { get; set; }
        public VideoGrant Video { get; set; }
        public string Sha256 { get; set; } = "HS256";
        public string Metadata { get; set; }
      
    }
}