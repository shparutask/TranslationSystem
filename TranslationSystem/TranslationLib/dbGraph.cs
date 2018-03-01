using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace TranslationLib
{
    public class Table
    {
        Dictionary<int, Table> _fK = new Dictionary<int, Table>();

        public string[] Columns { get; }
        public Dictionary<int, Table> FK { get { return _fK; } set { if (_fK.Count == 0) _fK = value; } }
        public string Name { get; }

        public Table(string[] columns, string name)
        {
            Columns = columns;
            Name = name;
        }
    }

    public class dbGraph
    {
        public Table[] Tables_graph { get; }
        private SqlConnection sql_conn;

        public dbGraph(string address)
        {
            sql_conn = new SqlConnection(address);
            using (sql_conn)
            {
                sql_conn.Open();
                string tableName = "";
                string[] columns;
                DataTable allTablesSchemaTable = sql_conn.GetSchema("Tables");
                Tables_graph = new Table[allTablesSchemaTable.Rows.Count];
                for (int i = 0; i < Tables_graph.Length; i++)
                {
                    String[] columnRestrictions = new String[4];
                    tableName = allTablesSchemaTable.Rows[i].ItemArray[allTablesSchemaTable.Columns[2].Ordinal].ToString();
                    columnRestrictions[2] = tableName;
                    DataTable columnsTable = sql_conn.GetSchema("Columns", columnRestrictions);

                    columns = new string[columnsTable.Rows.Count];
                    int k = 0;
                    foreach (DataRow r in columnsTable.Rows)
                    {
                        columns[k] = r.ItemArray[columnsTable.Columns[3].Ordinal].ToString();
                        k++;
                    }
                    Tables_graph[i] = new Table(columns, tableName);
                }

                Dictionary<int, Table> foreignKeys;
                foreach (Table t in Tables_graph)
                {
                    foreignKeys = new Dictionary<int, Table>();
                    var keys = sql_conn.CreateCommand();
                    keys.CommandText = "select keys.constraint_column_id, keys.parent_column_id,  secound.name from sys.foreign_key_columns keys join sys.tables main on main.object_id = keys.parent_object_id join sys.tables secound on secound.object_id = keys.referenced_object_id where main.name = '" + t.Name + "'";
                    var m_keys = keys.ExecuteReader();
                    while (m_keys.Read())
                    {
                        foreach (Table t1 in Tables_graph)
                        {
                            if (m_keys[2].ToString() == t1.Name)
                                foreignKeys.Add((int)m_keys[1], t1);
                        }
                    }
                    t.FK = foreignKeys;
                    m_keys.Close();
                }
            }
        }

        public List<ValueTag> ReturnDataValues(string table)
        {
            List<ValueTag> values = new List<ValueTag>();
            using (SqlCommand cmd = new SqlCommand("Select * From " + table, sql_conn))
            {
                /*Метод ExecuteReader() класса SqlCommand возврашает
                 объект типа SqlDataReader, с помошью которого мы можем
                 прочитать все строки, возврашенные в результате выполнения запроса
                 CommandBehavior.CloseConnection - закрываем соединение после запроса
                 */
                sql_conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    //цикл по всем столбцам полученной в результате запроса таблицы
                    while (dr.Read())
                    {
                        for (int i = 0; i < dr.FieldCount; i++)
                        /*метод GetName() класса SqlDataReader позволяет получить имя столбца
                         по номеру, который передается в качестве параметра, данному методу
                         и означает номер столбца в таблице(начинается с 0)
                         */
                        {
                            values.Add(new ValueTag { Value = dr.GetValue(i).ToString().Trim(), Column = table+ "." + dr.GetName(i).ToString().Trim() });
                        }
                    }
                }
            }
            return values;
        }
    }
}
