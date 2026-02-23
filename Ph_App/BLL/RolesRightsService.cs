using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Text;

namespace Ph_App.BLL
{
    // Simple roles-rights service persisted to a JSON file next to the exe.
    // Rights are a map: role -> set of allowed module keys.
    public static class RolesRightsService
    {
    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? ".", "roles_rights.xml");
        private static Dictionary<string, HashSet<string>> _rights = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        public static void InitializeIfNeeded()
        {
            if (_rights != null && _rights.Any()) return;
            Load();
            // If file missing or empty, ensure defaults for Admin/Cashier
            if (!_rights.Any())
            {
                _rights["Admin"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "medicine_management",
                    "stock_management",
                    "purchase_entry",
                    "sales_pos",
                    "daily_sales_report",
                    "expiry_report",
                    "low_stock_report",
                    "profit_report",
                    "user_management",
                    "audit_logs_viewer",
                    "roles_rights"
                };

                _rights["Cashier"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "sales_pos",
                    "daily_sales_report"
                };

                Save();
            }
        }

        public static bool IsAllowed(string role, string moduleKey)
        {
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(moduleKey)) return false;
            InitializeIfNeeded();
            if (_rights.TryGetValue(role, out var set)) return set.Contains(moduleKey);
            return false;
        }

        public static Dictionary<string, List<string>> GetAll()
        {
            InitializeIfNeeded();
            return _rights.ToDictionary(kv => kv.Key, kv => kv.Value.ToList());
        }

        public static void SetRights(string role, IEnumerable<string> moduleKeys)
        {
            if (string.IsNullOrEmpty(role)) return;
            InitializeIfNeeded();
            _rights[role] = new HashSet<string>(moduleKeys ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            Save();
        }

        [Serializable]
        public class RoleEntry
        {
            public string Role { get; set; }
            public List<string> Modules { get; set; }
        }

        [Serializable]
        public class RolesRightsDto
        {
            public List<RoleEntry> Entries { get; set; }
        }

        private static void Load()
        {
            try
            {
                if (!File.Exists(FilePath)) return;
                using (var fs = File.OpenRead(FilePath))
                {
                    var serializer = new XmlSerializer(typeof(RolesRightsDto));
                    var dto = serializer.Deserialize(fs) as RolesRightsDto;
                    var dict = new Dictionary<string, List<string>>();
                    if (dto?.Entries != null)
                    {
                        foreach (var e in dto.Entries)
                        {
                            dict[e.Role] = e.Modules ?? new List<string>();
                        }
                    }
                    _rights = dict.ToDictionary(k => k.Key, v => new HashSet<string>(v.Value, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
                }
            }
            catch
            {
                _rights = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private static void Save()
        {
            try
            {
                var dict = _rights.ToDictionary(kv => kv.Key, kv => kv.Value.ToList());
                var dto = new RolesRightsDto
                {
                    Entries = dict.Select(kv => new RoleEntry { Role = kv.Key, Modules = kv.Value }).ToList()
                };
                using (var fs = File.Create(FilePath))
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    var serializer = new XmlSerializer(typeof(RolesRightsDto));
                    serializer.Serialize(sw, dto);
                }
            }
            catch
            {
                // ignore failures - optional: log
            }
        }
    }
}
