namespace TaskManagement.Api
{
    public class DatabaseOptions
    {
        public const string SectionName = "DataBaseOptions";
        public const string SystemDatabaseSectionName = "SystemDatabase";
        public const string BusinessDatabaseSectionName = "BusinessDatabase";

        public string Type { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }
}
