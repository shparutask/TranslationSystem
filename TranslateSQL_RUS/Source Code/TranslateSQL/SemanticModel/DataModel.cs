using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace TranslateSQL.SemanticModel
{
    public class DataModel
    {
        public static DataModel CreateGraph()
        {
            DataModel graph = new DataModel();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionHelper.ConnectionString))
            {
                sqlConnection.Open();

                DataTable allTablesSchemaTable = sqlConnection.GetSchema("Tables");

                foreach (DataRow row in allTablesSchemaTable.Rows)
                {
                    string tableName = row.ItemArray[allTablesSchemaTable.Columns[2].Ordinal].ToString();

                    if (tableName == "sysdiagrams")
                        continue;

                    String[] columnRestrictions = new String[4];
                    columnRestrictions[2] = tableName;
                    DataTable columnsTable = sqlConnection.GetSchema("Columns", columnRestrictions);

                    List<string> columns = new List<string>();

                    foreach (DataRow r in columnsTable.Rows)
                        columns.Add(r.ItemArray[columnsTable.Columns[3].Ordinal].ToString());

                    graph.TablesGraph.Add(new Table(columns.ToArray(), tableName));
                }

                foreach (Table t in graph.TablesGraph)
                {
                    var keys = sqlConnection.CreateCommand();
                    keys.CommandText = String.Format("{0}'{1}'", commandTextPattern, t.Name);

                    using (var m_keys = keys.ExecuteReader())
                    {
                        while (m_keys.Read())
                        {
                            foreach (Table t1 in graph.TablesGraph)
                            {
                                if (!t.FK.ContainsKey(m_keys[1].ToString()) && m_keys[2].ToString() == t1.Name)
                                {
                                    t.FK.Add(m_keys[1].ToString(), new ForeignKey { T = t1, Column = m_keys[0].ToString() });
                                }
                            }
                        }
                    }
                }

                return graph;
            }
        }

        public readonly List<Table> TablesGraph = new List<Table>();

        private static string commandTextPattern = "select col.name, f_col.name, secound.name from sys.foreign_key_columns keys join sys.tables main on main.object_id = keys.parent_object_id join sys.tables secound on secound.object_id = keys.referenced_object_id join sys.columns f_col on keys.parent_object_id = f_col.object_id and f_col.column_id = keys.parent_column_id join sys.columns col on keys.referenced_object_id = col.object_id and col.column_id = keys.referenced_column_id where main.name = ";
    }

    public class Table
    {
        private Dictionary<string, ForeignKey> foreignKey = new Dictionary<string, ForeignKey>();

        public readonly string[] Columns;

        public readonly string Name;

        public Dictionary<string, ForeignKey> FK
        {
            get
            {
                return foreignKey;
            }

            set
            {
                if (foreignKey.Count == 0)
                    foreignKey = value;
            }
        }

        public Table(string[] columns, string name)
        {
            Columns = columns;
            Name = name;
        }
    }

    public class ForeignKey
    {
        public Table T { get; set; }

        public string Column { get; set; }
    }
}
