using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace TranslationLib
{
    public struct ForeignKey
    {
        public int const_col;
        public Table f_table;
        public int f_col;
    }

    public class Table
    {
        string[] columns;
        public List<ForeignKey> FK { get; set; }
        public string Name { get; }

        public Table(string[] columns, string name)
        {
            this.columns = columns;
            Name = name;
            FK = new List<ForeignKey>();
        }
    }

    public class dbGraph
    {
        public Table[] Tables_graph { get; }

        public dbGraph(string address)
        {
            using (SqlConnection conn = new SqlConnection(address))
            {
                conn.Open();
                DataTable allTablesSchemaTable = conn.GetSchema("Tables");
                Tables_graph = new Table[allTablesSchemaTable.Rows.Count];
                for (int i = 0; i < Tables_graph.Length; i++)
                {
                    String[] columnRestrictions = new String[4];
                    string tableName = allTablesSchemaTable.Rows[i].ItemArray[allTablesSchemaTable.Columns[2].Ordinal].ToString();
                    columnRestrictions[2] = tableName;
                    DataTable columnsTable = conn.GetSchema("Columns", columnRestrictions);

                    string[] columns = new string[columnsTable.Rows.Count];
                    int k = 0;
                    foreach (DataRow r in columnsTable.Rows)
                    {
                        columns[k] = r.ItemArray[columnsTable.Columns[3].Ordinal].ToString();
                        k++;
                    }
                    Tables_graph[i] = new Table(columns, tableName);
                }

                foreach (Table t in Tables_graph)
                {
                    var keys = conn.CreateCommand();
                    keys.CommandText = "select keys.constraint_column_id, keys.parent_column_id,  secound.name from sys.foreign_key_columns keys join sys.tables main on main.object_id = keys.parent_object_id join sys.tables secound on secound.object_id = keys.referenced_object_id where main.name = '" + t.Name + "'";
                    var m_keys = keys.ExecuteReader();
                    while (m_keys.Read())
                    {
                        foreach (Table t1 in Tables_graph)
                        {
                            if (m_keys[2].ToString() == t1.Name)
                                t.FK.Add(new ForeignKey { f_col = (int)m_keys[0], const_col = (int)m_keys[1], f_table = t1 });
                        }
                    }
                    m_keys.Close();
                }
            }
        }
    }
}
