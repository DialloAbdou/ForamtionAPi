using System.Text;

namespace AspWebApi
{
    public static class DisplayIdGenerator
    {
        public static string GenerateDisplayId(string prefix, int size = 12)
        {
            var chars = new List<char>();
            for (int i = 65; i <= 90; i++)
            {
                chars.Add((char)i);
            }
            for (int i = 97; i <= 122; i++)
            {
                chars.Add((char)i);
            }
            for (int i = 48; i <= 57; i++)
            {
                chars.Add((char)i);
            }

            StringBuilder sb = new();
            sb.Append(prefix);
            sb.Append('_');
            for (int i = 0; i < size; i++)
            {
                sb.Append(chars[Random.Shared.Next(0, chars.Count - 1)]);
            }

            return sb.ToString();
        }
    }
}
