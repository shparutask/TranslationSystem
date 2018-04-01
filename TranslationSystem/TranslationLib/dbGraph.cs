using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace TranslationLib
{
    public struct ForeignKey
    {
        public Table t;
        public string column;
    }

    public class Table
    {
        Dictionary<string, ForeignKey> _fK = new Dictionary<string, ForeignKey>();

        public string[] Columns { get; }
        public Dictionary<string, ForeignKey> FK { get { return _fK; } set { if (_fK.Count == 0) _fK = value; } }
        public string Name { get; }

        public Table(string[] columns, string name)
        {
            Columns = columns;
            Name = name;
        }
    }

    public class dbGraph
    {
        public List<Table> Tables_graph { get; }
        string address;

        public dbGraph(string address)
        {
            this.address = address;
            using (SqlConnection sql_conn = new SqlConnection(address))
            {
                sql_conn.Open();
                string tableName = "";
                string[] columns;
                DataTable allTablesSchemaTable = sql_conn.GetSchema("Tables");
                Tables_graph = new List<Table>();
                for (int i = 0; i < allTablesSchemaTable.Rows.Count; i++)
                {
                    String[] columnRestrictions = new String[4];
                    tableName = allTablesSchemaTable.Rows[i].ItemArray[allTablesSchemaTable.Columns[2].Ordinal].ToString();
                    if (tableName != "sysdiagrams")
                    {
                        columnRestrictions[2] = tableName;
                        DataTable columnsTable = sql_conn.GetSchema("Columns", columnRestrictions);

                        columns = new string[columnsTable.Rows.Count];
                        int k = 0;
                        foreach (DataRow r in columnsTable.Rows)
                        {
                            columns[k] = r.ItemArray[columnsTable.Columns[3].Ordinal].ToString();
                            k++;
                        }
                        Tables_graph.Add(new Table(columns, tableName));
                    }
                }

                Dictionary<string, ForeignKey> foreignKeys;
                foreach (Table t in Tables_graph)
                {
                    foreignKeys = new Dictionary<string, ForeignKey>();
                    var keys = sql_conn.CreateCommand();
                    keys.CommandText = "select col.name, f_col.name, secound.name from sys.foreign_key_columns keys join sys.tables main on main.object_id = keys.parent_object_id join sys.tables secound on secound.object_id = keys.referenced_object_id join sys.columns f_col on keys.parent_object_id = f_col.object_id and f_col.column_id = keys.parent_column_id join sys.columns col on keys.referenced_object_id = col.object_id and col.column_id = keys.referenced_column_id where main.name = '" + t.Name + "'";
                    var m_keys = keys.ExecuteReader();
                    while (m_keys.Read())
                    {
                        foreach (Table t1 in Tables_graph)
                        {
                            if (!foreignKeys.ContainsKey(m_keys[1].ToString()) && m_keys[2].ToString() == t1.Name)
                            {
                                foreignKeys.Add(m_keys[1].ToString(), new ForeignKey { t = t1, column = m_keys[0].ToString() });
                            }
                        }
                    }
                    t.FK = foreignKeys;
                    m_keys.Close();
                }
            }
        }

        public List<ValueTag> ReturnDataValues()
        {
            List<ValueTag> values = new List<ValueTag>();
            using (SqlConnection sql_conn = new SqlConnection(address))
            {
                sql_conn.Open();
                foreach (var table in Tables_graph)
                {
                    using (SqlCommand cmd = new SqlCommand("Select * From " + table.Name, sql_conn))
                    {
                        /*Метод ExecuteReader() класса SqlCommand возврашает
                         объект типа SqlDataReader, с помошью которого мы можем
                         прочитать все строки, возврашенные в результате выполнения запроса
                         CommandBehavior.CloseConnection - закрываем соединение после запроса
                         */


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
                                    values.Add(new ValueTag { Value = dr.GetValue(i).ToString().Trim(), Column = table.Name + "." + dr.GetName(i).ToString().Trim() });
                                }
                            }
                        }
                    }
                }
            }
            return values;
        }
    }
}
