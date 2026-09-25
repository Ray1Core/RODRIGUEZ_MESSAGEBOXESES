namespace RODRIGUEZ_MESSAGEBOXE.Models
{
    public class User
    {
        public int    Id       { get; set; }
        public string Name     { get; set; } = string.Empty;
        public string Email    { get; set; } = string.Empty;
        public string Gender   { get; set; } = string.Empty;
        public int    Age      { get; set; }
        public string Address  { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // ---------- SQL statement builders ----------

        public static string BuildInsertStatement(string[] fields, object[] values, string tbname)
        {
            string fieldList = "";
            for (int i = 0; i < fields.Length; i++)
            {
                fieldList += fields[i];
                if (i < fields.Length - 1) fieldList += ", ";
            }

            string valuesList = "";
            for (int i = 0; i < values.Length; i++)
            {
                valuesList += Format(values[i]);
                if (i < values.Length - 1) valuesList += ", ";
            }

            return "INSERT INTO " + tbname + " (" + fieldList + ") VALUES (" + valuesList + ");";
        }

        public static string BuildUpdateStatement(string[] fields, object[] values, string tbname, string whereClause)
        {
            string setList = "";
            for (int i = 0; i < fields.Length; i++)
            {
                setList += fields[i] + " = " + Format(values[i]);
                if (i < fields.Length - 1) setList += ", ";
            }

            return "UPDATE " + tbname + " SET " + setList + " WHERE " + whereClause + ";";
        }

        public static string BuildDeleteStatement(string tbname, string whereClause)
        {
            return "DELETE FROM " + tbname + " WHERE " + whereClause + ";";
        }

        public static string BuildViewStatement(string tbname)
        {
            return "SELECT * FROM " + tbname + ";";
        }

        private static string Format(object value)
        {
            bool isNumber = value is int || value is long || value is double || value is decimal;
            return isNumber ? value.ToString()! : "'" + value + "'";
        }
    }
}
