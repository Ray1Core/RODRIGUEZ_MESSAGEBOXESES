namespace RODRIGUEZ_MESSAGEBOXE.Models
{
    /// <summary>
    /// Plain data holder used by the Activity 7/8 demos (looping through a list
    /// and building SQL statement text). Not connected to any real table.
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // ---------- Activity 8: teaching-demo SQL text builders ----------
        // All four build plain strings for display only. Nothing here ever
        // touches a real database. Same style as User.BuildInsertStatement:
        // indexed `for` loops, string concatenation, no String.Join.

        public static string BuildInsertStatement(string[] fields, object[] values, string tbname)
        {
            string fieldList = "";
            for (int i = 0; i < fields.Length; i++)
            {
                fieldList += fields[i];
                if (i < fields.Length - 1)
                {
                    fieldList += ", ";
                }
            }

            string valuesList = "";
            for (int i = 0; i < values.Length; i++)
            {
                valuesList += Format(values[i]);
                if (i < values.Length - 1)
                {
                    valuesList += ", ";
                }
            }

            return "INSERT INTO " + tbname + " (" + fieldList + ") VALUES (" + valuesList + ");";
        }

        public static string BuildUpdateStatement(string[] fields, object[] values, string tbname, string whereClause)
        {
            string setList = "";
            for (int i = 0; i < fields.Length; i++)
            {
                setList += fields[i] + " = " + Format(values[i]);
                if (i < fields.Length - 1)
                {
                    setList += ", ";
                }
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

        // Numbers come out unquoted (22); everything else is wrapped in quotes.
        private static string Format(object value)
        {
            bool isNumber = value is int || value is long || value is double || value is decimal;
            return isNumber ? value.ToString()! : "'" + value + "'";
        }
    }
}
