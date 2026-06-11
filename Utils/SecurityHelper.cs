using System;

namespace gsbMonolith.Utils
{
    /// <summary>
    /// Provides utility methods for data security and privacy compliance (e.g. GDPR).
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Masks a name by replacing the second half of its characters with asterisks (*).
        /// </summary>
        /// <param name="name">The name to mask.</param>
        /// <returns>The masked name.</returns>
        public static string MaskName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";
            if (name.Length <= 2) return name + "***";
            int keep = Math.Max(1, name.Length / 2);
            return name.Substring(0, keep) + "***";
        }
    }
}
