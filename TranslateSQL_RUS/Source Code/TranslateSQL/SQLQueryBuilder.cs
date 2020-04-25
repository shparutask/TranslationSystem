using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TranslateSQL.Entities;
using TranslateSQL.SemanticModel;

namespace TranslateSQL
{
    public static class SQLQueryBuilder
    {
        public static string Build(string q)
        {
            where = new StringBuilder();
            select = new StringBuilder();
            from = new StringBuilder();
            tables.Clear();

            question = q.Replace("\"", string.Empty);

            getNamedEntities();

            var dbTerms = getTermsFromSemanticDbAnalysis();

            return buildSQLQuery(dbTerms);
        }

        //Distinguishing named entities
        private static void getNamedEntities()
        {
            var entityList = tomitaParser.GetNamedEntities(question);
            foreach (var entity in entityList)
            {
                Type type = entity.GetType();

                if (entity.GetType() == typeof(Address))
                {
                    var address = (Address)entity;
                    tables.Add("ADDRESSES");

                    if (!string.IsNullOrEmpty(address.StreetName))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" ADDRESSES.STREET like '%{address.StreetName}%'");
                    }
                    if (!string.IsNullOrEmpty(address.HouseNumber))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" ADDRESSES.HOUSENUMBER like '%{address.HouseNumber}%'");
                    }
                    if (!string.IsNullOrEmpty(address.Area))
                    {
                        tables.Add("AREAS");

                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" AREAS.NAME like '%{address.Area}%'");
                    }
                }

                if (entity.GetType() == typeof(Museum))
                {
                    var museum = (Museum)entity;
                    tables.Add("MUSEUMS");

                    if (!string.IsNullOrEmpty(museum.Name))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");

                        where.Append($" MUSEUMS.NAME like '%{museum.Name}%'");
                    }
                    if (!string.IsNullOrEmpty(museum.Description))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" MUSEUMS.DESCRIPTION like '%{museum.Description}%'");
                    }
                    if (!string.IsNullOrEmpty(museum.OpeningHours))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" MUSEUMS.OPENING_HOURS like '%{museum.OpeningHours}%'");
                    }
                    if (!string.IsNullOrEmpty(museum.ClosingHours))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" MUSEUMS.CLOSING_TIME like '%{museum.ClosingHours}%'");
                    }
                    if (!string.IsNullOrEmpty(museum.WorkingDays))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" MUSEUMS.WORKING_DAYS like '%{museum.WorkingDays}%'");
                    }
                }

                if (entity.GetType() == typeof(Park))
                {
                    Park park = (Park)entity;

                    tables.Add("PARKS");
                    if (!string.IsNullOrEmpty(park.Name))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" PARKS.NAME like '%{park.Name}%'");
                    }
                    if (!string.IsNullOrEmpty(park.Description))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" PARKS.DESCRIPTION like '%{park.Description}%'");
                    }
                    if (!string.IsNullOrEmpty(park.OpeningHours))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" PARKS.OPENING_HOURS like '%{park.OpeningHours}%'");
                    }
                    if (!string.IsNullOrEmpty(park.ClosingHours))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" PARKS.CLOSING_TIME like '%{park.ClosingHours}%'");
                    }
                    if (!string.IsNullOrEmpty(park.WorkingDays))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" PARKS.WORKING_DAYS like '%{park.WorkingDays}%'");
                    }
                }

                if (entity.GetType() == typeof(Monument))
                {
                    var monument = (Monument)entity;
                    tables.Add("MONUMENTS");

                    if (!string.IsNullOrEmpty(monument.Name))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" MONUMENTS.NAME like '%{monument.Name}%'");
                    }
                    if (!string.IsNullOrEmpty(monument.Description))
                    {
                        if (where.Length > 0)
                            where.Append(" OR");
                        where.Append($" MONUMENTS.DESCRIPTION like '%{monument.Description}%'");
                    }
                }

                if (entity.GetType() == typeof(Homestead))
                {
                    tables.Add("HOMESTEADS");
                }
            }
        }

        //Semantic DB Analysis
        private static List<string> getTermsFromSemanticDbAnalysis()
        {
            dbModel = SemanticDBModel.CreateSemanticDBModel();

            List<string> dbTerms = new List<string>();
            foreach (var element in dbModel.ProjectionTable.Where(x =>
                                                            x.NLTermList.Where(y => question.Split(' ').Contains(y)).Count() > 0))
            {
                if (tables.Intersect(element.DBTerm.Tables).Count() == 0)
                {
                    dbTerms.Add(element.DBTerm.Term);
                    tables.AddRange(element.DBTerm.Tables);
                }
            }

            tables = tables.Distinct().ToList();

            return dbTerms.Distinct().ToList();
        }

        //Build SQL Query
        private static string buildSQLQuery(List<string> dbTerms)
        {
            foreach (var term in dbTerms.Where(x => !where.ToString().Contains(x) && x.Contains("like '%") || x.Contains("=")))
            {
                if (where.Length == 0 || string.Equals(term, "OR") || string.Equals(term, "AND"))
                    where.Append($" {term}");
                else
                    where.Append($" OR {term}");
            }

            foreach (var term in dbTerms.Where(x => !x.Contains("like '%") && !x.Contains("=") && !string.Equals(x, "SELECT", StringComparison.InvariantCultureIgnoreCase)))
            {
                if (select.Length > 0)
                    select.Append($", {term}");
                else
                    select.Append($" {term}");
            }

            from.Append(joinTables(tables));

            StringBuilder result = new StringBuilder($"SELECT {select} FROM {from}");

            if (!string.IsNullOrEmpty(where.ToString()))
                result.Append($" WHERE {where}");

            return result.ToString().Replace("  ", " ");
        }

        private static string joinTables(List<string> tables)
        {
            List<string> tablesForJoin = new List<string>();
            StringBuilder joinFrom = new StringBuilder();
            var dbTables = dbModel.DatalogModel.TablesGraph.Where(x => tables.Contains(x.Name)).ToList();

            for (int i = 0; i < dbTables.Count - 1; i++)
            {
                Table tab1 = dbTables[i], tab2 = dbTables[i + 1];

                string conn = join(tab1, tab2);
                if (string.IsNullOrEmpty(conn))
                    conn = join(tab2, tab1);

                if (!string.IsNullOrEmpty(conn) && !tablesForJoin.Contains(conn))
                    tablesForJoin.Add(conn);
                else
                {
                    var compl = complexJoin(tab1, tab2, dbModel.DatalogModel);
                    foreach (var s in compl)
                        if (!string.IsNullOrEmpty(s)) tablesForJoin.Add(s);
                }
            }

            if (tablesForJoin.Count == 0)
                joinFrom.Append(tables[0]);
            else
            {
                joinFrom.Append(tablesForJoin[0]);

                for (int i = 1; i < tablesForJoin.Count; i++)
                {
                    string ex = indexSubstring(joinFrom.ToString(), tablesForJoin[i]);

                    if (string.IsNullOrEmpty(ex))
                        for (int j = 0; j < i; j++)
                        {
                            ex = indexSubstring(tablesForJoin[i], tablesForJoin[j]);

                            if (string.IsNullOrEmpty(ex))
                                ex = indexSubstring(tablesForJoin[j], tablesForJoin[i]);
                            else
                                break;
                        }

                    if (!joinFrom.ToString().Contains(tablesForJoin[i]))
                    {
                        int joinIndex = tablesForJoin[i].IndexOf("join");

                        if (string.IsNullOrEmpty(ex))
                            joinFrom.Append($" {tablesForJoin[i].Substring(joinIndex, tablesForJoin[i].Length - joinIndex)}");
                        else
                        {
                            if (joinIndex == tablesForJoin[i].LastIndexOf("join"))
                            {
                                string[] joins = tablesForJoin[i].Split();
                                joinFrom.Append($" {joins[1]} {joins[0]} on");

                                for (int k = 4; k < joins.Length; k++)
                                {
                                    joinFrom.Append($" {joins[k]}");
                                }
                            }
                            else
                                if (indexSubstring(joinFrom.ToString(), tablesForJoin[i]) != ex)
                                joinFrom.Append($" {tablesForJoin[i].Substring(tablesForJoin[i].IndexOf(ex) + ex.Length, tablesForJoin[i].Length - tablesForJoin[i].IndexOf(ex) - ex.Length)}");
                        }
                    }
                }
            }

            return joinFrom.ToString();
        }

        private static List<string> complexJoin(Table t1, Table t2, DataModel db)
        {
            List<string> joinedTables = new List<string>();
            List<Table> tables = new List<Table>() { t1 };

            foreach (var t in db.TablesGraph)
            {
                string[] col1 = foreignKeyColumn(t1, t.FK).Split(';');
                string[] col2 = foreignKeyColumn(t2, t.FK).Split(';');
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
                    var s = join(tables[j], tables[i]);
                    if (string.IsNullOrEmpty(s))
                        s = join(tables[i], tables[j]);
                    if (!string.IsNullOrEmpty(s) && !joinedTables.Contains(s))
                    {
                        foreach (var v in joinedTables)
                            if (v.Contains(s) || s.Contains(v))
                            {
                                IsBroken = true;
                                break;
                            }
                        if (!IsBroken)
                            joinedTables.Add(s);
                    }
                }
            }
            return joinedTables;
        }

        private static string join(Table t1, Table t2)
        {
            if (t1 == t2)
                return t2.Name;
            else
            {
                foreach (var f in t1.FK)
                {
                    string j = join(f.Value.T, t2);
                    if (!string.IsNullOrEmpty(j))
                        return $"{j} join {t1.Name} on {f.Value.T.Name}.{f.Value.Column} = {t1.Name}.{f.Key.ToString()}";
                }
            }
            return "";
        }

        private static string indexSubstring(string join1, string join2)
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

        private static string foreignKeyColumn(Table tab, Dictionary<string, ForeignKey> fkeys)
        {
            var result = fkeys.FirstOrDefault(x => x.Value.T == tab);
            return $"{result.Value.Column};{result.Key}";
        }


        static string tomitaDirectoryPath = @"C:\Users\sofis\Desktop\12 семестр\ВКР\TranslationSystem\Grammar";

        static TomitaParser tomitaParser = new TomitaParser(tomitaDirectoryPath);

        static SemanticDBModel dbModel;

        static StringBuilder select = new StringBuilder();

        static StringBuilder from = new StringBuilder();

        static StringBuilder where = new StringBuilder();

        static List<string> tables = new List<string>();

        static string question;
    }
}
