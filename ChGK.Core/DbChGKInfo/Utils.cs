namespace ChGK.Core.DbChGKInfo
{
    public static class Utils
    {
        public const string Host = "http://db.chgk.info";

        public static string ToProperDbChGKInfoId(this string id)
        {
            if (string.IsNullOrEmpty(id))
                return string.Empty;

            var properId = (id.EndsWith(".txt")) ? id.Remove(id.Length - 4) : id;
            properId = properId.StartsWith("/") ? properId.Substring(1) : properId;
            properId = properId.StartsWith("tour/") ? properId.Substring(5) : properId;

            return properId;
        }
    }
}