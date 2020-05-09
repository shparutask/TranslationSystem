using System.Collections.Generic;
using QueryExecution;
using System.Linq;

namespace TranslateSQL.SemanticModel
{
    public class SemanticDBModel
    {

        static List<string> prepositionList = new List<string>()
        {
            "в", "на", "во", "с", "до", "В"
        };

        public static SemanticDBModel CreateSemanticDBModel()
        {
            var dbModel = new SemanticDBModel();
            var queryExecutor = new QueryExecutor(ConnectionHelper.ConnectionString);

            dbModel.DatalogModel = DataModel.CreateGraph();

            foreach (var table in dbModel.DatalogModel.TablesGraph)
            {
                foreach (var column in table.Columns)
                {
                    var result = queryExecutor.ExecuteQuery($"SELECT {column} from {table.Name}");

                    foreach (var value in result.Split('\n'))
                        foreach (var word in value.Split(' '))
                            if (!prepositionList.Contains(word) && 
                                dbModel.ProjectionTable.Where(x => x.NLTermList.Contains(word)).Count() == 0)

                                dbModel.ProjectionTable.Add(new ProjectionElement
                                {
                                    NLTermList = new List<string> { word },

                                    DBTerm = new DatalogTerm
                                    {
                                        Term = $"{table.Name}.{column} like '%{word}%'",
                                        Tables = new List<string>() { table.Name }
                                    }
                                });
                }
            }

            return dbModel;
        }

        public DataModel DatalogModel;

        public List<ProjectionElement> ProjectionTable { get; } = new List<ProjectionElement>()
        {
            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "район", "Район", "районе", "Районе"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "AREAS.NAME",
                    Tables = new List<string>(){ "AREAS" }
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "парк", "Парк", "парки", "Парки",
                    "парка", "Парка", "парков", "Парков",
                    "парку", "Парку", "паркам", "Паркам",
                    "парком", "Парком", "парками", "Парками",
                    "парке", "Парке", "парках", "Парках"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "PARKS.NAME",
                    Tables = new List<string>(){ "PARKS" }
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "памятник", "Памятник", "памятники", "Памятники",
                    "памятника", "Памятника", "памятников", "Памятников",
                    "памятнику", "Памятнику", "памятникам", "Памятникам",
                    "памятником", "Памятником", "памятниками", "Памятниками",
                    "памятнике", "Памятнике", "памятниках", "Памятниках"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "MONUMENTS.NAME",
                    Tables = new List<string>(){ "MONUMENTS" }
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "музей", "Музей", "музеи", "Музеи",
                    "музея", "Музея", "музеев", "Музеев",
                    "музею", "Музею", "музеям", "Музеям",
                    "музеем", "Музеем", "музеями", "Музеями",
                    "музее", "Музее", "музеях", "Музеях"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "MUSEUMS.NAME",
                    Tables = new List<string>(){ "MUSEUMS" }
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "Какой адрес", "Адрес", "Где находится", "находится", "находятся"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "ADDRESSES.STREET, ADDRESSES.HOUSENUMBER, AREAS.NAME",
                    Tables = new List<string>(){ "ADDRESSES", "AREAS" }
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "сколько", "Сколько"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "Count(*)",
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "Во сколько", "открывается", "Как работает", "Открыт"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "OPENING_HOURS"
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "дни", "рабочие дни"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "WORKING_DAYS"
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "закрывается", "Закрыт"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "CLOSING_HOURS"
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "и"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "AND"
                }
            },

            new ProjectionElement
            {
                NLTermList = new List<string>()
                {
                    "или"
                },
                DBTerm = new DatalogTerm
                {
                    Term = "OR"
                }
            }
        };
    }

    public class ProjectionElement
    {
        public List<string> NLTermList { get; set; }

        public string InfologTerm { get; set; }

        public DatalogTerm DBTerm { get; set; }
    }

    public class DatalogTerm
    {
        public string Term { get; set; }

        public List<string> Tables { get; set; } = new List<string>();
    }
}
