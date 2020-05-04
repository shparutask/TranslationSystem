using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TranslateSQL.Entities;

namespace TranslateSQL
{
    public class TomitaParser
    {
        private string directoryPath;

        public TomitaParser(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public List<Entity> GetNamedEntities(string question)
        {
            //Write input in file
            using (var sw = new StreamWriter($"{directoryPath}//in.txt"))
            {
                sw.Write(question);
            }

            tomitaProcessRun();

            var namedEntities = getEnitiesFromOut();

            return namedEntities;
        }

        private void tomitaProcessRun()
        {
            ProcessStartInfo psiOpt = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = @"/c tomitaparser config.proto",
                WorkingDirectory = directoryPath,
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

        private List<Entity> getEnitiesFromOut()
        {
            var entities = new List<Entity>();

            using (var sr = new StreamReader($"{directoryPath}//out.txt"))
            {
                string str = sr.ReadLine();

                while (!string.IsNullOrEmpty(str))
                {
                    switch (str)
                    {
                        case "\tAddress":
                            {
                                sr.ReadLine();
                                var address = getAddress(sr);
                                entities.Add(address);

                                break;
                            }

                        case "\tMuseum":
                            {
                                sr.ReadLine();
                                var museum = getMuseum(sr);
                                entities.Add(museum);

                                break;
                            }

                        case "\tPark":
                            {
                                sr.ReadLine();
                                var park = getPark(sr);
                                entities.Add(park);

                                break;
                            }

                        case "\tMonument":
                            {
                                sr.ReadLine();
                                entities.Add(new Monument
                                {
                                    Name = sr.ReadLine().Substring("\t\tNAME = ".Length)
                                });

                                break;
                            }
                        case "\tHomestead":
                            {
                                sr.ReadLine();
                                entities.Add(new Homestead
                                {
                                    Name = sr.ReadLine().Substring("\t\tNAME = ".Length)
                                });

                                break;
                            }
                    }

                    str = sr.ReadLine();
                }
            }

            return entities;
        }

        private Address getAddress(StreamReader sr)
        {
            var streetNameString = "\t\tStreetName = ";
            var houseNumString = "\t\tHouseNumber = ";
            var areaNameString = "\t\tArea = ";
            var address = new Address();

            var s = sr.ReadLine();

            while (!string.Equals(s, "\t}"))
            {
                if (s.Contains(streetNameString))
                {
                    address.StreetName = s.Substring(streetNameString.Length);
                }

                if (s.Contains(houseNumString))
                {
                    address.HouseNumber = s.Substring(houseNumString.Length);
                }

                if (s.Contains(areaNameString))
                {
                    address.Area = s.Substring(areaNameString.Length);
                }

                s = sr.ReadLine();
            }

            return address;
        }

        private Museum getMuseum(StreamReader sr)
        {
            var museumName = "\t\tNAME = ";
            var openingHours = "\t\tOPENING_HOURS = ";
            var closingHours = "\t\tCLOSING_TIME = ";

            var museum = new Museum();

            var s = sr.ReadLine();

            while (!string.Equals(s, "\t}"))
            {
                if (s.Contains(museumName))
                {
                    museum.Name = s.Substring(museumName.Length);
                }

                if (s.Contains(openingHours))
                {
                    museum.OpeningHours = s.Substring(openingHours.Length);
                }

                if (s.Contains(closingHours))
                {
                    museum.OpeningHours = s.Substring(closingHours.Length);
                }

                s = sr.ReadLine();
            }

            return museum;
        }

        private Park getPark(StreamReader sr)
        {
            var parkName = "\t\tNAME = ";
            var openingHours = "\t\tOPENING_HOURS = ";
            var closingHours = "\t\tCLOSING_TIME = ";

            var park = new Park();

            var s = sr.ReadLine();

            while (!string.Equals(s, "\t}"))
            {
                if (s.Contains(parkName))
                {
                    park.Name = s.Substring(parkName.Length);
                }

                if (s.Contains(openingHours))
                {
                    park.OpeningHours = s.Substring(openingHours.Length);
                }

                if (s.Contains(closingHours))
                {
                    park.ClosingHours = s.Substring(closingHours.Length);
                }

                s = sr.ReadLine();
            }

            return park;
        }
    }
}
