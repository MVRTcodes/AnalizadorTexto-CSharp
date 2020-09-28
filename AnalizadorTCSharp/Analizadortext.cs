using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml;

namespace ProjecteM06UF1
{
    class Analizadortext
    {
        private string Directorio { get; set; }

        public Analizadortext(string dir)
        {
            this.Directorio = dir;
        }

        public void Iniciar()
        {
            Dictionary<int, string> dc = new Dictionary<int, string>();
            CrearCarpeta(dc);
        }

        private void CrearCarpeta(Dictionary<int,string> dc)
        {
            // Comprueba si la carpeta AbstractTool Existe, si no existe la crea.
            if (!Directory.Exists(this.Directorio))
            {
                Directory.CreateDirectory(this.Directorio);
                Console.WriteLine("Se ha creado el directorio AbstractTool en tu escritorio.");
                Console.WriteLine("inserta el archivo que desees analizar dentro de la carpeta AbstractTool.");
            }else this.AnalizarCarpeta(dc);
        }

        private void AnalizarCarpeta(Dictionary<int, string> dc)
        {
            Console.WriteLine("Analizando Directorio...");
            //Analiza la carpeta y extrae la ubicación de los archivos que acaben con .txt
            string[] RutasArchivos = Directory.GetFiles(this.Directorio, "*.txt");
            int c = 1;

            try
            {
                VacioArchivoExcep(RutasArchivos);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} No se encuentran archivos en la carpeta analizada.\nInserta el archivo que desees analizar dentro de la carpeta AbstractTool.",e);
                Environment.Exit(1);
            }

            
            foreach (string RA in RutasArchivos)
            {
                dc.Add(c, RA);
                Console.WriteLine(dc[c]);
                c++;
            }
            Console.WriteLine("{0} archivos encontrados", c - 1);
            this.ExtraerDatosArchivo(dc);
            

        }

        private void ExtraerDatosArchivo(Dictionary<int, string> dc)
        {
            if (File.Exists(dc[1]))
            {
                Console.WriteLine("Leyendo {0}", dc[1]);
                string content = File.ReadAllText(dc[1]);

                //Extracción de datos que nos interesan del Archivo
                FileInfo contentinfo = new FileInfo(dc[1]);

                // Nombre
                int namepoint = contentinfo.Name.IndexOf('.');
                string contentname = contentinfo.Name.Substring(0,namepoint);

                // Extension
                string contentext = contentinfo.Extension;

                // Fecha de Creación
                DateTime contentdate = contentinfo.CreationTime;

                int contentlength = 0;
                var contenttematica = new List<string>();

                // Diccionario con cada palabra y el numero de veces que se muestra
                Dictionary<string, int> contentpalabras = new Dictionary<string, int>();

                // Depuración de puntos y comas, separación del contenido de el Archivo
                char[] molestias = { ' ', '.', ',', '\n','\r','\t','/','"','?','!',':'};
                string[] contentcontador = content.Split(molestias);

                // Lista utilizada para realizar el contador de palabras en el texto
                var contentList = new List<string>();

                // Lista de Excepciones
                string[] contentexceptions = { "el", "els", "en", "es", "ets", "\'l", "l\'", "la", "les", "lo",
                    "los", "n\'", "na", "s\'", "sa", "ses", "un", "una", "unes", "uns","de","més","i","ni","o","bé","si",
                    "no", "però","sinó","que","no","obstant","això","malgrat", "ara","adés","ni","ja","altre","és","siga","doncs","per",
                    "tanmateix", "sinó", "sinó que","amb tot","a","per","al","als","del","dels","pel","pels","as","des","dets","pes","can",
                    "cal","cals","cas","tant","encara","endemés","fins","tot","mentre","després","com","tal","sempre","car","més-que", "menys","perquè","fi",
                    "son","çon","a","amb","arran","cap", "contra", "dalt", "damunt", "davall", "de", "deçà", "dellà", "des", "devers", "devora",
                    "dintre", "durant", "en", "entre", "envers", "excepte", "fins", "llevat","mitjançant", "per", "pro",
                    "salvant", "salvat", "segons", "sens", "sense", "sobre", "sota", "sots", "tret", "ultra", "via","tals","aquest","aquell","aquests","aquestes",
                    "aquelles","aquesta","seu","seva","seves","meva","meves","altre","altres","ha","dins","hi","va","li","també","qual","se","ve" };


                // Bucle para depurar espacios y palabras con apostrofes y introducirlas al diccionario
                foreach (string cc in contentcontador)
                {

                    if (cc != "")
                    {
                        
                        if (!contentpalabras.ContainsKey(cc.ToLower()))
                        {
                            string newcc = "";

                            if (cc.Length > 3 && cc[1] == '\'')
                            {
                                newcc = cc.Remove(0, 2);
                                if (!contentpalabras.ContainsKey(newcc.ToLower())) contentpalabras.Add(newcc.ToLower(), 1);
                                else contentpalabras[newcc.ToLower()] += 1;
                            }
                            else if (cc.Length > 3 && cc[cc.Length - 2] == '\'')
                            {
                                int index1 = cc.IndexOf('\'');
                                newcc = cc.Substring(0, index1);

                                if (!contentpalabras.ContainsKey(newcc.ToLower())) contentpalabras.Add(newcc.ToLower(), 1);
                                else contentpalabras[newcc.ToLower()] += 1;
                            }
                            else if (cc.Length > 1 && cc[0] == ' ')
                            {
                                newcc = cc.Remove(0, 1);
                                if (!contentpalabras.ContainsKey(newcc.ToLower())) contentpalabras.Add(newcc.ToLower(), 1);
                                else contentpalabras[newcc.ToLower()] += 1;
                            }
                            else contentpalabras.Add(cc.ToLower(), 1);
                        }
                        else
                        {
                            contentpalabras[cc.ToLower()] += 1;
                        }
                        contentList.Add(cc.ToLower());
                    }
                }

                // Bucle para eliminar todas las excepciones introducidas en el diccionario
                foreach (string escep in contentexceptions)
                {
                    if (contentpalabras.ContainsKey(escep)) contentpalabras.Remove(escep);
                }
                
                //Numero de Palabras

                contentlength = contentList.Count;

                // Variable utilizada para realizar el TOP 5 palabras mas utilizadas en el diccionario (LINQ)

                var contentitems = from pair in contentpalabras
                                   orderby pair.Value descending
                                   select pair;

                int counter = 1;
                foreach (KeyValuePair<string, int> pair in contentitems)
                {
                    //Console.WriteLine("{0}: {1}", pair.Key, pair.Value);+
                    
                    
                    contenttematica.Add(pair.Key);

                    counter++;
                    if (counter > 5) break;
                }

                this.CrearArchivo_info(contentname, contentext, contentdate, contentlength, contenttematica, dc, contentpalabras);
            }
        }

        private void CrearArchivo_info(string contentname, string contentext,DateTime contentdate,int contentlength, List<string> contenttematica, Dictionary<int,string> dc, Dictionary<string,int> contentpalabras)
        {

            // Creación archivo _info
            char[] nc = { '.', 't', 'x'};
            string infotxt = dc[1].TrimEnd(nc) + "_info.txt";
            //Console.WriteLine(infotxt);

            if (File.Exists(infotxt)) File.Delete(infotxt);
            File.WriteAllText(infotxt, "Nom del Fitxer: " + contentname +
                "\nExtensió: " + contentext + "\nData: " + contentdate +
                "\nNúmero de paraules: " + contentlength + "\nTemàtica: ");
            int cp = 1;

            // creació xml amb les tematiques
            XmlWriter contentxml = XmlWriter.Create(this.Directorio+contentname + "_xml.xml");
            contentxml.WriteStartDocument();
            contentxml.WriteStartElement("tematiques");
            
            foreach (string ct in contenttematica)
            {
                
                if (contenttematica.Count != cp) File.AppendAllText(infotxt, ct + ", ");
                else File.AppendAllText(infotxt, ct + ".");
                cp++;
            }

            var contentitems = from pair in contentpalabras
                               orderby pair.Value descending
                               select pair;

            foreach (KeyValuePair<string, int> pair in contentitems)
            {
                int value;
                if (!int.TryParse(pair.Key, out value))
                {
                    //Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                    contentxml.WriteStartElement("tematica");
                    contentxml.WriteAttributeString("ocurrencies", pair.Value.ToString());
                    contentxml.WriteString(pair.Key);
                    contentxml.WriteEndElement();
                }

            }
            contentxml.WriteEndDocument();
            contentxml.Close();
        }

        private void VacioArchivoExcep(string[] arx)
        {
            //excepció, si la carpeta esta buida es llença
            if (arx.Length == 0) throw new ArgumentOutOfRangeException();
        }
    }
}
