using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiParse0._2
{
    public class FicheroConfig
    {
        //Public
        //public int iPosOrigen;
        //public string sNombreOrigen;
        //public bool bMoverSi;
        //public int iPosFinal;
        //public string sNombreFinal;
        //public string sOperacion;
        public int iNumTotalItemsOrigen;
        public int iNumTotalItemsFinal;

        public List<Linea> Listline = new List<Linea>();

        //Private
        string sFirstLine;
        StreamReader reader;
        StreamReader filereader;


        public FicheroConfig(string PathConfigFile)
        {
            //reader = new StreamReader(@"C:\juan\Dev\Musikarte\MusiParse0.2\ConfigFile.MusiParse");
            reader = new StreamReader(PathConfigFile);
            sFirstLine = reader.ReadLine();
            List<string> lnumItem = sFirstLine.Split(':').ToList();
            iNumTotalItemsOrigen = int.Parse(lnumItem[0]);
            iNumTotalItemsFinal = int.Parse(lnumItem[1]);
            //this.iPosOrigen = 0;
            //this.sNombreOrigen = string.Empty;
            //this.bMoverSi = false;
            //this.iPosFinal = 0;
            //this.sNombreFinal = string.Empty;
            //this.sOperacion = string.Empty;

        }
        public void ReadConfigFile()
        {

            string sAuxLine;
            string sPathFile = string.Empty;
           
            while (!reader.EndOfStream)
            {
                sAuxLine = reader.ReadLine();
                int iSeparator = sAuxLine.ToList().FindIndex(n => n == ';');
                Linea line = new Linea();
                if (iSeparator != 0)
                {
                    List<string> lsnumItem = sAuxLine.Split(';').ToList();
                    List<string> lsInfoFinal = lsnumItem[0].Split(',').ToList();
                    line.sIsString = lsInfoFinal[0];
                    line.iPosFinal = int.Parse(lsInfoFinal[1]);
                    line.sNombreFinal = lsInfoFinal[2];
                    if (lsInfoFinal.Count == 4)
                    {
                        line.sFinalValue = lsInfoFinal[3];
                    }
                    if (lsnumItem.Count() == 1)
                    {
                        //popupwarning no tenenemos campo destino donde copiar
                    }
                    else
                    {
                        List<string> lsInfoOrigen = lsnumItem[1].Split(',').ToList();
                        line.sTypeMove = lsInfoOrigen[0];
                        List<string> lsPosOrigen = lsInfoOrigen[1].Split('+').ToList();
                        line.iPosOrigen = new List<int>();
                        if (line.sTypeMove.Equals("M"))
                        {
                            for (int i = 0; i < lsPosOrigen.Count; i++)
                            {
                                line.iPosOrigen.Add(int.Parse(lsPosOrigen[i]));
                            }
                        }
                        else if (line.sTypeMove.Equals("FILE"))
                        {
                            sPathFile = lsInfoOrigen[1];
                            //filereader = new StreamReader(sPathFile);
                            //line.lsidcategroias = new List<string>();
                            //line.lsfamilies = new List<string>();
                            //while (!filereader.EndOfStream)
                            //{
                            //    sFirstLine = filereader.ReadLine();
                            //    List<string> lfamilies = sFirstLine.Split(';').ToList();

                            //    line.lsidcategroias.Add(lfamilies[0]);
                            //    line.lsfamilies.Add(lfamilies[1]);
                            //}


                        }
                        else
                        {
                            line.iPosOrigen.Add(int.Parse(lsInfoOrigen[1]));
                        }
                        if (!line.sTypeMove.Equals("FILE"))
                        {
                            line.sNombreOrigen = lsInfoOrigen[2];
                        }
                        if (line.sTypeMove.Equals("MUL"))
                        {
                            lsInfoOrigen[3] = lsInfoOrigen[3].Replace('.', ',');
                            line.dValueOperation = double.Parse(lsInfoOrigen[3]);
                        }
                        if (line.sTypeMove.Equals("PRE"))
                        {
                            line.sPrefix = lsInfoOrigen[3];
                        }
                        if (line.sTypeMove.Equals("FILE"))
                        {
                             sPathFile = lsInfoOrigen[3];
                            line.iPosOrigen.Add(int.Parse(lsInfoOrigen[1]));
                        }

                    }
                    if (!(sPathFile.Equals(string.Empty)))
                    {
                        filereader = new StreamReader(sPathFile);
                        line.lsidcategroias = new List<string>();
                        line.lsfamilies = new List<string>();
                        while (!filereader.EndOfStream)
                        {
                            sFirstLine = filereader.ReadLine();
                            List<string> lfamilies = sFirstLine.Split(';').ToList();

                            line.lsidcategroias.Add(lfamilies[0]);
                            line.lsfamilies.Add(lfamilies[1]);
                        }

                    }
                    Listline.Add(line);
                }
            }

          
        
        

        }

    }
}
