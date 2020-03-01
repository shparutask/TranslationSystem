using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslateSQL
{
    public static class SQLQueryBuilder
    {
        private static void tomitaRun()
        {
            ProcessStartInfo psiOpt = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = @"/c tomitaparser config.proto",
                WorkingDirectory = @"..\..\..\TranslateSQL\TomitaFiles",
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            psiOpt.WindowStyle = ProcessWindowStyle.Hidden;
            psiOpt.RedirectStandardOutput = true;
            psiOpt.CreateNoWindow = true;

            Process proc = new Process();

            proc.StartInfo = psiOpt;
            proc.Start();

            proc.WaitForExit();
        }

        private static List<string> ComplexJoin(Table t1, Table t2, DBGraph DB)
        {
            List<string> join = new List<string>();
            List<Table> tables = new List<Table>() { t1 };

            foreach (var t in DB.TablesGraph)
            {
                string[] col1 = ContainsValue(t1, t.FK).Split(';');
                string[] col2 = ContainsValue(t2, t.FK).Split(';');
                if (!string.IsNullOrEmpty(col1[0]) || !string.IsNullOrEmpty(col2[0]))
                {
                    tables.Add(t);
                    if (!string.IsNullOrEmpty(col1[0]) && !string.IsNullOrEmpty(col2[0]))
                        return new List<string>() { $"{t1.Name} join {t.Name} on {t1.Name}.{col1[0]} = {t.Name}.{col1[1]} join {t2.Name} on {t2.Name}.{col2[0]} = {t.Name}.{col2[1]}" };
                }
            }

            tables.Add(t2);

            for (int i = 0; i < tables.Count; i++)
            {
                for (int j = i + 1; j < tables.Count; j++)
                {
                    bool IsBroken = false;
                    var s = Join(tables[j], tables[i]);
                    if (string.IsNullOrEmpty(s))
                        s = Join(tables[i], tables[j]);
                    if (!string.IsNullOrEmpty(s) && !join.Contains(s))
                    {
                        foreach (var v in join)
                            if (v.Contains(s) || s.Contains(v))
                            {
                                IsBroken = true;
                                break;
                            }
                        if (!IsBroken) join.Add(s);
                    }
                }
            }
            return join;
        }

        private static string Join(Table t1, Table t2)
        {
            if (t1 == t2)
                return t2.Name;
            else
            {
                foreach (var f in t1.FK)
                {
                    string j = Join(f.Value.T, t2);
                    if (!string.IsNullOrEmpty(j))
                        return $"{j} join {t1.Name} on {f.Value.T.Name}.{f.Value.Column} = {t1.Name}.{f.Key.ToString()}";
                }
            }
            return "";
        }

        private static string IndexSubstring(string join1, string join2)
        {
            string[] joinSplit1 = join1.Split();
            string[] joinSplit2 = join2.Split();

            for (int i = 1; i < joinSplit1.Length; i++)
                for (int j = 1; j < joinSplit2.Length; j++)
                {
                    if (joinSplit1[i] == joinSplit2[j] && joinSplit1[i - 1] == joinSplit2[j - 1] && joinSplit1[i - 1] == "join")
                        return $"{joinSplit2[j - 1]} {joinSplit2[j]} on {joinSplit2[j + 2]} = {joinSplit2[j + 4]}";
                }

            return string.Empty;
        }

        private static string ContainsValue(Table tab, Dictionary<string, ForeignKey> fkeys)
        {
            var result = fkeys.FirstOrDefault(x => x.Value.T == tab);

            //if (fkeys.Values.)
            return $"{result.Value.Column};{result.Key}";

            //return string.Empty;
        }

        private static string JoinTables(List<string> tables)
        {
            DBGraph DB = DBGraph.CreateGraph("SPB_ATTRACTIONS", "SQLEXPRESS");
            List<string> join = new List<string>();
            StringBuilder JoinFrom = new StringBuilder();

            for (int i = 0; i < tables.Count - 1; i++)
            {
                Table tab1 = null, tab2 = null;
                foreach (var table in DB.TablesGraph)
                {
                    if (tables[i] == table.Name)
                        tab1 = table;
                    if (tables[i + 1] == table.Name)
                        tab2 = table;
                }

                string conn = Join(tab1, tab2);
                var compl = new List<string>();
                if (string.IsNullOrEmpty(conn)) conn = Join(tab2, tab1);
                if (!string.IsNullOrEmpty(conn))
                    join.Add(conn);
                else
                {
                    compl = ComplexJoin(tab1, tab2, DB);
                    foreach (var s in compl)
                        if (!string.IsNullOrEmpty(s)) join.Add(s);
                }
            }

            if (join.Count == 0)
                JoinFrom.Append(tables[0]);

            else
            {
                if (join.Count >= 1)
                {
                    JoinFrom.Append(join[0]);

                    for (int i = 1; i < join.Count; i++)
                    {
                        string ex = IndexSubstring(JoinFrom.ToString(), join[i]);

                        if (string.IsNullOrEmpty(ex))
                            for (int j = 0; j < i; j++)
                            {
                                ex = IndexSubstring(join[i], join[j]);
                                if (string.IsNullOrEmpty(ex)) ex = IndexSubstring(join[j], join[i]);
                                if (!string.IsNullOrEmpty(ex)) break;
                            }

                        if (!JoinFrom.ToString().Contains(join[i]))
                        {
                            int joinIndex = join[i].IndexOf("join");

                            if (string.IsNullOrEmpty(ex))
                                JoinFrom.Append($" {join[i].Substring(joinIndex, join[i].Length - joinIndex)}");
                            else
                            {
                                if (joinIndex == join[i].LastIndexOf("join"))
                                {
                                    string[] joins = join[i].Split();
                                    JoinFrom.Append($" {joins[1]} {joins[0]} on");

                                    for (int k = 4; k < joins.Length; k++)
                                    {
                                        JoinFrom.Append($" {joins[k]}");
                                    }
                                }
                                else
                                    if (IndexSubstring(JoinFrom.ToString(), join[i]) != ex)
                                    JoinFrom.Append($" {join[i].Substring(join[i].IndexOf(ex) + ex.Length, join[i].Length - join[i].IndexOf(ex) - ex.Length)}");
                            }
                        }
                    }

                }
            }

            return JoinFrom.ToString();
        }

        public static string Build(string question)
        {
            string select = "SELECT";
            string from = "FROM";
            string where = "WHERE";

            List<string> tables = new List<string>();

            using (var sw = new StreamWriter("../../../TranslateSQL/TomitaFiles/in.txt"))
            {
                sw.Write(question);
            }

            tomitaRun();

            using (var sr = new StreamReader("../../../TranslateSQL/TomitaFiles/out.txt"))
            {
                while (!string.IsNullOrEmpty(sr.ReadLine()))
                {
                    string table = sr.ReadLine();

                    sr.ReadLine();
                    switch (table)
                    {
                        case "\tTime":
                            {
                                var hoursWorkVar = "\t\tHoursWork = ";
                                var hoursWork = sr.ReadLine().Substring(hoursWorkVar.Length);

                                where += $" ";

                                break;
                            }
                        case "\tAddress":
                            {
                                var streetName = "\t\tStreetName = ";
                                var houseNum = "\t\tHouseNumber = ";

                                var street = sr.ReadLine().Substring(streetName.Length);

                                if (string.Compare(street, "район", true) == 0)
                                {
                                    var areaName = "\t\tArea = ";
                                    var area = sr.ReadLine().Substring(areaName.Length);
                                    if (string.Compare(area, "какой", true) == 0)
                                    {
                                        select += " AREAS.NAME";

                                    }

                                    tables.Add("AREAS");
                                    break;
                                }

                                if (string.Compare(street, "какой", true) == 0 || string.Compare(street, "адрес", true) == 0)
                                {
                                    select += " ADDRESSES.STREET, ADDRESSES.HOUSENUMBER";
                                }
                                else
                                {
                                    var home = sr.ReadLine().Substring(houseNum.Length);
                                    where += $" ADDRESSES.STREET like '%{street}%' AND ADDRESSES.HOUSENUMBER = {home}";
                                }

                                tables.Add("ADDRESSES");

                                break;
                            }

                        case "\tMuseum":
                            {
                                var museumName = "\t\tNAME = ";
                                var name = sr.ReadLine().Substring(museumName.Length);

                                if (string.Compare(name, "какой", true) == 0)
                                {
                                    select += " MUSEUMS.NAME";
                                }
                                else
                                {
                                    where += " MUSEUMS.NAME like '%" + name + "%'";
                                }

                                tables.Add("MUSEUMS");

                                break;
                            }

                        case "\tPark":
                            {
                                var parkName = "\t\tNAME = ";
                                var name = sr.ReadLine().Substring(parkName.Length);

                                if (string.Compare(name, "какой", true) == 0)
                                {
                                    select += " PARKS.NAME";
                                }
                                else
                                {
                                    where += $" PARKS.NAME like '%{name}%'";
                                }

                                tables.Add("PARKS");

                                break;
                            }

                        case "\tMonument":
                            {
                                var monumentName = "\t\tNAME = ";
                                var name = sr.ReadLine().Substring(monumentName.Length);

                                if (string.Compare(name, "какой", true) == 0)
                                {
                                    select += " MONUMENTS.NAME";
                                }
                                else
                                {
                                    where += $" MONUMENTS.NAME like '%{name}%'";
                                }

                                tables.Add("MONUMENTS");

                                break;
                            }
                        case "\tHomestead":
                            {
                                var homeName = "\t\tNAME = ";
                                var name = sr.ReadLine().Substring(homeName.Length);

                                if (string.Compare(name, "какой", true) == 0)
                                {
                                    select += " HOMESTEADS.NAME";
                                }
                                else
                                {
                                    where += $" HOMESTEADS.NAME like '%{name}%'";
                                }

                                tables.Add("HOMESTEADS");

                                break;
                            }
                    }

                }

                if (tables.Count == 0)
                {
                    tables.Add(select.Substring(0, select.Length - 2));
                    select = "TITLE  ";
                }

                from = $"FROM {JoinTables(tables)}";
            }

            string result = $"{select} {from} {where}";

            return result;
        }
    }
}
